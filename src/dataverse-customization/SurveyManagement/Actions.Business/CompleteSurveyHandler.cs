using Actions.Business.Base;
using Actions.Model.CompleteSurvey;
using Actions.Models.Base;
using Core.Utilities.Constants;
using Core.Utilities.Dynamics;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Actions.Business
{
    public class CompleteSurveyHandler : BaseHandler
    {
        private SurveyManager _surveyManager;
        public CompleteSurveyHandler(IOrganizationService service, Guid userId) : base(service, userId)
        {
            _surveyManager = new SurveyManager(service);
        }

        public BaseResult Handle(CompleteSurveyIn input)
        {
            var surveyRequest = GetSurveyRequest(input.SurveyRequestId);

            if (surveyRequest == null)
                return new BaseResult(false, "There is no survey request.");

            if ((surveyRequest.GetBoolean("hz_completed") ?? false) == true)
                return new BaseResult(true, "This survey has already been completed.");

            if (surveyRequest.Contains("hz_expirationdate") && surveyRequest.GetDateTime("hz_expirationdate").Value < DateTime.Now)
                return new BaseResult(false, "This survey has expired.");

            if (!surveyRequest.Contains("hz_surveyid"))
                return new BaseResult(false, "There is no survey definition.");

            var survey = _surveyManager.GetSurveyById(surveyRequest.GetLookup("hz_surveyid").Id);
            if (survey == null)
                return new BaseResult(false, "There is no survey.");

            var saveResult = SaveAnswers(surveyRequest.Id, survey.Id, input.Answers);
            if (!saveResult.ActionResult.Success)
                return saveResult;

            var completeResult = CompleteSurvey(surveyRequest.Id);
            return completeResult;
        }

        private BaseResult SaveAnswers(Guid surveyRequestId, Guid surveyId, List<AnswerItemModel> answers)
        {
            var requestToCreateRecords = new ExecuteTransactionRequest()
            {
                Requests = new OrganizationRequestCollection(),
                ReturnResponses = true
            };

            var questions = _surveyManager.GetSurveyQuestionsBySurveyId(surveyId);

            foreach (var question in questions.Entities)
            {
                var required = question.GetBoolean("hz_required") ?? false;
                var questionType = (QuestionType)question.GetOptionSet("hz_typecode").Value;
                var answer = answers.FirstOrDefault(z => z.QuestionId == question.Id);

                if (required && answer == null)
                    return new BaseResult(false, $"Required questions can not be empty. The question is ({question.GetString("hz_name")})");

                if (required && answer != null)
                {
                    if (questionType == QuestionType.Text && string.IsNullOrEmpty(answer.TextAnswer))
                        return new BaseResult(false, $"Required questions can not be empty. The question is ({question.GetString("hz_name")})");

                    if (questionType == QuestionType.Select && !answer.SelectAnswer.HasValue)
                        return new BaseResult(false, $"Required questions can not be empty. The question is ({question.GetString("hz_name")})");

                    if (questionType == QuestionType.MultiSelect && (answer.MultiSelectAnswer == null || !answer.MultiSelectAnswer.Any()))
                        return new BaseResult(false, $"Required questions can not be empty. The question is ({question.GetString("hz_name")})");
                }

                if (questionType == QuestionType.Text || questionType == QuestionType.Select)
                {
                    var entity = new Entity("hz_surveyresponse");
                    //entity["hz_name"] = "";
                    //entity["hz_answer"] = "";
                    entity["hz_surveyrequestid"] = new EntityReference("hz_surveyrequest", surveyRequestId);
                    entity["hz_surveyquestionid"] = new EntityReference("hz_surveyquestion", question.Id);

                    if ((question.GetBoolean("hz_additionalanswer") ?? false) && !string.IsNullOrEmpty(answer?.AdditionalAnswer))
                        entity["hz_additionalanswer"] = answer.AdditionalAnswer;

                    if (questionType == QuestionType.Text && !string.IsNullOrEmpty(answer?.TextAnswer))
                        entity["hz_textanswer"] = answer.TextAnswer;

                    if (questionType == QuestionType.Select && answer != null && answer.SelectAnswer.HasValue)
                        entity["hz_selectanswerid"] = new EntityReference("hz_surveyansweroption", answer.SelectAnswer.Value);

                    CreateRequest createRequest = new CreateRequest { Target = entity };
                    requestToCreateRecords.Requests.Add(createRequest);
                }

                if (questionType == QuestionType.MultiSelect)
                {
                    if (answer == null || answer.MultiSelectAnswer == null || !answer.MultiSelectAnswer.Any())
                    {
                        var entity = new Entity("hz_surveyresponse");
                        //entity["hz_name"] = "";
                        //entity["hz_answer"] = "";
                        entity["hz_surveyrequestid"] = new EntityReference("hz_surveyrequest", surveyRequestId);
                        entity["hz_surveyquestionid"] = new EntityReference("hz_surveyquestion", question.Id);

                        if ((question.GetBoolean("hz_additionalanswer") ?? false) && !string.IsNullOrEmpty(answer?.AdditionalAnswer))
                            entity["hz_additionalanswer"] = answer.AdditionalAnswer;

                        CreateRequest createRequest = new CreateRequest { Target = entity };
                        requestToCreateRecords.Requests.Add(createRequest);
                    }

                    if (answer != null)
                    {
                        foreach (var item in answer.MultiSelectAnswer)
                        {
                            var entity = new Entity("hz_surveyresponse");
                            //entity["hz_name"] = "";
                            //entity["hz_answer"] = "";
                            entity["hz_surveyrequestid"] = new EntityReference("hz_surveyrequest", surveyRequestId);
                            entity["hz_surveyquestionid"] = new EntityReference("hz_surveyquestion", question.Id);
                            entity["hz_multiselectanswerid"] = new EntityReference("hz_surveyansweroption", item);

                            if ((question.GetBoolean("hz_additionalanswer") ?? false) && !string.IsNullOrEmpty(answer?.AdditionalAnswer))
                                entity["hz_additionalanswer"] = answer.AdditionalAnswer;

                            CreateRequest createRequest = new CreateRequest { Target = entity };
                            requestToCreateRecords.Requests.Add(createRequest);
                        }
                    }
                }
            }

            try
            {
                var responseForCreateRecords = (ExecuteTransactionResponse)Service.Execute(requestToCreateRecords);
                return new BaseResult(true);
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                return new BaseResult(false, $"Create request failed for the survey reponse {((ExecuteTransactionFault)(ex.Detail)).FaultedRequestIndex + 1} and the reason being: {ex.Detail.Message}");
            }
        }

        private BaseResult CompleteSurvey(Guid surveyRequestId)
        {
            try
            {
                var entity = new Entity("hz_surveyrequest", surveyRequestId);
                entity["hz_completed"] = true;
                entity["hz_completiondate"] = DateTime.Now;
                Service.Update(entity);

                return new BaseResult(true);
            }
            catch (Exception ex)
            {
                return new BaseResult(false, $"Your survey could not be completed. The reason is {ex.Message}");
            }
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

    }
}
