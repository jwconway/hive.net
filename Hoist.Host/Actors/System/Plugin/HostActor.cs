using System;
using Akka.Actor;
using Hoist.Host.Messages;

namespace Hoist.Host.Actors
{
	/// <summary>
	/// The parent actor for the host really
	/// </summary>
	public class PluginManagerActor : BaseTypedActor, 
		IHandle<CheckForNewPluginsMessage>
	{
		private readonly IActorRef _pluginStoreRef;

		public PluginManagerActor()
		{
			_pluginStoreRef = Context.ActorOf(Props.Create<PluginStoreActor>(), "PluginStore");
		}

		public void Handle(CheckForNewPluginsMessage message)
		{
			try
			{
				_pluginStoreRef.Tell(new CheckForNewPluginsMessage(message.FolderPath), Self);
			}
			catch(Exception ex)
			{
				Logger.Error(ex, "Exception in NodeActor");
				Sender.Tell(new Failure() { Exception = ex, Timestamp = DateTime.UtcNow }, Self);
			}
		}
	}
}
