using Microsoft.PowerPlatform.Dataverse.Client;
using System;

namespace Core.Utilities.Dynamics
{
    public class ServiceClientWrapper
    {
        public readonly ServiceClient ServiceClient;

        public ServiceClientWrapper(string uri, string clientId, string secret)
        {
            ServiceClient = new ServiceClient(new Uri(uri), clientId, secret, true);
        }

        public ServiceClientWrapper(string uri, string clientId, string secret, bool useUniqueInstance)
        {
            ServiceClient = new ServiceClient(new Uri(uri), clientId, secret, useUniqueInstance);
        }
    }
}
