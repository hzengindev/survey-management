using Core.Utilities.Results;
using DTOs.Survey;

namespace Business.Abstract.Survey
{
    public interface ICompleteSurvey
    {
        IResult Handle(CompleteSurveyHandlerInput input);
    }
}