using SD.Data.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SD.Web.Areas.UserRegular.Models
{
    public class SensorRegistrationByUserModel
    {
        public SensorAPIViewModel SensorSelected { get; set; }
        [StringLength(35, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(300, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Description { get; set; }

        [StringLength(300, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string UserDescription { get; set; }

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
        public bool TickOff { get; set; }

        public bool AlarmTriggered { get; set; }

        public int AlarmMin { get; set; }

        public int AlarmMax { get; set; }

        public string UserId { get; set; }

        public string SensorId { get; set; }

        public string Id { get; set; }

        public string Tag { get; set; }

        public int ApiInterval { get; set; }

    }
}
