using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Services.Data.Services
{
	public class NotificationHub : Hub
	{
		//private readonly static ConnectionMapping<string> _connections =
		//   new ConnectionMapping<string>();

		//public Task SendNotificationAsync(string message, string userId)
		//{
		//	string name = Context.User.Identity.Name;

		//	foreach (var connectionId in _connections.GetConnections(name))
		//	{
		//		Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
		//	}

		//	return Task.CompletedTask;
		//}

		//public override Task OnConnectedAsync()
		//{
		//	string name = Context.User.Identity.Name;

		//	_connections.Add(name, Context.ConnectionId);

		//	return base.OnConnectedAsync();
		//}

		//public override Task OnDisconnected(bool stopCalled)
		//{
		//	string name = Context.User.Identity.Name;

		//	_connections.Remove(name, Context.ConnectionId);

		//	return base.OnDisconnected(stopCalled);
		//}

		//public override Task OnReconnected()
		//{
		//	string name = Context.User.Identity.Name;

		//	if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
		//	{
		//		_connections.Add(name, Context.ConnectionId);
		//	}

		//	return base.OnReconnected();
		//}
	}
}
