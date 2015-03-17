using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Akka.Actor;
using jaywayco.Dust.Actors.Messages;

namespace jaywayco.Dust.Actors.Actors
{
	public class PluginStoreActor : BaseTypedActor, IHandle<GetAllPluginsRequest>
	{
		public async void Handle(GetAllPluginsRequest message)
		{
			try
			{
				var fileSystemActorRef = Context.ActorOf(Props.Create<FileSystemActor>(), "FileSystem");
				var pluginBootstrapperRef = Context.ActorOf(Props.Create<PluginBootstrapperActor>(), "PluginBootstrapper");

				var dllFiles = await fileSystemActorRef.Ask<FilesInPathResponse>(new FilesInPathRequest(message.FolderPath, "*.dll"));

				foreach (var dll in dllFiles.Files)
				{
					pluginBootstrapperRef.Tell(new BootstrapPluginsFromAssemblyRequest(dll));
				}
			}
			catch(Exception ex)
			{
				Logger.Error(ex, "Exception happened!!");
				Sender.Tell(new Failure() { Exception = ex, Timestamp = DateTime.UtcNow }, Self);
			}
		}
	}
}