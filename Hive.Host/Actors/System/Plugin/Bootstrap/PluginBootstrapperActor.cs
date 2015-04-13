using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using Akka.Actor;
using Hive.Host.Actors.BaseClasses;
using Hive.Host.Actors.System.Plugin.Bootstrap.Process;
using Hive.Host.Messages;
using Hive.Plugin;

namespace Hive.Host.Actors.System.Plugin.Bootstrap
{
	/// <summary>
	/// Given an assembly reference will load a new appdomain and initialize its actor system
	/// </summary>
	public class PluginBootstrapperActor : BaseTypedActor, IHandle<BootstrapPluginsFromAssemblyMessage>
	{
		private readonly IActorRef pluginProcessActorRef;

		public PluginBootstrapperActor()
		{
			pluginProcessActorRef = Context.ActorOf(Props.Create<PluginProcessActor>());	
		}

		public async void Handle(BootstrapPluginsFromAssemblyMessage message)
		{
			try
			{
				//get assembly ref
				var assembly = Assembly.LoadFrom(message.AssemblyPath);

				//get implementations of IPlugin
				var pluginImplementations = assembly
									.GetTypes()
									.Where(type => type.GetInterfaces().Any(interfaceType => interfaceType.FullName == typeof(IPlugin).FullName))
									.Where(type => type.IsClass && !type.IsAbstract);

				if (pluginImplementations.Count() > 1)
				{
					throw new NotSupportedException("Only one plugin allowed per assembly");
				}
				//if we have some in this assembly create an app domain, grab a new port number 
				//and initialize the actor system in the new appdomain
				if (pluginImplementations.Any())
				{
					//create app domain
					var appDomain = AppDomain.CreateDomain(message.AssemblyPath, new Evidence(), Path.GetDirectoryName(message.AssemblyPath), AppDomain.CurrentDomain.BaseDirectory, false);
					//grab a new port number
					//pass on the message to initialize the actor system on the above app domain
					pluginProcessActorRef.Tell(new InitializePluginProcessMessage(appDomain, assembly.FullName), Self);

				}
			}
			catch (ReflectionTypeLoadException rtlex)
			{
				//just log this and crack on
				Logger.Warning("ReflectionTypeLoadException happened!!\r\n{0}", rtlex);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Exception happened!!");
				Sender.Tell(new Failure() { Exception = ex, Timestamp = DateTime.UtcNow }, Self);
			}
		}
	}
}
