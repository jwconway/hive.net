using System;
using Hive.Plugin;
using Hive.Plugin.Plugin;
using Microsoft.Practices.Unity;

namespace Hive.Test.Plugins
{
	[Serializable]
	public class ConsoleWriterPlugin : PluginBase
	{
		public override void RegisterPluginDependencies(UnityContainer builder)
		{
			//throw new NotImplementedException();
		}

		public override IPluginAppStart GetPluginAppStart()
		{
			return new ThisPluginAppStart();
		}
	}

	public class ThisPluginAppStart : IPluginAppStart
	{
		public void StartPluginApp()
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("Starting");
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
