using Core.Utilities.Results;
using Core.Utilities.Serialize;
using Microsoft.Xrm.Sdk;
using System;
using System.Activities;
using System.ServiceModel;

namespace Actions.Base
{
    public class ActionWrapper
    {
        public static void Execute(ITracingService tracingService, CodeActivityContext executionContext, OutArgument actionResult, Action action)
        {
            try { action.Invoke(); }
            catch (FaultException<OrganizationServiceFault> e)
            {
                tracingService.Trace("FaultException: {0}", e.ToString());
                executionContext.SetValue(actionResult, Serializer.Serialize<ActionResult>(new ActionResult(false, e.Message)));
            }
            catch (Exception e)
            {
                tracingService.Trace("Exception: {0}", e.ToString());
                executionContext.SetValue(actionResult, Serializer.Serialize<ActionResult>(new ActionResult(false, e.Message)));
            }
        }

        public static void ExecuteWithData(ITracingService tracingService, CodeActivityContext executionContext, OutArgument actionResult, OutArgument dataResult, Action action)
        {
            try { action.Invoke(); }
            catch (FaultException<OrganizationServiceFault> e)
            {
                tracingService.Trace("FaultException: {0}", e.ToString());
                executionContext.SetValue(actionResult, Serializer.Serialize<ActionResult>(new ActionResult(false, e.Message)));
                executionContext.SetValue(dataResult, string.Empty);
            }
            catch (Exception e)
            {
                tracingService.Trace("Exception: {0}", e.ToString());
                executionContext.SetValue(actionResult, Serializer.Serialize<ActionResult>(new ActionResult(false, e.Message)));
                executionContext.SetValue(dataResult, string.Empty);
            }
        }
    }
}
