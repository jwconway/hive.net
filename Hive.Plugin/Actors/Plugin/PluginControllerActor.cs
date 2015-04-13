using System;
using Akka.Actor;
using Akka.Dispatch.SysMsg;
using Hive.Plugin.Messages.Control;
using Hive.Plugin.Plugin;

namespace Hive.Plugin.Actors.Plugin
{
	/// <summary>
	/// This actor will handle control messages from the host in the plugins own app domain
	/// </summary>
	public class PluginControllerActor : UntypedActor, IHandle<PluginStatusChangeMessage>
	{
		public IPluginIdentity PluginIdentity { get; set; }
		private readonly IPluginAppStart pluginAppStart;
		private IActorRef PluginActorRef;
		private PluginStatus PluginStatus;

		public PluginControllerActor(IPluginIdentity pluginIdentity, IPluginAppStart pluginAppStart)
		{
			PluginIdentity = pluginIdentity;
			this.pluginAppStart = pluginAppStart;
			PluginStatus = PluginStatus.Stopped;
			PluginActorRef = Context.ActorOf(Props.Create(()=> new PluginActor(pluginAppStart)));
		}

		protected override void OnReceive(object message)
		{
			if(!message.IsForMe(PluginIdentity.PluginId))
			{
				Sender.Tell(new PluginResponse<PluginIgnoredResponse>());	
			}

			if (message is PluginAction<StatusRequest>)
			{
				Sender.Tell(new PluginStatusResponse(PluginStatus));
			}
			else if (message is PluginAction<StartRequest>)
			{
				if (PluginStatus != PluginStatus.Stopped)
				{
					Sender.Tell(new PluginResponse<NotSupportedResponse>());
				}
				PluginActorRef.Tell(new PluginControlMessages.Start(PluginStatus));
				Sender.Tell(new PluginResponse<AcknowledgedResponse>());
			}
			else if (message is PluginAction<StopRequest>)
			{
				if (PluginStatus != PluginStatus.Running && PluginStatus != PluginStatus.Paused)
				{
					Sender.Tell(new PluginResponse<NotSupportedResponse>());
				}
				PluginActorRef.Tell(new PluginControlMessages.Stop(PluginStatus));
				Sender.Tell(new PluginResponse<AcknowledgedResponse>());
			}
			else if (message is PluginAction<PauseRequest>)
			{
				if (PluginStatus != PluginStatus.Running)
				{
					Sender.Tell(new PluginResponse<NotSupportedResponse>());
				}
				PluginActorRef.Tell(new PluginControlMessages.Pause(PluginStatus));
				Sender.Tell(new PluginResponse<AcknowledgedResponse>());
			}
			else if (message is PluginAction<UnPauseRequest>)
			{
				if (PluginStatus != PluginStatus.Paused)
				{
					Sender.Tell(new PluginResponse<NotSupportedResponse>());
				}
				PluginActorRef.Tell(new PluginControlMessages.UnPause(PluginStatus));
				Sender.Tell(new PluginResponse<AcknowledgedResponse>());
			}
			else if (message is PluginAction<DeleteRequest>)
			{
				if (PluginStatus != PluginStatus.Stopped)
				{
					Sender.Tell(new PluginResponse<NotSupportedResponse>());
				}
				PluginActorRef.Tell(new PluginControlMessages.Delete(PluginStatus));
				Sender.Tell(new PluginResponse<AcknowledgedResponse>());
			}
			else if (message is PluginStatusChangeMessage)
			{
				PluginStatus = ((PluginStatusChangeMessage)message).NewStatus;
			}
			else
			{
				Console.WriteLine("Thanks for the message but i dont understand '" + message + "'!!");
				Sender.Tell(new PluginResponse<NotUnderstoodResponse>());
			}
		}

		public void Handle(PluginStatusChangeMessage message)
		{
			PluginStatus = message.NewStatus;
		}
	}

	public interface IPluginIdentity
	{
		string PluginId { get; }
	}

	public class PluginIgnoredResponse : PluginActionResponse
	{
	}
}
