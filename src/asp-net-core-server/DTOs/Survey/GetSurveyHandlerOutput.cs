using Core.Utilities.Constants;
using System;
using System.Collections.Generic;

namespace DTOs.Survey
{
    public class GetSurveyHandlerOutput
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public PaginationType PaginationType { get; set; }
        public int RecordPerPage { get; set; }
        public List<SurveyQuestionItemModel> SurveyQuestions { get; set; }

        public GetSurveyHandlerOutput(Guid id, string name, string description, string imageURL, PaginationType paginationType, int recordPerPage)
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
