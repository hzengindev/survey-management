using Core.Utilities.Constants;
using System;

namespace DTOs.Survey
{
    public class AnswerItemModel
    {
        public Guid QuestionId { get; set; }
        public QuestionType QuestionType { get; set; }
        public string AdditionalAnswer { get; set; }
        public string TextAnswer { get; set; }
        public Guid? SelectAnswer { get; set; }
        public Guid[] MultiSelectAnswer { get; set; }
    }
}