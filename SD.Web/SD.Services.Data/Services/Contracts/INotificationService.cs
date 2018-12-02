using System.Threading.Tasks;

namespace SD.Services.Data.Services.Contracts
{
	public interface INotificationService
	{
		Task SendNotificationAsync(string message, string userId);
	}
}
