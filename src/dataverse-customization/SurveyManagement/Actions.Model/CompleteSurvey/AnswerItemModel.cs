using Core.Utilities.Constants;
using System;
using System.Runtime.Serialization;

namespace Actions.Model.CompleteSurvey
{
    [DataContract]
    public class AnswerItemModel
    {
        [DataMember(Name = "questionId")]
        public Guid QuestionId { get; set; }

        [DataMember(Name = "questionType")]
        public QuestionType QuestionType { get; set; }

        [DataMember(Name = "additionalAnswer")]
        public string AdditionalAnswer { get; set; }

        [DataMember(Name = "textAnswer")]
        public string TextAnswer { get; set; }

        [DataMember(Name = "selectAnswer")]
        public Guid? SelectAnswer { get; set; }

        [DataMember(Name = "multiSelectAnswer")]
        public Guid[] MultiSelectAnswer { get; set; }
    }
}
