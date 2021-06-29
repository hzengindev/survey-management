using Actions.Base;
using Actions.Business;
using Actions.Model.GetSurvey;
using Core.Utilities.Serialize;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;

namespace Actions
{
    public sealed class GetSurvey : CodeActivity
    {
        [Input("Input Parameter")]
        public InArgument<string> InputParameter { get; set; }

        [Output("Action Result")]
        public OutArgument<string> ActionResult { get; set; }

        [Output("Data Result")]
        public OutArgument<string> DataResult { get; set; }

        /// <summary>
        /// Executes the workflow activity.
        /// </summary>
        /// <param name="executionContext">The execution context.</param>
        protected override void Execute(CodeActivityContext executionContext)
        {
            // Create the tracing service
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            if (tracingService == null)
                throw new InvalidPluginExecutionException("Failed to retrieve tracing service.");

            tracingService.Trace("Entered GetSurvey.Execute(), Activity Instance Id: {0}, Workflow Instance Id: {1}",
                executionContext.ActivityInstanceId,
                executionContext.WorkflowInstanceId);

            // Create the context
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();

            if (context == null)
                throw new InvalidPluginExecutionException("Failed to retrieve workflow context.");

            tracingService.Trace("GetSurvey.Execute(), Correlation Id: {0}, Initiating User: {1}",
                context.CorrelationId,
                context.InitiatingUserId);

            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            ActionWrapper.ExecuteWithData(tracingService, executionContext, this.ActionResult, this.DataResult, () =>
            {
                var _in = Serializer.Deserialize<GetSurveyIn>(executionContext.GetValue(this.InputParameter));
                using (var handler = new GetSurveyHandler(service, context.UserId))
                {
                    var result = handler.Handle(_in);
                    executionContext.SetValue(this.ActionResult, result.ActionResultSerialize);
                    executionContext.SetValue(this.DataResult, result.DataResultSerialize);
                }
            });

            tracingService.Trace("Exiting GetSurvey.Execute(), Correlation Id: {0}", context.CorrelationId);
        }
    }
}