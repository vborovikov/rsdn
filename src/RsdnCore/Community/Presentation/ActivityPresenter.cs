namespace Rsdn.Community.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Interaction;
    using NavigationModel;
    using Relay.RequestModel;

    public abstract class ActivityPresenter : NavigablePresenter
    {
        protected readonly IPresenterHost host;
        private ItemsState threadsState;
        private ThreadPresenter currentThread;

        protected ActivityPresenter(IPresenterHost host)
        {
            this.host = host;
            this.Threads = new ObservableCollection<ThreadPresenter>();
        }

        public abstract string Name { get; }

        public ObservableCollection<ThreadPresenter> Threads { get; }

        public ThreadPresenter CurrentThread
        {
            get
            {
                return this.currentThread;
            }
            set
            {
                PrepareThreadAsync(value);

                this.currentThread = value;
                RaisePropertyChanged(nameof(this.CurrentThread));
            }
        }

        public ItemsState ThreadsState
        {
            get
            {
                return this.threadsState;
            }
            private set
            {
                if (this.threadsState != value)
                {
                    this.threadsState = value;
                    RaisePropertyChanged(nameof(this.ThreadsState));
                }
            }
        }

        internal async Task LoadThreadsAsync()
        {
            this.ThreadsState = ItemsState.Loading;
            this.ThreadsState = await PresenterExtensions.RefreshItemsAsync(
                this.Threads,
                this.host.RunQueryAsync(GetThreadsQuery()),
                m => new ThreadPresenter(this.host) { Model = m });

            this.CurrentThread = this.Threads.FirstOrDefault(thread => thread.IsNew) ?? this.Threads.FirstOrDefault();
        }

        protected override async Task OnNavigatedFromAsync(object parameter)
        {
            //todo: make sure these calls are executed and not interrupted by exiting the app
            if (this.currentThread != null)
                await this.currentThread.MarkAsViewedAsync();
            await base.OnNavigatedFromAsync(parameter);
        }

        protected abstract IQuery<IEnumerable<ThreadModel>> GetThreadsQuery();

        private async Task PrepareThreadAsync(ThreadPresenter newThread)
        {
            if (this.currentThread != null)
                await this.currentThread.MarkAsViewedAsync();
            if (newThread != null)
                await newThread.LoadPostsAsync();
        }
    }
}