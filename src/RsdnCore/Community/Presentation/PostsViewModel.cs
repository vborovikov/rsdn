﻿namespace Rsdn.Community.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Interaction;
    using Interaction.Requests.Posts;
    using NavigationModel;
    using Relay.RequestModel;

    [Navigable("Rsdn.Xaml.ActivityPage, Rsdn")]
    public class PostsViewModel : ActivityViewModel
    {
        public PostsViewModel(IPresenterHost host) : base(host)
        {
        }

        public override string Name => "Posts";

        protected override IQuery<IEnumerable<ThreadDetails>> GetThreadsQuery()
        {
            return new PostsQuery();
        }
    }
}