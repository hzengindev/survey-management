using Core.Utilities.Constants;
using Core.Utilities.Dynamics;
using System;
using System.Collections.Generic;

namespace DTOs.Survey
{
    public class SurveyQuestionItemModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public int Order { get; set; }
        public bool Required { get; set; }
        public bool AdditionalAnswer { get; set; }
        public QuestionType QuestionType { get; set; }
        public Lookup SurveyQuestionGroup { get; set; }

        public List<SurveyAnswerOptionItemModel> AnswerOptions { get; set; }

        public SurveyQuestionItemModel(Guid id, string name, string description, string imageURL, int order, bool required, bool additionalAnswer, QuestionType questionType, Lookup surveyQuestionGroup)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.ImageURL = imageURL;
            this.Order = order;
            this.Required = required;
            this.AdditionalAnswer = additionalAnswer;
            this.QuestionType = questionType;
            this.SurveyQuestionGroup = surveyQuestionGroup;

            this.AnswerOptions = new List<SurveyAnswerOptionItemModel>();
        }
    }
}
