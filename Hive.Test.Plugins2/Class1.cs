using System;
using Hive.Plugin;
using Hive.Plugin.Plugin;
using Microsoft.Practices.Unity;

namespace Hive.Test.Plugins2
{
    [Serializable]
    public class ConsoleWriterPlugin : PluginBase
    {
	    public override void RegisterPluginDependencies(UnityContainer builder)
	    {
		    
	    }

	    public override IPluginAppStart GetPluginAppStart()
	    {
		    return new PluginAppStart2();
	    }
    }

	public class PluginAppStart2 : IPluginAppStart
	{
		public void StartPluginApp()
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Plugin2 Starting...");
			Console.ResetColor();
		}

		public void StopPluginApp()
		{
			
		}

		public void PausePluginApp()
		{
			
		}

		public void DeletePluginApp()
		{
			
		}

		public void UnPausePluginApp()
		{
			
		}
	}
}
