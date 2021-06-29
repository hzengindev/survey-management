using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace Core.Utilities.Dynamics
{
    public static class MetadataHelper
    {
        public static Dictionary<int, string> GetGlobalOptionSetValues(IOrganizationService service, string optionSetName)
        {
            var retrieveOptionSetRequest = new RetrieveOptionSetRequest { Name = optionSetName };
            var retrieveOptionSetResponse = (RetrieveOptionSetResponse)service.Execute(retrieveOptionSetRequest);
            var optionSetMetadata = (OptionSetMetadata)retrieveOptionSetResponse.OptionSetMetadata;

            var returnValue = new Dictionary<int, string>();
            foreach (var item in optionSetMetadata.Options.ToList())
                returnValue.Add(item.Value.Value, item.Label.UserLocalizedLabel.Label);
            return returnValue;
        }

        public static Dictionary<int, string> GetOptionSetValues(IOrganizationService service, string entityLogicalName, string attributeName)
        {
            var attributeRequest = new RetrieveAttributeRequest { EntityLogicalName = entityLogicalName, LogicalName = attributeName, RetrieveAsIfPublished = true };
            var attributeResponse = (RetrieveAttributeResponse)service.Execute(attributeRequest);
            var attributeMetadata = (EnumAttributeMetadata)attributeResponse.AttributeMetadata;

            var returnValue = new Dictionary<int, string>();
            foreach (var item in attributeMetadata.OptionSet.Options.ToList())
                returnValue.Add(item.Value.Value, item.Label.UserLocalizedLabel.Label);
            return returnValue;
        }
    }
}
