using SD.Data.Models.DomainModels;
using SD.Data.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SD.Web.Areas.UserRegular.Models
{
    public class UserSensorViewModel
    {
        public UserSensorViewModel(UserSensor sensor)
        {
            Name = sensor.Name;
            Description = sensor.Description;
            Type = sensor.Type;
            UserInterval = sensor.UserInterval;
            LastValueUser = sensor.LastValueUser;
            TimeStamp = sensor.TimeStamp;
            Coordinates = sensor.Coordinates;
            IsPublic = sensor.IsPublic;
            AlarmTriggered = sensor.AlarmTriggered;
            AlarmMin = sensor.AlarmMin;
            AlarmMax = sensor.AlarmMax;
            UserId = sensor.UserId;
            User = sensor.User;
            SensorId = sensor.SensorId;
            Sensor = sensor.Sensor;
            Id = sensor.Id;
            
        }

        public string Id { get; set; }

        [StringLength(35, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(300, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Description { get; set; }

        public string Type { get; set; }

        [Range(0, int.MaxValue)]
        public int UserInterval { get; set; }

        [Range(0, int.MaxValue)]
        public string LastValueUser { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime TimeStamp { get; set; }

        [StringLength(19, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 18)]
        public string Coordinates { get; set; }

        public bool IsPublic { get; set; }

        public bool AlarmTriggered { get; set; }

        public double AlarmMin { get; set; }

        public double AlarmMax { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string SensorId { get; set; }

        public Sensor Sensor { get; set; }
    }
}
