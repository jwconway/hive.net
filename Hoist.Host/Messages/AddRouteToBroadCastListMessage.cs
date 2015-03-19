namespace Hoist.Host.Messages
{
	public class AddRouteToBroadCastListMessage
	{
		public AddRouteToBroadCastListMessage(string route)
		{
			Route = route;
		}

		public string Route { get; private set; }
	}
}