using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jaywayco.Dust.Actors.Messages
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
