namespace Rsdn.Configuration
{
    using System;
    using Relay.RequestModel.Default;

    internal class AppRequestDispatcher : DefaultRequestDispatcher
    {
        private const string App = "App";

        public AppRequestDispatcher(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public override string ToString() => App;
    }
}