using Actions.Business.Base;
using Actions.Model.GetSurvey;
using Actions.Models.Base;
using Core.Utilities.Constants;
using Core.Utilities.Dynamics;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Actions.Business
{
    public class GetSurveyHandler : BaseHandler
    {
        private SurveyManager _surveyManager;
        public GetSurveyHandler(IOrganizationService service, Guid userId) : base(service, userId) {
            _surveyManager = new SurveyManager(service);
        }

        public BaseDataResult<GetSurveyOut> Handle(GetSurveyIn input)
        {
            var surveyRequest = GetSurveyRequest(input.SurveyRequestId);

            if (surveyRequest == null)
                return new BaseDataResult<GetSurveyOut>(false, "There is no survey request.");

            if ((surveyRequest.GetBoolean("hz_completed") ?? false) == true)
                return new BaseDataResult<GetSurveyOut>(false, "This survey has already been completed.");
            
            if (surveyRequest.Contains("hz_expirationdate") && surveyRequest.GetDateTime("hz_expirationdate").Value < DateTime.Now)
                return new BaseDataResult<GetSurveyOut>(false, "This survey has expired.");

            if (!surveyRequest.Contains("hz_surveyid"))
                return new BaseDataResult<GetSurveyOut>(false, "There is no survey definition.");

            var survey = _surveyManager.GetSurveyById(surveyRequest.GetLookup("hz_surveyid").Id);
            if (survey == null)
                return new BaseDataResult<GetSurveyOut>(false, "There is no survey.");

            var returnValue = new GetSurveyOut(survey.Id,
                survey.GetString("hz_name"),
                survey.GetString("hz_description"),
                survey.Contains("hz_imageid") ? $"/Image/download.aspx?Entity=hz_survey&Attribute=hz_image&Id={survey.Id}&Full=true" : null,
                (PaginationType)survey.GetOptionSet("hz_paginationtypecode").Value,
                survey.GetInt("hz_recordperpage") ?? 0
                );

            returnValue.SurveyQuestions = GetSurveyQuestions(survey.Id);

            return new BaseDataResult<GetSurveyOut>(true, returnValue);
        }

        private Entity GetSurveyRequest(Guid surveyRequestId)
        {
            var qe = new QueryExpression("hz_surveyrequest");
            qe.ColumnSet = new ColumnSet("hz_name", "hz_responsiblefirstname", "hz_responsiblelastname", "hz_responsibleemail",
                "hz_surveyid", "hz_expirationdate", "hz_completed", "hz_completiondate");
            qe.NoLock = true;
            qe.TopCount = 1;
            qe.Criteria.AddCondition(new ConditionExpression("hz_surveyrequestid", ConditionOperator.Equal, surveyRequestId));
            qe.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));

            var surveyRequest = Service.RetrieveFirst(qe);
            return surveyRequest;
        }

        private List<SurveyQuestionItemModel> GetSurveyQuestions(Guid surveyId)
        {
            var ec = _surveyManager.GetSurveyQuestionsBySurveyId(surveyId);
            if (!ec.Entities.Any())
                return null;

            var answerOptions = GetAnswerOptions(ec.Entities.Select(z => z.Id).ToArray());
            var answerOptionsWithTemplate = GetAnswerOptionsByTemplateId(ec.Entities
                .Where(z => z.Contains("hz_surveyansweroptiontemplateid"))
                .Select(z => z.GetLookup("hz_surveyansweroptiontemplateid").Id).ToArray());

            


            var returnValue = new List<SurveyQuestionItemModel>();
            foreach (var item in ec.Entities)
                returnValue.Add(new SurveyQuestionItemModel(item.Id,
                    item.GetString("hz_name"),
                    item.GetString("hz_description"),
                    item.Contains("hz_imageid") ? $"/Image/download.aspx?Entity=hz_surveyquestion&Attribute=hz_image&Id={item.Id}&Full=true" : null,
                    item.GetInt("hz_order") ?? 0,
                    item.GetBoolean("hz_required") ?? false,
                    item.GetBoolean("hz_additionalanswer") ?? false,
                    (QuestionType)item.GetOptionSet("hz_typecode").Value,
                    MapSurveyQuestionGroup(item)
                    )
                {
                    AnswerOptions = MapSurveyAnswerOption((QuestionType)item.GetOptionSet("hz_typecode").Value,
                    item.Id,
                    item.GetLookup("hz_surveyansweroptiontemplateid")?.Id,
                    answerOptions,
                    answerOptionsWithTemplate)
                });

            return returnValue;
        }

        private IEnumerable<IGrouping<Guid, Entity>> GetAnswerOptions(Guid[] questionIds)
        {
            var qe = new QueryExpression("hz_surveyansweroption");
            qe.ColumnSet = new ColumnSet("hz_surveyansweroptionid", "hz_name", "hz_order", "hz_imageid", "hz_surveyquestionid");
            qe.NoLock = true;
            qe.Criteria.AddCondition(new ConditionExpression("hz_surveyquestionid", ConditionOperator.In, questionIds));
            qe.Criteria.AddCondition(new ConditionExpression("hz_surveyansweroptiontemplateid", ConditionOperator.Null));
            qe.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));

            var ec = Service.RetrieveAll(qe);
            if (!ec.Entities.Any())
                return null;

            var grouped = ec.Entities.GroupBy(z => z.GetLookup("hz_surveyquestionid").Id);
            return grouped;
        }

        private IEnumerable<IGrouping<Guid, Entity>> GetAnswerOptionsByTemplateId(Guid[] answerOptionTemplateId)
        {
            var qe = new QueryExpression("hz_surveyansweroptiontemplate");
            qe.ColumnSet = new ColumnSet("hz_name", "hz_surveyansweroptiontemplateid");
            qe.NoLock = true;
            qe.Criteria.AddCondition(new ConditionExpression("hz_surveyansweroptiontemplateid", ConditionOperator.In, answerOptionTemplateId));
            qe.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));

            var answerOptionLink = qe.AddLink("hz_surveyansweroption", "hz_surveyansweroptiontemplateid", "hz_surveyansweroptiontemplateid", JoinOperator.Inner);
            answerOptionLink.EntityAlias = "answerOption";
            answerOptionLink.Columns = new ColumnSet("hz_surveyansweroptionid", "hz_name", "hz_order", "hz_imageid");
            answerOptionLink.LinkCriteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));
            answerOptionLink.LinkCriteria.AddCondition(new ConditionExpression("hz_surveyquestionid", ConditionOperator.Null));

            var ec = Service.RetrieveAll(qe);
            if (!ec.Entities.Any())
                return null;

            var grouped = ec.Entities.GroupBy(z => z.GetKey("hz_surveyansweroptiontemplateid").Value);
            return grouped;
        }

        private List<SurveyAnswerOptionItemModel> MapSurveyAnswerOption(QuestionType questionType, Guid questionId, Guid? answerOptionTemplateId,
            IEnumerable<IGrouping<Guid, Entity>> answerOptions, IEnumerable<IGrouping<Guid, Entity>> answerOptionsWithTemplate)
        {
            if (questionType == QuestionType.Text)
                return null;

            if (questionType == QuestionType.Select || questionType == QuestionType.MultiSelect)
            {
                return answerOptionTemplateId.HasValue
                    ? MapAnswerOptionsUsingAnswerTemplateList(answerOptionTemplateId.Value, answerOptionsWithTemplate)
                    : MapAnswerOptionsUsingAnswerList(questionId, answerOptions);
            }

            return null;
        }

        private List<SurveyAnswerOptionItemModel> MapAnswerOptionsUsingAnswerList(Guid questionId, IEnumerable<IGrouping<Guid, Entity>> answerOptions)
        {
            if (answerOptions == null || !answerOptions.Any())
                return null;

            var returnValue = new List<SurveyAnswerOptionItemModel>();
            var _answerOptions = answerOptions.FirstOrDefault(z => z.Key == questionId);
            if (_answerOptions == null || !_answerOptions.Any())
                return null;
            
            foreach (var item in _answerOptions.OrderBy(z => z.GetInt("hz_order") ?? 0))
                returnValue.Add(new SurveyAnswerOptionItemModel(
                    item.GetKey("hz_surveyansweroptionid").Value,
                    item.GetString("hz_name"),
                    item.GetInt("hz_order") ?? 0,
                    item.Contains("hz_imageid") ? $"/Image/download.aspx?Entity=hz_surveyansweroption&Attribute=hz_image&Id={item.Id}&Full=true" : null));

            return returnValue;
        }

        private List<SurveyAnswerOptionItemModel> MapAnswerOptionsUsingAnswerTemplateList(Guid answerOptionTemplateId, IEnumerable<IGrouping<Guid, Entity>> answerOptions)
        {
            if (answerOptions == null || !answerOptions.Any())
                return null;

            var returnValue = new List<SurveyAnswerOptionItemModel>();
            var _answerOptions = answerOptions.FirstOrDefault(z => z.Key == answerOptionTemplateId);
            if (_answerOptions == null || !_answerOptions.Any())
                return null;

            foreach (var item in _answerOptions.OrderBy(z => z.GetInt("answerOption", "hz_order") ?? 0))
                returnValue.Add(new SurveyAnswerOptionItemModel(
                    item.GetKey("answerOption", "hz_surveyansweroptionid").Value,
                    item.GetString("answerOption", "hz_name"),
                    item.GetInt("answerOption", "hz_order") ?? 0,
                    item.Contains("answerOption.hz_imageid") ? $"/image/answeroption/{item.GetKey("answerOption", "hz_surveyansweroptionid")}" : null));

            return returnValue;
        }

        private SurveyQuestionGroup MapSurveyQuestionGroup(Entity entity)
        {
            if (!entity.Contains("hz_surveyquestiongroupid"))
                return null;

            var returnValue = new SurveyQuestionGroup(
                entity.GetLookup("hz_surveyquestiongroupid").Id,
                entity.GetString("surveyQuestionGroup", "hz_name"),
                entity.GetString("surveyQuestionGroup", "hz_description"),
                entity.GetInt("surveyQuestionGroup", "hz_order") ?? 0,
                entity.GetBoolean("surveyQuestionGroup", "hz_showdescription") ?? false);

            return returnValue;
        }
    }
}
