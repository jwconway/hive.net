using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;
using Hive.Host.Messages;
using Hive.Plugin.Messages.Control;

namespace Hive.Host.Actors.System.Cluster
{
	public class ClusterBusNodeActor : UntypedActor
	{
		private readonly List<IActorRef> members = new List<IActorRef>();
		private IActorRef router;

		public ClusterBusNodeActor()
		{
			router = Context.ActorOf(Props.Empty.WithRouter(new BroadcastGroup(members)), "broadcaster");
		}

		protected override void OnReceive(object message)
		{
			if (message is RegisterMessage)
			{
				var registerMessage = (RegisterMessage)message;
				if (registerMessage.Member.Roles.Contains("plugin"))
				{
					var controllerRef = Context.ActorSelection(registerMessage.Member.Address.ToString() + "/user/controller").ResolveOne(TimeSpan.FromMilliseconds(10000)).Result;
					router.Tell(new AddRoutee(Routee.FromActorRef(controllerRef)), Self);
					members.Add(controllerRef);
				}
			}
			if (message is ListPluginsRequest)
			{
				Sender.Tell(new ListPluginsResponse(members), Self);
			}
			else if(message.GetType().IsGenericType && message.GetType().GetGenericTypeDefinition() == typeof(PluginAction<>))
			{
				router.Tell(message, Self);
			}
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