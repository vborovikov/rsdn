namespace Rsdn.Configuration
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using Client;
    using Client.Data;
    using Client.Data.Storage;
    using Community.Interaction.Requests;
    using Community.Presentation;
    using Community.Presentation.Infrastructure;
    using Community.Presentation.NavigationModel;
    using Relay.RequestModel;
    using Relay.RequestModel.Default;
    using SimpleInjector;
    using SimpleInjector.Diagnostics;
    using Windows.UI.Xaml.Controls;

    internal static class ObjectGraph
    {
        private class ContainerWrapper : IServiceProvider, IPresenterProvider
        {
            public ContainerWrapper(Container container)
            {
                this.Container = container;
            }

            public Container Container { get; }

            TViewModel IPresenterProvider.Get<TViewModel>()
            {
                return this.Container.GetInstance<TViewModel>();
            }

            object IServiceProvider.GetService(Type serviceType)
            {
                try
                {
                    return this.Container.GetInstance(serviceType);
                }
                catch (ActivationException)
                {
                    return null;
                }
            }
        }

        public static IServiceProvider Build(Frame presentationFrame)
        {
            var wrapper = new ContainerWrapper(new Container());

            BuildData(wrapper);
            BuildManagement(wrapper, presentationFrame);
            BuildControl(wrapper);
            BuildPresentation(wrapper);

#if DEBUG
            wrapper.Container.Verify();
#endif
            return wrapper;
        }

        private static void BuildData(ContainerWrapper wrapper)
        {
            wrapper.Container
                .RegisterSingleton<IDatabaseFactory, DatabaseFactory>();

            var dataAccessAssembly = typeof(Gateway).GetTypeInfo().Assembly;
            var baseGatewayIface = typeof(IGateway);

            var registrations =
                from type in dataAccessAssembly.GetExportedTypes()
                where type.GetTypeInfo().IsAbstract == false &&
                    type.GetInterfaces().Contains(baseGatewayIface)
                select new
                {
                    Service = type.GetInterfaces().Single(iface =>
                        iface != baseGatewayIface &&
                        baseGatewayIface.IsAssignableFrom(iface)),
                    Implementation = type
                };

            foreach (var reg in registrations)
            {
                wrapper.Container.Register(reg.Service, reg.Implementation, Lifestyle.Singleton);
            }
        }

        private static void BuildManagement(ContainerWrapper wrapper, Frame presentationFrame)
        {
            wrapper.Container
                .RegisterSingleton<INavigationService>(
                    () => new FrameNavigationService(presentationFrame, navigationStackName: "AppFrame"));

            wrapper.Container
                .RegisterSingleton<ICredentialManager, CredentialManager>();
            wrapper.Container
                .RegisterSingleton<IUpdateManager, UpdateManager>();
        }

        private static void BuildControl(ContainerWrapper wrapper)
        {
            var controlAssemblies = new[] { typeof(IGateway).GetTypeInfo().Assembly };

            wrapper.Container
                .RegisterSingleton<IRequestDispatcher>(() => new DefaultRequestDispatcher(wrapper));

            wrapper.Container
                .Register(typeof(IQueryHandler<,>), controlAssemblies);
            wrapper.Container
                .Register(typeof(IAsyncQueryHandler<,>), controlAssemblies);

            wrapper.Container
                .Register(typeof(ICommandHandler<>), controlAssemblies);
            wrapper.Container
                .Register(typeof(IAsyncCommandHandler<>), controlAssemblies);

            wrapper.Container
                .RegisterSingleton<IEventDispatcher>(() => new EventDispatcher(wrapper.Container));
            // We don't register blindly all event handlers here,
            // only presenters are registered as event handlers later in BuildPresentation

#if DEBUG
            wrapper.Container
                .RegisterDecorator(typeof(IRequestDispatcher), typeof(DebugRequestDispatcher), Lifestyle.Singleton);
#endif
        }

        private static void BuildPresentation(ContainerWrapper wrapper)
        {
            var presentationAssembly = typeof(PresenterLocator).GetTypeInfo().Assembly;
            var excludedServiceTypes = new[] {
                typeof(IDisposable), typeof(INotifyPropertyChanged),
                typeof(INavigable), typeof(ITombstone)
            };

            var registrations = (from presenterType in presentationAssembly.GetExportedTypes()
                                 let isPagePresenter = presenterType.GetTypeInfo().IsSubclassOf(typeof(NavigablePresenter))
                                 let isDialogPresenter = presenterType.GetTypeInfo().IsSubclassOf(typeof(DialogPresenter))
                                 where presenterType.GetTypeInfo().IsAbstract == false && (isPagePresenter || isDialogPresenter)
                                 select new
                                 {
                                     Services = presenterType.GetInterfaces().Except(excludedServiceTypes).ToArray(),
                                     Implementation = presenterType,
                                     Registration = isPagePresenter ?
                                        Lifestyle.Singleton.CreateRegistration(presenterType, wrapper.Container) :
                                        Lifestyle.Transient.CreateRegistration(presenterType, wrapper.Container),
                                     IsTransient = isDialogPresenter,
                                 }).ToArray();

            foreach (var reg in registrations)
            {
                wrapper.Container.AddRegistration(reg.Implementation, reg.Registration);
                if (reg.IsTransient)
                {
                    reg.Registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent,
                        justification: "Dialog presenter lifetime is handled by DialogManager.");
                }
            }

            // Mostly these are event handler interfaces
            var serviceTypes = registrations.SelectMany(reg => reg.Services).Distinct();
            foreach (var item in serviceTypes)
            {
                var serviceType = item;
                var implRegistrations = from reg in registrations
                                        where reg.Services.Contains(serviceType)
                                        select reg.Registration;

                foreach (var reg in implRegistrations)
                    wrapper.Container.AddRegistration(serviceType, reg);
            }

            wrapper.Container
                .RegisterSingleton<IDialogManager, DialogManager>();
            wrapper.Container
                .RegisterSingleton(() => new PresenterLocator(wrapper));
        }
    }
}