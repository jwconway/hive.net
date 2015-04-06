using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;
using Hive.Host.Messages;

namespace Hive.Host.Actors.System.Cluster
{
	public class ClusterBusNodeActor : TypedActor, IHandle<RegisterMessage>, IHandle<ListPluginsRequest>, IHandle<HiveControlMessage>
	{
		private readonly List<IActorRef> members = new List<IActorRef>();
		private IActorRef router;

		public ClusterBusNodeActor()
		{
			router = Context.ActorOf(Props.Empty.WithRouter(new BroadcastGroup(members)), "broadcaster");
		}

		public void Handle(RegisterMessage message)
		{
			if (message.Member.Roles.Contains("plugin"))
			{
				var controllerRef = Context.ActorSelection(message.Member.Address.ToString() + "/user/controller").ResolveOne(TimeSpan.FromMilliseconds(10000)).Result;
				router.Tell(new AddRoutee(Routee.FromActorRef(controllerRef)), Self);
				members.Add(controllerRef);
			}
		}

		public void Handle(ListPluginsRequest message)
		{
			Sender.Tell(new ListPluginsResponse(members), Self);
		}

		public void Handle(HiveControlMessage message)
		{
			
			router.Tell(message, Self);
		}
	}

	

	public class HiveControlMessage
	{
		public string Message { get; private set; }

		public HiveControlMessage(string message)
		{
			Message = message;
		}
	}

	public class ListPluginsRequest
	{
	}

	public class ListPluginsResponse
	{
		public ListPluginsResponse(List<IActorRef> members)
		{
			Members = members;
		}

		public List<IActorRef> Members { get; private set; } 
	}
}