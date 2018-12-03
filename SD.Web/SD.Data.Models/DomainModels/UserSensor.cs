using SD.Data.Models.Abstract;
using SD.Data.Models.Contracts;
using SD.Data.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SD.Data.Models.DomainModels
{
    public class UserSensor : BaseEntity
    {
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

		[MaxLength(9)]
		public string Latitude { get; set; }

        [MaxLength(9)]
        public string Longitude { get; set; }
        

        public bool IsPublic { get; set; }

        public bool AlarmTriggered { get; set; }

		[Range(0, int.MaxValue)]
		public double AlarmMin { get; set; }

		[Range(0, int.MaxValue)]
		public double AlarmMax { get; set; }

		[Range(0, int.MaxValue)]
		public int PollingInterval { get; set; }

        public string UserId { get; set; } 

        public ApplicationUser User { get; set; }

		public string SensorId { get; set; }

        public Sensor Sensor { get; set; }
    }
}
