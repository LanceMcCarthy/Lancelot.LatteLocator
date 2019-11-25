using System.Runtime.Serialization;

namespace LatteLocator.Core.Models
{
    [DataContract]
    public class StarbucksCard
    {
        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public long AccountNumber { get; set; }

        [DataMember]
        public string ScannedData { get; set; }
    }
}