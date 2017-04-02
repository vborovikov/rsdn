namespace Rsdn.Community.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Relay.PresentationModel;

    public sealed class PresenterLocator
    {
        private class NullProvider : IPresenterProvider
        {
            public TViewModel Get<TViewModel>() where TViewModel : Presenter
            {
                return default(TViewModel);
            }
        }

        private readonly IPresenterProvider serviceProvider;

        public PresenterLocator()
            : this(new NullProvider())
        {
        }

        public PresenterLocator(IPresenterProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public ShellPresenter Shell => this.serviceProvider.Get<ShellPresenter>();

        public DirectoryPresenter Directory => this.serviceProvider.Get<DirectoryPresenter>();

        public SigninPresenter Signin => this.serviceProvider.Get<SigninPresenter>();

        public ForumPresenter Forum => this.serviceProvider.Get<ForumPresenter>();

        public PostsPresenter Posts => this.serviceProvider.Get<PostsPresenter>();

        public VotesPresenter Votes => this.serviceProvider.Get<VotesPresenter>();
    }
}