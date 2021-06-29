using Core.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Actions.Model.GetSurvey
{
    [DataContract]
    public class SurveyQuestionItemModel
    {
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "imageURL")]
        public string ImageURL { get; set; }

        [DataMember(Name = "order")]
        public int Order { get; set; }

        [DataMember(Name = "required")]
        public bool Required { get; set; }

        [DataMember(Name = "additionalAnswer")]
        public bool AdditionalAnswer { get; set; }

        [DataMember(Name = "questionType")]
        public QuestionType QuestionType { get; set; }

        [DataMember(Name = "surveyQuestionGroup")]
        public SurveyQuestionGroup SurveyQuestionGroup { get; set; }

        [DataMember(Name = "answerOptions")]
        public List<SurveyAnswerOptionItemModel> AnswerOptions { get; set; }

        public SurveyQuestionItemModel(Guid id, string name, string description, string imageURL, int order, bool required, bool additionalAnswer, QuestionType questionType, SurveyQuestionGroup surveyQuestionGroup)
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
