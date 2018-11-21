using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using SD.Data.Models.Abstract;

namespace SD.Data.Models
{
    public class Sensor : BaseEntity
    {
        [Required]
        [JsonProperty("sensorId")]
        public Guid SensorId { get; set; }

        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("minPollingIntervalInSeconds")]
        public int MinPollingIntervalInSeconds { get; set; }

        [JsonProperty("measureType")]
        public string MeasureType { get; set; }

        [JsonIgnore]
        public ICollection<SensorData> SensorData { get; set; }
    }
}
