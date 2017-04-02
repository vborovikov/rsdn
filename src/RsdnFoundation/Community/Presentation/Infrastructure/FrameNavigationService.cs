namespace Rsdn.Community.Presentation.Infrastructure
{
    using System;
    using System.Linq;
    using System.Reflection;
    using NavigationModel;
    using Windows.UI.Xaml.Controls;
    using Xaml.Interactivity;

    /// <summary>
    /// Provides default implementation for the <see cref="INavigationService"/> interface.
    /// </summary>
    public class FrameNavigationService : INavigationService
    {
        private readonly Frame frame;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameNavigationService"/> class.
        /// </summary>
        /// <param name="frame">The navigation service.</param>
        public FrameNavigationService(Frame frame, string navigationStackName)
        {
            this.frame = frame;
            SuspensionManager.RegisterFrame(this.frame, navigationStackName);
        }

        /// <summary>
        /// Gets a value that indicates whether there is at least one entry in back navigation history.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if there is at least one entry in back navigation history; otherwise, <c>false</c>.
        /// </value>
        public bool CanGoBack
        {
            get
            {
                return this.frame.CanGoBack;
            }
        }

        /// <summary>
        /// Navigates to the most recent entry in back navigation history, if there is one.
        /// </summary>
        public void GoBack()
        {
            this.frame.GoBack();
        }

        /// <summary>
        /// Remove the most recent entry from the back stack.
        /// </summary>
        public void RemoveBackEntry()
        {
            if (this.frame.BackStack.Any())
            {
                this.frame.BackStack.Remove(this.frame.BackStack.First());
            }
        }

        /// <summary>
        /// Navigates to a view associated with the specified <see cref="T:NavigatorViewModel">context</see>.
        /// </summary>
        /// <param name="target">The target view name.</param>
        /// <param name="parameter">The context view model.</param>
        public void Navigate(Type targetType, string target = null, object parameter = null)
        {
            var contentType = GetContentType(target ?? NavigableAttribute.DefaultTarget, targetType);
            this.frame.Navigate(contentType, parameter);
        }

        private static Type GetContentType(string target, Type targetType)
        {
            var navigableAttr = targetType.GetTypeInfo()
                .GetCustomAttributes(typeof(NavigableAttribute), inherit: true)
                .Cast<NavigableAttribute>()
                .SingleOrDefault(attr => attr.Target == target);

            if (navigableAttr == null)
                throw new InvalidOperationException();

            return Type.GetType(navigableAttr.ContentType);
        }
    }
}