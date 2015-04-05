using System.IO;
using Akka.Actor;
using Hoist.Host.Actors.BaseClasses;
using Hoist.Host.Messages;

namespace Hoist.Host.Actors.System.Plugin.Store.FileSystem
{
	/// <summary>
	/// File system tasks
	/// </summary>
	public class FileSystemActor : BaseTypedActor, IHandle<FilesInPathRequest>
	{
		public void Handle(FilesInPathRequest message)
		{
			Sender.Tell(new FilesInPathResponse(Directory.GetFiles(message.DirectoryPath, message.Filter)), Self);
		}
	}
}
