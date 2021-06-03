using System.Collections.Generic;

namespace DTOs.Survey
{
    public class CompleteSurveyHandlerInput : IInput
    {
        public string Code { get; set; }
        public List<AnswerItemModel> Answers { get; set; }
    }
}