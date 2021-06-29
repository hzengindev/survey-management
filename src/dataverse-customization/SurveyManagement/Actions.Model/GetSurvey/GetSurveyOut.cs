using Actions.Model.Base;
using Core.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Actions.Model.GetSurvey
{
    [DataContract]
    public class GetSurveyOut : IOut
    {
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [DataMember(Name ="name")]
        public string Name { get; set; }

        [DataMember(Name= "description")]
        public string Description { get; set; }

        [DataMember(Name = "imageURL")]
        public string ImageURL { get; set; }

        [DataMember(Name= "paginationType")]
        public PaginationType PaginationType { get; set; }

        [DataMember(Name= "recordPerPage")]
        public int RecordPerPage { get; set; }

        [DataMember(Name = "surveyQuestions")]
        public List<SurveyQuestionItemModel> SurveyQuestions { get; set; }

        public GetSurveyOut(Guid id, string name, string description, string imageURL, PaginationType paginationType, int recordPerPage)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.ImageURL = imageURL;
            this.PaginationType = paginationType;
            this.RecordPerPage = recordPerPage;

            this.SurveyQuestions = new List<SurveyQuestionItemModel>();
        }
    }
}
