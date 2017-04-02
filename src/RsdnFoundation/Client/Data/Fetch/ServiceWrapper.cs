namespace Rsdn.Client.Data.Fetch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.Threading;
    using System.Threading.Tasks;
    using Janus;
    using Windows.Storage;

    internal class ServiceWrapper : IDisposable
    {
        private const string EndpointAddressKey = "EndpointAddress";

        private static readonly string[] EndpointAddresses = {
            "http://rsdn.ru/ws/janusAT.asmx",
            "http://rsdn.org/ws/janusAT.asmx",
        };

        private readonly JanusATSoapClient janus;
        private readonly IDisposable cancelRegistration;

        public ServiceWrapper(CancellationToken cancelToken)
        {
            var endpointAddress = EndpointAddresses.First();
            object endpointAddressObj;
            if (ApplicationData.Current.RoamingSettings.Values.TryGetValue(EndpointAddressKey, out endpointAddressObj))
            {
                endpointAddress = endpointAddressObj as string;
            }

            this.janus = new JanusATSoapClient(endpointAddress);
            this.cancelRegistration = cancelToken.Register(this.Close, useSynchronizationContext: false);
        }

        public JanusATSoapClient Service => this.janus;

        public static async Task<bool> CheckAsync()
        {
            string validAddress = null;

            foreach (var endpointAddress in EndpointAddresses)
            {
                var service = new JanusATSoapClient(endpointAddress);

                try
                {
                    await service.CheckAsync();
                    validAddress = endpointAddress;
                }
                catch (Exception)
                {
                    validAddress = null;
                }
                finally
                {
                    await service.CloseAsync();
                }

                if (String.IsNullOrWhiteSpace(validAddress) == false)
                {
                    break;
                }
            }

            if (String.IsNullOrWhiteSpace(validAddress))
                return false;

            ApplicationData.Current.RoamingSettings.Values[EndpointAddressKey] = validAddress;
            return true;
        }

        public void Dispose()
        {
            this.cancelRegistration.Dispose();
            Close();
        }

        private void Close()
        {
            var commObj = (ICommunicationObject)this.janus;
            try
            {
                if (commObj.State != CommunicationState.Faulted)
                {
                    commObj.Close();
                }
            }
            catch (CommunicationException)
            {
                commObj.Abort();
            }
            catch (TimeoutException)
            {
                commObj.Abort();
            }
            catch (Exception)
            {
                commObj.Abort();
                throw;
            }
        }
    }
}