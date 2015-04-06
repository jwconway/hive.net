using System;
using Akka.Actor;
using Hive.Plugin.Messages.Control;

namespace Hive.Plugin.Actors.Plugin
{
	/// <summary>
	/// This actor will handle control messages from the host in the plugins own app domain
	/// </summary>
	public class PluginControllerActor : UntypedActor, IHandle<PluginStatusChangeMessage>
	{
		private readonly IPluginAppStart pluginAppStart;
		private IActorRef PluginActorRef;
		private PluginStatus PluginStatus;

		public PluginControllerActor(IPluginAppStart pluginAppStart)
		{
			this.pluginAppStart = pluginAppStart;
			PluginStatus = PluginStatus.Stopped;
			PluginActorRef = Context.ActorOf(Props.Create(()=> new PluginActor(pluginAppStart)));
		}

		protected override void OnReceive(object message)
		{
			if (message is PluginActions.StatusRequest)
			{
				Sender.Tell(new PluginStatusResponse(PluginStatus));
			}
			else if (message is PluginActions.StartRequest)
			{
				PluginActorRef.Tell(new PluginControlMessages.Start(PluginStatus));
				Sender.Tell(new PluginActions.AcknowledgedResponse());
			}
			else if (message is PluginStatusChangeMessage)
			{
				PluginStatus = ((PluginStatusChangeMessage)message).NewStatus;
			}
			else
			{
				Console.WriteLine("Thanks for the message but i dont understand '" + message + "'!!");
				Sender.Tell(new PluginActions.NotUnderstoodResponse());
			}
		}

		public void Handle(PluginStatusChangeMessage message)
		{
			PluginStatus = message.NewStatus;
		}
	}

	public interface IPluginAppStart
	{
		void StartPluginApp();
	}

	public class PluginAppStart : IPluginAppStart
	{
		public void StartPluginApp()
		{
			
		}
	}

	public class PluginActor : UntypedActor
	{
		private readonly IPluginAppStart pluginAppStart;

		public PluginActor(IPluginAppStart pluginAppStart)
		{
			this.pluginAppStart = pluginAppStart;
		}

		protected override void OnReceive(object message)
		{
			if (message is PluginControlMessages.Start)
			{
				var startMessage = (PluginControlMessages.Start)message;
				try
				{	
					Sender.Tell(new PluginStatusChangeMessage(startMessage.CurrentStatus, PluginStatus.Starting));
					//start plugin
					pluginAppStart.StartPluginApp();
					Sender.Tell(new PluginStatusChangeMessage(startMessage.CurrentStatus, PluginStatus.Running));
				}
				catch(Exception ex)
				{
					Sender.Tell(new PluginStatusChangeMessage(startMessage.CurrentStatus, PluginStatus.Faulted));
				}
			}
		}
	}
}
