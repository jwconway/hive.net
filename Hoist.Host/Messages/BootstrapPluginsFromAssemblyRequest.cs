namespace Hoist.Host.Messages
{
	public class BootstrapPluginsFromAssemblyRequest
	{
		public BootstrapPluginsFromAssemblyRequest(string assemblyPath, int assemblyNumber)
		{
			AssemblyNumber = assemblyNumber;
			AssemblyPath = assemblyPath;
		}

		public string AssemblyPath { get; private set; }
		public int AssemblyNumber { get; private set; }
	}
}