using SD.Data.Models.DomainModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SD.Web.Areas.Administration.Models.SensorViewModels
{
	public class RegisterViewModel
	{
		public RegisterViewModel(){}

		public RegisterViewModel(string userId, IEnumerable<Sensor> stateSensors, IEnumerable<Sensor> nonStateSensors)
		{
			this.UserId = userId;
			this.StateSensors = stateSensors;
			this.NonStateSensors = nonStateSensors;
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

		[Required]
		[Display(Name = "Sensor ID")]
		public string SensorId { get; set; }

        [Display(Name = "Value")]
        public string LastValueUser { get; set; }

        [Display(Name = "Type")]
        public string Type { get; set; }

		public bool IsState { get; set; }

		public IEnumerable<Sensor> StateSensors { get; set; }

		public IEnumerable<Sensor> NonStateSensors { get; set; }
	}
}
