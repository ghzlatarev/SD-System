using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SD.Web.Areas.Administration.Models.SensorViewModels
{
	public class RegisterViewModel
	{
		public RegisterViewModel(){}

		public RegisterViewModel(string userId, IList<Tuple<string, string>> sensorNamesIds)
		{
			this.UserId = userId;
			this.SensorNamesIds = sensorNamesIds;
		}

		[Required]
		[Display(Name = "User ID")]
		public string UserId { get; set; }

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
		public int Latitude { get; set; }

		[Required]
		[Display(Name = "Longitude")]
		public int Longitude { get; set; }

		[Required]
		[Display(Name = "IsPublic")]
		public bool IsPublic { get; set; }

		[Required]
		[Display(Name = "AlarmTriggered")]
		public bool AlarmTriggered { get; set; }

		[Required]
		[Display(Name = "AlarmMin")]
		public int AlarmMin { get; set; }

		[Required]
		[Display(Name = "AlarmMax")]
		public int AlarmMax { get; set; }

		[Required]
		[Display(Name = "Sensor ID")]
		public string SensorId { get; set; }

		public IList<Tuple<string, string>> SensorNamesIds { get; set; }
	}
}
