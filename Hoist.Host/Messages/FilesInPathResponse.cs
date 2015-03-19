using System.Collections.Generic;

namespace Hoist.Host.Messages
{
	public class FilesInPathResponse
	{
		public FilesInPathResponse(IEnumerable<string> files)
		{
			Files = files;
		}

		public IEnumerable<string> Files { get; private set; } 
	}
}
