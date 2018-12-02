using SD.Data.Models.DomainModels;
using System;

namespace SD.Web.Areas.Administration.Models.SensorViewModels
{
	public class SensorTableViewModel
	{
		public SensorTableViewModel(UserSensor userSensor)
		{
			this.Id = userSensor.Id;
			this.Name = userSensor.Name;
			this.Latitude = userSensor.Latitude;
			this.Longitude = userSensor.Longitude;
			this.AlarmMax = userSensor.AlarmMax;
			this.AlarmMin = userSensor.AlarmMin;
			this.IsPublic = userSensor.IsPublic;
		}

		public Guid Id { get; set; }

		public string Name { get; set; }

		public int Latitude { get; set; }

		public int Longitude { get; set; }

		public double AlarmMax { get; set; }

		public double AlarmMin { get; set; }

		public bool IsPublic { get; set; }

		public bool AlarmTriggered { get; set; }
	}
}
