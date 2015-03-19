using System;
using Akka.Actor;
using Hoist.Host.Messages;

namespace Hoist.Host.Actors
{
	/// <summary>
	/// Gets the paths to all of the dlls in the filesystem
	/// </summary>
	public class PluginStoreActor : BaseTypedActor, IHandle<GetAllPluginsRequest>
	{
		public async void Handle(GetAllPluginsRequest message)
		{
			try
			{
				var fileSystemActorRef = Context.ActorOf(Props.Create<FileSystemActor>(), "FileSystem");
				var pluginBootstrapperRef = Context.ActorOf(Props.Create<PluginBootstrapperActor>(), "PluginBootstrapper");

				var dllFiles = await fileSystemActorRef.Ask<FilesInPathResponse>(new FilesInPathRequest(message.FolderPath, "*.dll"));
				int assemblyNumber = 1;
				foreach (var dll in dllFiles.Files)
				{
					pluginBootstrapperRef.Tell(new BootstrapPluginsFromAssemblyRequest(dll, assemblyNumber));
					assemblyNumber++;
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