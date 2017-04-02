namespace Rsdn.Janus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    internal partial class JanusATSoapClient
    {
        public JanusATSoapClient(string remoteAddress)
            : this(EndpointConfiguration.JanusATSoap, remoteAddress)
        {
        }
    }
}