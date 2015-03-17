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
				var assemblyActorRef = Context.ActorOf(Props.Create<AssemblyActor>());
				var appDomainActorRef = Context.ActorOf(Props.Create<AppDomainActor>());
				var pluginActorRef = Context.ActorOf(Props.Create<PluginActor>());

				var pluginImplementationsTask = assemblyActorRef.Ask<ImplementationsOfTypeResponse>(new ImplementationsOfTypeRequest(typeof(IPlugin), Assembly.LoadFrom(message.AssemblyPath)));

				await pluginImplementationsTask.ContinueWith(task =>
				{
					foreach (var plugin in task.Result.Implementations)
					{
						var appDomainResponseTask = appDomainActorRef.Ask<CreateAppDomainResponse>(new CreateAppDomainRequest(plugin.FullName, Path.GetDirectoryName(message.AssemblyPath)));

						appDomainResponseTask.ContinueWith(task2 => pluginActorRef.Tell(new InitializePluginMessage(task2.Result.AppDomain), Self));
					}
				});
				
				foreach (var plugin in pluginImplementationsTask.Implementations)
				{
					var appDomainResponse = appDomainActorRef.Ask<CreateAppDomainResponse>(new CreateAppDomainRequest(plugin.FullName, Path.GetDirectoryName(message.AssemblyPath)));
					pluginActorRef.Tell(new InitializePluginMessage(appDomainResponse.Result.AppDomain), Self);
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
