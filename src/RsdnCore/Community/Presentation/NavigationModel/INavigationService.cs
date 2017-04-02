namespace Rsdn.Community.Presentation.NavigationModel
{
    using System;

    /// <summary>
    /// Enables <see cref="NavigablePresenter">presenters</see> to support and reflect view navigation.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Gets a value that indicates whether there is at least one entry in back navigation history.
        /// </summary>
        /// <value>
        ///		<c>true</c> if there is at least one entry in back navigation history; otherwise, <c>false</c>.
        /// </value>
        bool CanGoBack { get; }

        /// <summary>
        /// Navigates to the most recent entry in back navigation history, if there is one.
        /// </summary>
        void GoBack();

        /// <summary>
        /// Remove the most recent entry from the back stack.
        /// </summary>
        void RemoveBackEntry();

        /// <summary>
        /// Navigates to a view associated with the specified <see cref="NavigablePresenter">context</see>.
        /// </summary>
        /// <param name="targetType">Type of the target presenter.</param>
        /// <param name="target">The target view name.</param>
        /// <param name="parameter">The parameter.</param>
        void Navigate(Type targetType, string target = null, object parameter = null);
    }
}