using System;

namespace DTOs.Survey
{
    public class SurveyQuestionGroup
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
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
