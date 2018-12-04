﻿using SD.Data.Models.DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SD.Services.Data.Services.Contracts
{
	public interface INotificationService
	{
		Task SendNotificationAsync(string message, string userId);

		Task<List<Notification>> GetItemsAsync(string userId);
	}
}