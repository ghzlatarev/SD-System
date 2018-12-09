using SD.Data.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
			this.Latitude = double.Parse(userSensor.Latitude);
			this.Longitude = double.Parse(userSensor.Longitude);
			this.IsPublic = userSensor.IsPublic;
			this.AlarmTriggered = userSensor.AlarmTriggered;
			this.AlarmMin = userSensor.AlarmMin;
			this.AlarmMax = userSensor.AlarmMax;
			this.Id = userSensor.Id;
			this.IsState = userSensor.Sensor.IsState;
		}

		[Required]
		[Display(Name = "Id")]
		public string Id { get; set; }

		[Required]
		[StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		[Display(Name = "Name")]
		public string Name { get; set; }
		
		[StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		[Display(Name = "Description")]
		public string Description { get; set; }

		[Required]
		[Range(1, int.MaxValue)]
		[Display(Name = "PollingInterval")]
		public int PollingInterval { get; set; }

		[Required]
		[Range(-90, 90)]
		[Display(Name = "Latitude")]
		public double Latitude { get; set; }

		[Required]
		[Range(-180, 180)]
		[Display(Name = "Longitude")]
		public double Longitude { get; set; }
		
		[Display(Name = "IsPublic")]
		public bool IsPublic { get; set; }
		
		[Display(Name = "AlarmTriggered")]
		public bool AlarmTriggered { get; set; }

		[Display(Name = "AlarmMin")]
		public double AlarmMin { get; set; }
		
		[Display(Name = "AlarmMax")]
		public double AlarmMax { get; set; }
		
		public bool IsState { get; set; }
	}
}
