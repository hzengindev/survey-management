namespace DTOs.Survey
{
    public class GetSurveyHandlerInput : IInput
    {
        public string Code { get; set; }

        public GetSurveyHandlerInput(string code)
        {
            this.Code = code;
        }
    }
}
