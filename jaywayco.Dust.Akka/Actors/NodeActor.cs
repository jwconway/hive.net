using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;
using jaywayco.Dust.Actors.Messages;

namespace jaywayco.Dust.Actors.Actors
{
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
