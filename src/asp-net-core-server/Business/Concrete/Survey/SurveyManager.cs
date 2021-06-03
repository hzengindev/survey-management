using Business.Abstract.Survey;
using Core.Utilities.Caching;
using Core.Utilities.Dynamics;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace Business.Concrete.Survey
{
    public class SurveyManager : ISurveyService
    {
        IOrganizationService _service;
        ICacheService _cacheService;
        public SurveyManager(IOrganizationService service, ICacheService cacheService)
        {
            _service = service;
            _cacheService = cacheService;
        }

        public Entity GetSurveyById(Guid id)
        {
            var cacheKey = $"cache_survey_{id}";
            if (_cacheService.IsAdd(cacheKey))
                return _cacheService.Get<Entity>(cacheKey);
                
            var qe = new QueryExpression("hz_survey");
            qe.ColumnSet = new ColumnSet("hz_name", "hz_description", "hz_paginationtypecode", "hz_imageid", "hz_recordperpage");
            qe.NoLock = true;
            qe.TopCount = 1;
            qe.Criteria.AddCondition(new ConditionExpression("hz_surveyid", ConditionOperator.Equal, id));
            qe.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));

            var survey = _service.RetrieveFirst(qe);
            _cacheService.Add(cacheKey, survey, 1440);
            return survey;
        }

        public EntityCollection GetSurveyQuestionsBySurveyId(Guid surveyId)
        {
            var cacheKey = $"cache_survey_question_ec_{surveyId}";
            if (_cacheService.IsAdd(cacheKey))
                return _cacheService.Get<EntityCollection>(cacheKey);

            var qe = new QueryExpression("hz_surveyquestion");
            qe.ColumnSet = new ColumnSet("hz_name", "hz_order", "hz_surveyquestiongroupid", "hz_surveyansweroptiontemplateid",
                "hz_required", "hz_description", "hz_additionalanswer", "hz_typecode", "hz_imageid");
            qe.NoLock = true;
            qe.Criteria.AddCondition(new ConditionExpression("hz_surveyid", ConditionOperator.Equal, surveyId));
            qe.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));
            qe.AddOrder("hz_order", OrderType.Ascending);

            var ec = _service.RetrieveAll(qe);
            _cacheService.Add(cacheKey, ec, 1440);
            return ec;
        }

        //private EntityCollection GetAnswerOptions(Guid[] questionIds)
        //{
        //    var qe = new QueryExpression("hz_surveyansweroption");
        //    qe.ColumnSet = new ColumnSet("hz_name", "hz_order", "hz_imageid", "hz_surveyquestionid");
        //    qe.NoLock = true;
        //    qe.Criteria.AddCondition(new ConditionExpression("hz_surveyquestionid", ConditionOperator.In, questionIds));
        //    qe.Criteria.AddCondition(new ConditionExpression("hz_surveyansweroptiontemplateid", ConditionOperator.Null));
        //    qe.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));

        //    var ec = _service.RetrieveAll(qe);
        //    return ec;
        //}

        //private EntityCollection GetAnswerOptionsByTemplateId(Guid[] answerOptionTemplateId)
        //{
        //    var qe = new QueryExpression("hz_surveyansweroptiontemplate");
        //    qe.ColumnSet = new ColumnSet("hz_name", "hz_surveyansweroptiontemplateid");
        //    qe.NoLock = true;
        //    qe.Criteria.AddCondition(new ConditionExpression("hz_surveyansweroptiontemplateid", ConditionOperator.In, answerOptionTemplateId));
        //    qe.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));

        //    var answerOptionLink = qe.AddLink("hz_surveyansweroption", "hz_surveyansweroptiontemplateid", "hz_surveyansweroptiontemplateid", JoinOperator.Inner);
        //    answerOptionLink.EntityAlias = "answerOption";
        //    answerOptionLink.Columns = new ColumnSet("hz_surveyansweroptionid", "hz_name", "hz_order", "hz_imageid");
        //    answerOptionLink.LinkCriteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));
        //    answerOptionLink.LinkCriteria.AddCondition(new ConditionExpression("hz_surveyquestionid", ConditionOperator.Null));

        //    var ec = _service.RetrieveAll(qe);
        //    return ec;
        //}
    }
}