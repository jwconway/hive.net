namespace Hoist.Host.Messages
{
	public class BootstrapPluginsFromAssemblyMessage
	{
		public BootstrapPluginsFromAssemblyMessage(string assemblyPath, int assemblyNumber)
		{
			AssemblyNumber = assemblyNumber;
			AssemblyPath = assemblyPath;
		}

		public string AssemblyPath { get; private set; }
		public int AssemblyNumber { get; private set; }
	}
}