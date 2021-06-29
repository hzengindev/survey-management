using Core.Utilities.Dynamics;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace Actions.Business
{
    public class SurveyManager
    {
        private IOrganizationService Service;

        public SurveyManager(IOrganizationService service)
        {
            this.Service = service;
        }

        public Entity GetSurveyById(Guid id)
        {
            var qe = new QueryExpression("hz_survey");
            qe.ColumnSet = new ColumnSet("hz_name", "hz_description", "hz_paginationtypecode", "hz_imageid", "hz_recordperpage");
            qe.NoLock = true;
            qe.TopCount = 1;
            qe.Criteria.AddCondition(new ConditionExpression("hz_surveyid", ConditionOperator.Equal, id));
            qe.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));

            var survey = Service.RetrieveFirst(qe);
            
            return survey;
        }

        public EntityCollection GetSurveyQuestionsBySurveyId(Guid surveyId)
        {
            var qe = new QueryExpression("hz_surveyquestion");
            qe.ColumnSet = new ColumnSet("hz_name", "hz_order", "hz_surveyquestiongroupid", "hz_surveyansweroptiontemplateid",
                "hz_required", "hz_description", "hz_additionalanswer", "hz_typecode", "hz_imageid");
            qe.NoLock = true;
            qe.Criteria.AddCondition(new ConditionExpression("hz_surveyid", ConditionOperator.Equal, surveyId));
            qe.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));
            qe.AddOrder("hz_order", OrderType.Ascending);

            var surveyQuestionGroupLink = qe.AddLink("hz_surveyquestiongroup", "hz_surveyquestiongroupid", "hz_surveyquestiongroupid", JoinOperator.LeftOuter);
            surveyQuestionGroupLink.EntityAlias = "surveyQuestionGroup";
            surveyQuestionGroupLink.Columns = new ColumnSet("hz_name", "hz_description", "hz_showdescription", "hz_order");

            var ec = Service.RetrieveAll(qe);
            
            return ec;
        }
    }
}
