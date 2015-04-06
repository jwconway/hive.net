using System.Collections.Generic;
using Akka.Actor;
using Akka.Routing;
using Hoist.Host.Messages;

namespace Hoist.Host.Actors
{
	/// <summary>
	/// Tracks routes to the plugin actors in the remote plugin actor systems and routes messages to them all
	/// </summary>
	public class PluginObserver : BaseUntypedActor, IHandle<AddRouteToBroadCastListMessage>
	{
		public PluginObserver()
		{
			RouteNames = new List<string>();
		}

		public List<string> RouteNames { get; private set; }

		public void Handle(AddRouteToBroadCastListMessage message)
		{
			RouteNames.Add(message.Route);
		}


		protected override void OnReceive(object message)
		{
			if (message is AddRouteToBroadCastListMessage)
			{
				Handle((AddRouteToBroadCastListMessage)message);
				return;
			}
			var broadcaster = Context.ActorOf(Props.Empty.WithRouter(new BroadcastGroup(RouteNames)));
			broadcaster.Tell(new Broadcast(message));
		}
	}
}
