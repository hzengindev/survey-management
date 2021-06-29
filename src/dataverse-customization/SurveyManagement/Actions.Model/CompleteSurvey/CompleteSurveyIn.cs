using Actions.Model.Base;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Actions.Model.CompleteSurvey
{
    [DataContract]
    public class CompleteSurveyIn : IIn
    {
        [DataMember(Name = "surveyRequestId")]
        public Guid SurveyRequestId { get; set; }

        [DataMember(Name = "answers")]
        public List<AnswerItemModel> Answers { get; set; }
    }
}
