using SD.Data.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SD.Web.Areas.UserRegular.Models
{
    public class SensorDataAPIViewModel
    {
        public SensorDataAPIViewModel(SensorData sensorData)
        {
            TimeStamp = sensorData.TimeStamp;
            Value = sensorData.Value;
            ValueType = sensorData.ValueType;
            SensorId = sensorData.SensorId;
        }

        public DateTime? TimeStamp { get; set; }
        [Range(int.MinValue, int.MaxValue)]
        public string Value { get; set; }

        public string ValueType { get; set; }

        public Guid SensorId { get; set; }
    }
}
