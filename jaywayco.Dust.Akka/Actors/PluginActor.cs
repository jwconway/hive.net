using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace jaywayco.Dust.Actors.Actors
{
	public class PluginActor : BaseTypedActor, IHandle<InitializePluginMessage>
	{
		public void Handle(InitializePluginMessage message)
		{
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Starting plugin {0}", message.PluginAppDomain.FriendlyName);
			Console.ResetColor();

			var pluginInstance = message.PluginAppDomain.CreateInstanceAndUnwrap(message.PluginAssemblyName, message.PluginTypeName) as IPlugin;
			pluginInstance.Start();
		}
	}

	public class InitializePluginMessage
	{
		public InitializePluginMessage(AppDomain pluginAppDomain, string pluginAssemblyName, string pluginTypeName)
		{
			PluginTypeName = pluginTypeName;
			PluginAssemblyName = pluginAssemblyName;
			PluginAppDomain = pluginAppDomain;
		}

		public AppDomain PluginAppDomain { get; private set; }
		public string PluginAssemblyName { get; private set; }
		public string PluginTypeName { get; private set; }
	}
}
