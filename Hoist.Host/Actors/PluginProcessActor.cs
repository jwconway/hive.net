using System;
using Akka.Actor;
using Hoist.Host.Messages;
using Hoist.Plugin;

namespace Hoist.Host.Actors
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
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Starting plugin {0}", message.PluginAppDomain.FriendlyName);
			Console.ResetColor();

			//make sure we always get the same one of these
			var pluginBroadcasterRef = Context.ActorSelection("/user/pluginbroadcaster");

			//marshal a new object out of the new app doamin into this one and start the ball rolling
			var pluginProcessMarshaller = message.PluginAppDomain.CreateInstanceAndUnwrap(typeof(PluginProcessMarshaller).Assembly.FullName, typeof(PluginProcessMarshaller).FullName) as PluginProcessMarshaller;
			pluginProcessMarshaller.SetupPluginProcessActorSystem(message.Port);

			//add the route to the new actor system to the broadcaster register
			var actorRoute = "akka.tcp://testclient@localhost:" + message.Port + "/user/actorSetup";
			pluginBroadcasterRef.Tell(new AddRouteToBroadCastListMessage(actorRoute));

			var pluginActorRef = Context.ActorSelection(actorRoute);

			//start the plugin
			pluginActorRef.Tell(new RunMessage());
		}
	}
}
