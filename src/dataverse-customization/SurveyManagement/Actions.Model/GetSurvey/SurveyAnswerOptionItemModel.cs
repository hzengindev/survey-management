using System;
using System.Runtime.Serialization;

namespace Actions.Model.GetSurvey
{
    [DataContract]
    public class SurveyAnswerOptionItemModel
    {
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "order")]
        public int Order { get; set; }

        [DataMember(Name = "imageURL")]
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
