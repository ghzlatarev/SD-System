using SD.Data.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SD.Web.Areas.Administration.Models.SensorViewModels
{
	public class ModifyViewModel
	{
		public ModifyViewModel() { }

		public ModifyViewModel(UserSensor userSensor)
		{
			this.Name = userSensor.Name;
			this.Description = userSensor.Description;
			this.PollingInterval = userSensor.PollingInterval;
			this.Latitude = userSensor.Latitude;
			this.Longitude = userSensor.Longitude;
			this.IsPublic = userSensor.IsPublic;
			this.AlarmTriggered = userSensor.AlarmTriggered;
			this.AlarmMin = userSensor.AlarmMin;
			this.AlarmMax = userSensor.AlarmMax;
			this.Id = userSensor.Id;
		}

		[Required]
		[Display(Name = "Id")]
		public string Id { get; set; }

		[Required]
		[StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		[Display(Name = "Name")]
		public string Name { get; set; }

		[Required]
		[StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		[Display(Name = "Description")]
		public string Description { get; set; }

		[Required]
		[Display(Name = "PollingInterval")]
		public int PollingInterval { get; set; }

		[Required]
		[Display(Name = "Latitude")]
		public string Latitude { get; set; }

		[Required]
		[Display(Name = "Longitude")]
		public string Longitude { get; set; }

		[Required]
		[Display(Name = "IsPublic")]
		public bool IsPublic { get; set; }

		[Required]
		[Display(Name = "AlarmTriggered")]
		public bool AlarmTriggered { get; set; }

		[Required]
		[Display(Name = "AlarmMin")]
		public double AlarmMin { get; set; }

		[Required]
		[Display(Name = "AlarmMax")]
		public double AlarmMax { get; set; }

		public IList<Tuple<string, string>> SensorNamesIds { get; set; }
	}
}
