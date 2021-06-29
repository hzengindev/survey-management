using Microsoft.VisualStudio.TestTools.UnitTesting;
using Actions.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Tooling.Connector;
using Actions.UnitTest;
using Actions.Model.GetSurvey;
using Microsoft.Xrm.Sdk;
using Core.Utilities.Serialize;

namespace Actions.Business.Tests
{
    [TestClass()]
    public class GetSurveyHandlerTests
    {
        private CrmServiceClient crmServiceClient;
        private GetSurveyHandler handler;

        [TestInitialize]
        public void Initialize()
        {
            crmServiceClient = ConnectionHelper.GetCrmServiceClient();
            handler = new GetSurveyHandler(crmServiceClient, Guid.Parse("36fac482-c8e2-4d46-9889-18533085b7f9"));
        }

        [TestMethod()]
        public void HandleTest()
        {
            var result = handler.Handle(new GetSurveyIn()
            {
                SurveyRequestId = Guid.Parse("e4f057b0-bbc2-eb11-bacc-0022489c58e9")
            });

            if (!result.ActionResult.Success)
                Assert.Fail(result.ActionResult.Message);
        }

        [TestMethod()]
        public void HandleTestByAction()
        {
            var request = new OrganizationRequest();
            request.RequestName = "hz_GetSurvey";
            request.Parameters["InputParameter"] = Serializer.Serialize<GetSurveyIn>(new GetSurveyIn() { SurveyRequestId = Guid.Parse("e4f057b0-bbc2-eb11-bacc-0022489c58e9") });

            var response = crmServiceClient.Execute(request);
            ;
        }
    }
}