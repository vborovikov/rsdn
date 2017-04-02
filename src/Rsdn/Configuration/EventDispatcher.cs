namespace Rsdn.Configuration
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Community.Interaction.Requests;
    using SimpleInjector;

    internal class EventDispatcher : IEventDispatcher
    {
        private readonly Container container;

        public EventDispatcher(Container container)
        {
            this.container = container;
        }

        public Task PublishAsync<TEvent>(TEvent e) where TEvent : IEvent
        {
            var syncEventHandlerTasks = from eventHandler in GetHandlers<TEvent>()
                                        select Task.Run(delegate { eventHandler.Handle(e); });

            var asyncEventHandlersTasks = from eventHandler in GetAsyncHandlers<TEvent>()
                                          select eventHandler.HandleAsync(e);

            return Task.WhenAll(syncEventHandlerTasks.Concat(asyncEventHandlersTasks));
        }

        private IEnumerable<IAsyncEventHandler<TEvent>> GetAsyncHandlers<TEvent>() where TEvent : IEvent
        {
            return GetHandlers(typeof(IAsyncEventHandler<TEvent>))
                .Cast<IAsyncEventHandler<TEvent>>()
                .ToArray();
        }

        private IEnumerable<IEventHandler<TEvent>> GetHandlers<TEvent>() where TEvent : IEvent
        {
            return GetHandlers(typeof(IEventHandler<TEvent>))
                .Cast<IEventHandler<TEvent>>()
                .ToArray();
        }

        private IEnumerable GetHandlers(Type handlerType)
        {
            var handlers = (from reg in container.GetCurrentRegistrations()
                            let assignableInterfaces = (from iface in reg.ServiceType.GetInterfaces()
                                                        where handlerType.IsAssignableFrom(iface)
                                                        select iface)
                            where assignableInterfaces.Any() || handlerType.IsAssignableFrom(reg.ServiceType)
                            select reg.GetInstance()).Distinct().ToArray();

            return handlers;
        }
    }
}