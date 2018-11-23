using System;
using Newtonsoft.Json;
using SD.Data.Models.Abstract;

namespace SD.Data.Models.DomainModels
{
    public class SensorData : BaseEntity
    {
        [JsonProperty("timeStamp")]
        public DateTime? TimeStamp { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("valueType")]
        public string ValueType { get; set; }

        [JsonIgnore]
        public Guid SensorId { get; set; }

        [JsonIgnore]
        public Sensor Sensor { get; set; }
    }
}
