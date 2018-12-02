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
        public int LastValueUser { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime TimeStamp { get; set; }

		[Range(0, int.MaxValue)]
		public int Latitude { get; set; }

		[Range(0, int.MaxValue)]
		public int Longitude { get; set; }

		public bool IsPublic { get; set; }

        public bool AlarmTriggered { get; set; }

		[Range(0, int.MaxValue)]
		public double AlarmMin { get; set; }

		[Range(0, int.MaxValue)]
		public double AlarmMax { get; set; }

		[Range(0, int.MaxValue)]
		public int PollingInterval { get; set; }

		public Guid UserId { get; set; }

        public ApplicationUser User { get; set; }

		public Guid SensorId { get; set; }

        public Sensor Sensor { get; set; }
    }
}
