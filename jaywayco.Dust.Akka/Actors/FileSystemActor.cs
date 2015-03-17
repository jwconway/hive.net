using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using jaywayco.Dust.Actors.Messages;

namespace jaywayco.Dust.Actors.Actors
{
	public class FileSystemActor : TypedActor, IHandle<FilesInPathRequest>
	{
		public void Handle(FilesInPathRequest message)
		{
			Sender.Tell(new FilesInPathResponse(Directory.GetFiles(message.DirectoryPath, message.Filter)), Self);
		}
	}
}
