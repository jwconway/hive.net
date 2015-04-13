using System.IO;
using Akka.Actor;
using Hive.Host.Actors.BaseClasses;
using Hive.Host.Messages;

namespace Hive.Host.Actors.System.Plugin.Store.FileSystem
{
	/// <summary>
	/// File system tasks
	/// </summary>
	public class FileSystemActor : BaseTypedActor, IHandle<FilesInPathRequest>
	{
		public void Handle(FilesInPathRequest message)
		{
			Sender.Tell(new FilesInPathResponse(Directory.GetFiles(message.DirectoryPath, message.Filter, SearchOption.AllDirectories)), Self);
		}
	}
}
