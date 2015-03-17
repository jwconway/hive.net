namespace jaywayco.Dust.Actors.Messages
{
	public class BootstrapPluginsFromAssemblyRequest
	{
		public BootstrapPluginsFromAssemblyRequest(string assemblyPath)
		{
			AssemblyPath = assemblyPath;
		}

		public string AssemblyPath { get; private set; }
	}
}