namespace jaywayco.Dust.Actors.Actors
{
	public class BootstrapPluginMessage
	{
		public BootstrapPluginMessage(string fullyQualifiedClassName, string assemblyPath)
		{
			AssemblyPath = assemblyPath;
			FullyQualifiedClassName = fullyQualifiedClassName;
		}

		public string FullyQualifiedClassName { get; private set; }
		public string AssemblyPath { get; private set; }
	}
}