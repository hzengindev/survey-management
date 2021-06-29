using Microsoft.Xrm.Tooling.Connector;
using System;

namespace Actions.UnitTest
{
    public static class ConnectionHelper
    {
        public static CrmServiceClient GetCrmServiceClient()
        {
            var URI = new Uri("<uri>");
            var clientId = "<clientId>";
            var secketKey = "<secretKey>";
            var uniqueInstance = false;

            var crmServiceClient = new CrmServiceClient(URI, clientId, secketKey, uniqueInstance, null);
            return crmServiceClient;
        }
    }
}
