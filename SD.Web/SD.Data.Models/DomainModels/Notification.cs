using SD.Data.Models.Abstract;
using SD.Data.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SD.Data.Models.DomainModels
{
	public class Notification : BaseEntity
	{
		public string Message { get; set; }

		public bool IsRead { get; set; }

		public string UserId { get; set; }

		public ApplicationUser User { get; set; }
	}
}
