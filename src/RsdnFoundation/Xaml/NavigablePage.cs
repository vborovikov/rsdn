namespace Rsdn.Xaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Community.Presentation.NavigationModel;
    using Interactivity;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    public class NavigablePage : Page
    {
        private bool isNewInstance;
        private string pageKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigablePage"/> class.
        /// </summary>
        public NavigablePage()
        {
            this.isNewInstance = true;
        }

        /// <summary>
        /// Called when a page is no longer the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var navigable = this.DataContext as INavigable;
            if (navigable != null)
            {
                var serializedTask = Task.CompletedTask;
                if (e.NavigationMode != NavigationMode.Back)
                {
                    serializedTask = HandleSerializing(navigable);
                }

                RunAfter(serializedTask, () => navigable.OnNavigatedFromAsync(null));
            }
        }

        /// <summary>
        /// Called when a page becomes the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var navigable = this.DataContext as INavigable;
            if (navigable != null)
            {
                var navigatedTask = navigable.OnNavigatedToAsync(e.NavigationMode == NavigationMode.Back);

                if (this.isNewInstance == false ||
                    HandleDeserializing(navigable, e, navigatedTask) == false)
                {
                    var tombstone = navigable as ITombstone;
                    if (tombstone != null)
                    {
                        IDictionary<string, object> state = null;

                        if (e.Parameter != null)
                        {
                            state = new Dictionary<string, object>
                            {
                                { String.Empty, e.Parameter }
                            };
                        }

                        RunAfter(navigatedTask,
                            () => tombstone.OnDeserializingAsync(state ?? new Dictionary<string, object>()));
                    }
                }
            }

            this.isNewInstance = false;
        }

        private static Task RunAfter(Task first, Func<Task> next)
        {
            var tcs = new TaskCompletionSource<object>();
            first.ContinueWith(delegate
            {
                if (first.IsFaulted) tcs.TrySetException(first.Exception.InnerExceptions);
                else if (first.IsCanceled) tcs.TrySetCanceled();
                else
                {
                    try
                    {
                        next().ContinueWith(t =>
                        {
                            if (t.IsFaulted) tcs.TrySetException(t.Exception.InnerExceptions);
                            else if (t.IsCanceled) tcs.TrySetCanceled();
                            else tcs.TrySetResult(null);
                        }, TaskContinuationOptions.ExecuteSynchronously);
                    }
                    catch (Exception exc) { tcs.TrySetException(exc); }
                }
            }, TaskContinuationOptions.ExecuteSynchronously);
            return tcs.Task;
        }

        /// <summary>
        /// Handles the page deserializing.
        /// </summary>
        /// <param name="navigable">The navigator view model.</param>
        private bool HandleDeserializing(INavigable navigable, NavigationEventArgs e, Task navigatedTask)
        {
            var frameState = SuspensionManager.SessionStateForFrame(this.Frame);
            this.pageKey = "Page-" + this.Frame.BackStackDepth;

            if (e.NavigationMode == NavigationMode.New)
            {
                // Clear existing state for forward navigation when adding a new page to the navigation stack
                var nextPageKey = this.pageKey;
                int nextPageIndex = this.Frame.BackStackDepth;
                while (frameState.Remove(nextPageKey))
                {
                    nextPageIndex++;
                    nextPageKey = "Page-" + nextPageIndex;
                }
            }
            else
            {
                var tombstone = navigable as ITombstone;
                if (tombstone != null)
                {
                    // Pass the navigation parameter and preserved page state to the page, using the same strategy
                    // for loading suspended state and recreating pages discarded from cache
                    var state = (Dictionary<string, object>)frameState[this.pageKey];
                    var hasState = state.Any();

                    if (hasState)
                    {
                        RunAfter(navigatedTask, () => tombstone.OnDeserializingAsync(state));
                    }

                    return hasState;
                }
            }

            // New page, no state, or no support for managing state
            return false;
        }

        /// <summary>
        /// Handles the page serializing.
        /// </summary>
        /// <param name="navigable">The navigator view model.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.Navigation.NavigationEventArgs" /> instance
        /// containing the event data.</param>
        private Task HandleSerializing(INavigable navigable)
        {
            var tombstone = navigable as ITombstone;
            if (tombstone != null)
            {
                var frameState = SuspensionManager.SessionStateForFrame(this.Frame);
                var pageState = new Dictionary<String, Object>();

                return tombstone.OnSerializingAsync(pageState).ContinueWith(delegate
                {
                    frameState[this.pageKey] = pageState;
                });
            }

            return Task.CompletedTask;
        }
    }
}