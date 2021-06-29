using Actions.Model.Base;
using System;
using System.Runtime.Serialization;

namespace Actions.Model.GetSurvey
{
    [DataContract]
    public class GetSurveyIn : IIn
    {
        [DataMember(Name = "surveyRequestId")]
        public Guid SurveyRequestId { get; set; }
    }
}
