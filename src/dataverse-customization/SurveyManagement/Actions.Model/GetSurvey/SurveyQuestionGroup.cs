using System;
using System.Runtime.Serialization;

namespace Actions.Model.GetSurvey
{
    [DataContract]
    public class SurveyQuestionGroup
    {
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "order")]
        public int Order { get; set; }

        [DataMember(Name = "showDescription")]
        public bool ShowDescription { get; set; }

        public SurveyQuestionGroup(Guid id, string name, string description, int order, bool showDescription)
        {
            Id = id;
            Name = name;
            Description = description;
            Order = order;
            ShowDescription = showDescription;
        }
    }
}
