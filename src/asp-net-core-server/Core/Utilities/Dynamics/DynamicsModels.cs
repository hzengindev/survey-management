using System;

namespace Core.Utilities.Dynamics
{
    public class Lookup
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class OptionSet
    {
        public int Value { get; set; }
        public string Label { get; set; }
    }
}
