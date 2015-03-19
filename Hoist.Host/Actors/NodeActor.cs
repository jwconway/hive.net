using System;
using Akka.Actor;
using Hoist.Host.Messages;

namespace Hoist.Host.Actors
{
	/// <summary>
	/// The parent actor for the host really
	/// </summary>
	public class NodeActor : BaseTypedActor, 
		IHandle<GetAllPluginsRequest>
	{
		public void Handle(GetAllPluginsRequest message)
		{
			try
			{
				var pluginStoreRef = Context.ActorOf(Props.Create<PluginStoreActor>(), "PluginStore");
				pluginStoreRef.Tell(new GetAllPluginsRequest(message.FolderPath), Self);
			}
			catch(Exception ex)
			{
				Logger.Error(ex, "Exception in NodeActor");
				Sender.Tell(new Failure() { Exception = ex, Timestamp = DateTime.UtcNow }, Self);
			}
		}
	}
}
