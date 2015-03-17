using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jaywayco.Dust.Actors.Messages
{
	public class CheckForNewPluginsRequest
	{
	}

	public class GetAllPluginsRequest
	{
		public GetAllPluginsRequest(string folderPath)
		{
			FolderPath = folderPath;
		}

		public string FolderPath { get; private set; }
	}
}
