using Microsoft.Xrm.Sdk;
using System;

namespace Business.Abstract.Survey
{
    public interface ISurveyService
    {
        Entity GetSurveyById(Guid id);
        EntityCollection GetSurveyQuestionsBySurveyId(Guid surveyId);
    }
}
