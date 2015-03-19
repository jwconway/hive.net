namespace Hoist.Host.Messages
{
	public class FilesInPathRequest
	{
		public FilesInPathRequest(string directoryPath, string filter)
		{
			Filter = filter;
			DirectoryPath = directoryPath;
		}

		public FilesInPathRequest(string directoryPath)
			: this(directoryPath, "*.*")
		{
		}

		public string DirectoryPath { get; private set; }
		public string Filter { get; private set; }
	}
}