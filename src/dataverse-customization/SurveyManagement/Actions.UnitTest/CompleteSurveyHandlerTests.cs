using Actions.Model.CompleteSurvey;
using Actions.UnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Tooling.Connector;
using System;

namespace Actions.Business.Tests
{
    [TestClass()]
    public class CompleteSurveyHandlerTests
    {
        private CrmServiceClient crmServiceClient;
        private CompleteSurveyHandler handler;

        [TestInitialize]
        public void Initialize()
        {
            crmServiceClient = ConnectionHelper.GetCrmServiceClient();
            handler = new CompleteSurveyHandler(crmServiceClient, Guid.Parse("36fac482-c8e2-4d46-9889-18533085b7f9"));
        }

        [TestMethod()]
        public void HandleTest()
        {
            var result = handler.Handle(new CompleteSurveyIn()
            {
                SurveyRequestId = Guid.Parse("e4f057b0-bbc2-eb11-bacc-0022489c58e9"),
                Answers = new System.Collections.Generic.List<AnswerItemModel>()
            });

            if (!result.ActionResult.Success)
                Assert.Fail(result.ActionResult.Message);
        }
    }
}