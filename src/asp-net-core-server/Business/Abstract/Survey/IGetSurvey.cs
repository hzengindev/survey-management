using Core.Utilities.Results;
using DTOs.Survey;

namespace Business.Abstract.Survey
{
    public interface IGetSurvey
    {
        IDataResult<GetSurveyHandlerOutput> Handle(GetSurveyHandlerInput input);
    }
}