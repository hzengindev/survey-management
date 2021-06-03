using System;

namespace DTOs.Survey
{
    public class SurveyAnswerOptionItemModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public string ImageURL { get; set; }

        public SurveyAnswerOptionItemModel(Guid id, string name, int order, string imageURL)
        {
            this.Id = id;
            this.Name = name;
            this.Order = order;
            this.ImageURL = imageURL;
        }
    }
}
