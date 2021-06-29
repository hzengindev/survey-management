using System;
using System.Runtime.Serialization;

namespace Core.Utilities.Results
{
    [DataContract]
    public class Lookup
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}