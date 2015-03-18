using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using jaywayco.Dust.Actors.Messages;

namespace jaywayco.Dust.Actors.Actors
{
	public class PluginBootstrapperActor : BaseTypedActor, IHandle<BootstrapPluginsFromAssemblyRequest>
	{
		public async void Handle(BootstrapPluginsFromAssemblyRequest message)
		{
			try
			{
				var pluginActorRef = Context.ActorOf(Props.Create<PluginActor>());

				//var pluginImplementationsTask = assemblyActorRef.Ask<ImplementationsOfTypeResponse>(new ImplementationsOfTypeRequest(typeof(IPlugin), Assembly.LoadFrom(message.AssemblyPath)));
				var assembly = Assembly.LoadFrom(message.AssemblyPath);
				var pluginImplementations = assembly
									.GetTypes()
									.Where(type => type.GetInterfaces().Any(interfaceType => interfaceType.FullName == typeof(IPlugin).FullName));
				
				foreach (var plugin in pluginImplementations)
				{
					var appDomain = AppDomain.CreateDomain(plugin.FullName, new Evidence(), Path.GetDirectoryName(message.AssemblyPath), AppDomain.CurrentDomain.BaseDirectory, false);
					pluginActorRef.Tell(new InitializePluginMessage(appDomain, assembly.FullName, plugin.FullName), Self);
				}
			}
			catch(Exception ex)
			{
				Logger.Error(ex, "Exception happened!!");
				Sender.Tell(new Failure() { Exception = ex, Timestamp = DateTime.UtcNow }, Self);
			}
		}
	}
}
