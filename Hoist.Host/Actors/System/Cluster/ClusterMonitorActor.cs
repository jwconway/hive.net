using System;
using Akka.Actor;
using Akka.Cluster;
using Hoist.Host.Messages;

namespace Hoist.Host.Actors.System.Cluster
{
	public class ClusterMonitorActor : TypedActor, IHandle<ClusterEvent.MemberUp>
	{
		private readonly ICanTell clusterRegistry;

		public ClusterMonitorActor(ICanTell clusterRegistry)
		{
			this.clusterRegistry = clusterRegistry;

			Akka.Cluster.Cluster cluster = Akka.Cluster.Cluster.Get(Context.System);
			cluster.Subscribe(Context.Self, new Type[]{ typeof(ClusterEvent.MemberUp) });
		}

		public void Handle(ClusterEvent.MemberUp message)
		{
			clusterRegistry.Tell(new RegisterMessage(message.Member), Self);
		}
	}
}