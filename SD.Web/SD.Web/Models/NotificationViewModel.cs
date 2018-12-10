using SD.Data.Models.DomainModels;

namespace SD.Web.Models
{
	public class NotificationViewModel
	{
		public NotificationViewModel(Notifications notification)
		{
			this.Id = notification.Id;
			this.Message = notification.Message;
			this.IsRead = notification.IsRead;
		}

		public string Id { get; set; }

		public string Message { get; set; }

		public bool IsRead { get; set; }
	}
}
