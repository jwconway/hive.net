namespace Hoist.Host.Messages
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
