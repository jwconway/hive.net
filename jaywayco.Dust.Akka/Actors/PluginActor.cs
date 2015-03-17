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
		}
	}

	public class InitializePluginMessage
	{
		public InitializePluginMessage(AppDomain pluginAppDomain)
		{
			PluginAppDomain = pluginAppDomain;
		}

		public AppDomain PluginAppDomain { get; private set; }
	}
}
