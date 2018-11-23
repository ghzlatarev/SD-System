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

        [StringLength(18, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 17)]
        public string Coordinates { get; set; }

        public bool isPublic { get; set; }

        public bool AlarmTriggered { get; set; }

        public int AlarmMin { get; set; }

        public int AlarmMax { get; set; }

        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; }

		public Guid SensorId { get; set; }

        public Sensor Sensor { get; set; }
    }
}
