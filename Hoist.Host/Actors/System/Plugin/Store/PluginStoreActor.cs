using System;
using System.Collections.Generic;
using Akka.Actor;
using Hoist.Host.Actors.BaseClasses;
using Hoist.Host.Actors.System.Plugin.Bootstrap;
using Hoist.Host.Actors.System.Plugin.Store.FileSystem;
using Hoist.Host.Messages;

namespace Hoist.Host.Actors.System.Plugin.Store
{
	/// <summary>
	/// Gets the paths to all of the dlls in the filesystem
	/// </summary>
	public class PluginStoreActor : BaseTypedActor, IHandle<CheckForNewPluginsMessage>
	{
		private readonly IActorRef fileSystemActorRef;
		private readonly IActorRef pluginBootstrapperRef;
		private readonly List<string> loadedPluginDlls = new List<string>(); 

		public PluginStoreActor()
		{
			fileSystemActorRef = Context.ActorOf(Props.Create<FileSystemActor>(), "FileSystem");
			pluginBootstrapperRef = Context.ActorOf(Props.Create<PluginBootstrapperActor>(), "PluginBootstrapper");
		}

		public async void Handle(CheckForNewPluginsMessage message)
		{
			try
			{
				var dllFiles = await fileSystemActorRef.Ask<FilesInPathResponse>(new FilesInPathRequest(message.FolderPath, "*.dll"));
				int assemblyNumber = 1;
				foreach (var dll in dllFiles.Files)
				{
					if (!loadedPluginDlls.Contains(dll))
					{
						pluginBootstrapperRef.Tell(new BootstrapPluginsFromAssemblyMessage(dll, assemblyNumber));
						assemblyNumber++;
						loadedPluginDlls.Add(dll);
					}
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