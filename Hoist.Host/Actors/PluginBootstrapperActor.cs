using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using Akka.Actor;
using Hoist.Host.Messages;
using Hoist.Plugin;

namespace Hoist.Host.Actors
{
	/// <summary>
	/// Given an assembly reference will load a new appdomain and initialize its actor system
	/// </summary>
	public class PluginBootstrapperActor : BaseTypedActor, IHandle<BootstrapPluginsFromAssemblyRequest>
	{
		public async void Handle(BootstrapPluginsFromAssemblyRequest message)
		{
			try
			{
				var pluginProcessActorRef = Context.ActorOf(Props.Create<PluginProcessActor>());

				//get assembly ref
				var assembly = Assembly.LoadFrom(message.AssemblyPath);

				//get implementations of IPlugin
				var pluginImplementations = assembly
									.GetTypes()
									.Where(type => type.GetInterfaces().Any(interfaceType => interfaceType.FullName == typeof(IPlugin).FullName));

				//if we have some in this assembly create an app domain, grab a new port number 
				//and initialize the actor system in the new appdomain
				if (pluginImplementations.Any())
				{
					//create app domain
					var appDomain = AppDomain.CreateDomain(message.AssemblyPath, new Evidence(), Path.GetDirectoryName(message.AssemblyPath), AppDomain.CurrentDomain.BaseDirectory, false);
					//grab a new port number
					var port = 50000 + message.AssemblyNumber;//I Hate this - is there a better way to do this
					//pass on the message to initialize the actor system on the above app domain
					pluginProcessActorRef.Tell(new InitializePluginProcessMessage(appDomain, assembly.FullName, port), Self);
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Exception happened!!");
				Sender.Tell(new Failure() { Exception = ex, Timestamp = DateTime.UtcNow }, Self);
			}
		}
	}
}
