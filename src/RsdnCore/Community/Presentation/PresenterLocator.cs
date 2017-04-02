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

        public ShellViewModel Shell => this.serviceProvider.Get<ShellViewModel>();

        public DirectoryViewModel Directory => this.serviceProvider.Get<DirectoryViewModel>();

        public SigninViewModel Signin => this.serviceProvider.Get<SigninViewModel>();

        public ForumViewModel Forum => this.serviceProvider.Get<ForumViewModel>();

        public PostsViewModel Posts => this.serviceProvider.Get<PostsViewModel>();

        public VotesViewModel Votes => this.serviceProvider.Get<VotesViewModel>();
    }
}