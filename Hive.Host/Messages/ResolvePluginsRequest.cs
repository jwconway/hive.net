namespace Hive.Host.Messages
{
	public class CheckForNewPluginsMessage
	{
		public CheckForNewPluginsMessage(string folderPath)
		{
			FolderPath = folderPath;
		}

		public string FolderPath { get; private set; }
	}
}
