using System;
using SignalR;
using System.Threading.Tasks;

namespace WilliamsonFamily.Web.Connection
{
	public class PhoneConnection : PersistentConnection
	{
		protected override Task OnReceivedAsync(string connectionId, string data)
		{
			return Connection.Broadcast(data);
		}

		protected override Task OnConnectedAsync(SignalR.Hosting.IRequest request, System.Collections.Generic.IEnumerable<string> groups, string connectionId)
		{
			return Connection.Broadcast("Client [" + connectionId + "] connected");
		}

		protected override Task OnDisconnectAsync(string connectionId)
		{
			return Connection.Broadcast("Client [" + connectionId + "] disconnected");
		}
	}
}