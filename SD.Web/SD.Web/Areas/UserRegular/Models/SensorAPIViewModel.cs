using SD.Data.Models.DomainModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SD.Web.Areas.UserRegular.Models
{
    public class SensorAPIViewModel
    {
        public SensorAPIViewModel(Sensor sensor)
        {
            SensorId = sensor.SensorId;
            Tag = sensor.Tag;
            Description = sensor.Description;
            MinPollingIntervalInSeconds = sensor.MinPollingIntervalInSeconds;
            MeasureType = sensor.MeasureType;
        }

        public string SensorId { get; set; }
        [MaxLength(50)]
        public string Tag { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        [Range(0,int.MaxValue)]
        public int MinPollingIntervalInSeconds { get; set; }
        [Range(0,50)]
        public string MeasureType { get; set; }
        
    }
}
