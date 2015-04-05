using System;
using Akka.Actor;
using Hoist.Host.Actors.BaseClasses;
using Hoist.Host.Messages;
using Hoist.Plugin;

namespace Hoist.Host.Actors.System.Plugin.Bootstrap.Process
{
	/// <summary>
	/// Calls the marshaller in the new app domain. The marshaller creates the new actor system remotely
	/// </summary>
	public class PluginProcessActor : BaseTypedActor, IHandle<InitializePluginProcessMessage>
	{
		public PluginProcessActor()
		{
		}
		public void Handle(InitializePluginProcessMessage message)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Starting plugin {0}", message.PluginAppDomain.FriendlyName);
			Console.ResetColor();

			//marshal a new object out of the new app doamin into this one and start the ball rolling
			var pluginProcessMarshaller = message.PluginAppDomain.CreateInstanceAndUnwrap(typeof(PluginProcessMarshaller).Assembly.FullName, typeof(PluginProcessMarshaller).FullName) as PluginProcessMarshaller;
			pluginProcessMarshaller.SetupPluginProcessActorSystem(message.Port);
		}
	}
}
