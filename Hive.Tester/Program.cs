using System;
using System.Threading;
using Akka.Actor;
using Akka.Configuration;
using Hive.Host;
using Hive.Host.Actors;
using Hive.Host.Actors.System.Cluster;
using Hive.Host.Actors.System.Plugin.Store;
using Hive.Host.Messages;
using Hive.Plugin.Messages.Control;

namespace Hive.Tester
{
	class Program
	{
		static void Main(string[] args)
		{
//			var config = ConfigurationFactory.ParseString(@"
//			akka {
//				actor {
//				  provider = ""Akka.Cluster.ClusterActorRefProvider, Akka.Cluster""
//				}
//				remote {
//					helios.tcp {
//						transport-class = ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
//						applied-adapters = []
//						transport-protocol = tcp
//						port = 50003
//						hostname = localhost
//					}
//				}
//				 cluster {
//					seed-nodes = [""akka.tcp://Hive@localhost:50003""]
//					    roles = [host]
//					    auto-down-unreachable-after = 10s
//				  }
//				}
//			}
//			");

//			using (var system = ActorSystem.Create("Hive", config))
//			{
//				//var hostActorRef = system.ActorOf(Props.Create<PluginManagerActor>(), "Node");

//				var pluginStoreActor = system.ActorOf(Props.Create<PluginStoreActor>());
//				var clusterRegistryRef = system.ActorOf(Props.Create<ClusterBusNodeActor>(), "bus");
//				system.ActorOf(Props.Create(() => new ClusterMonitorActor(clusterRegistryRef)));

//				system.Scheduler.ScheduleTellRepeatedly(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), pluginStoreActor, new CheckForNewPluginsMessage(@"C:\GitRepo\Hive\Hive.Test.Plugins\bin\Debug"), pluginStoreActor);
				
//				Console.WriteLine("Waiting for more plugins");
//				while (true)
//				{
//					//Console.WriteLine("Tick...");
//					Console.Write("Enter a command:");
//					var command = Console.ReadLine();
//					if (command == "ls")
//					{
//						var plugins = clusterRegistryRef.Ask<ListPluginsResponse>(new ListPluginsRequest()).Result;
//						foreach (var plugin in plugins.Members)
//						{
//							Console.WriteLine("{0}", plugin.Path.Address);
//						}
//					}
//					if (command.StartsWith("send:"))
//					{
//						var message = command.Split(new char[] { ':' })[1];
//						clusterRegistryRef.Tell(new HiveControlMessage(message));
//					}
//					Thread.Sleep(1000);
//				}
//			}

			using (var hive = Hive.Host.Hive.Join().WithPluginStore<PluginStoreActor>().StartWatchingForPlugins(new CheckForNewPluginsMessage(@"C:\GitRepo\Hive\Hive.Tester\bin\Debug\Plugins")))
			{
				Console.WriteLine("Waiting for more plugins");
				while (true)
				{
					//Console.WriteLine("Tick...");
					Console.Write("Enter a command:");
					var command = Console.ReadLine();
					if (command == "ls")
					{
						var plugins = hive.GetMembers();
						foreach (var plugin in plugins)
						{
							Console.WriteLine("{0}", plugin.Address);
						}
					}
					if (command.StartsWith("send:"))
					{
						var message = command.Split(new char[] { ':' })[1];
						hive.NotifyMembers(new HiveControlMessage(message));
					}

					if (command.StartsWith("plugin:"))
					{
						var parts = command.Split(new[] { ':', ';' });
						var pluginId = parts[1];
						var pluginOperation = parts[3];
						if (pluginOperation.ToLower() == "start")
						{
							hive.NotifyMembers(new PluginAction<StartRequest>());
						}
						if (pluginOperation.ToLower() == "stop")
						{
							hive.NotifyMembers(new PluginAction<StopRequest>());
						}
						if (pluginOperation.ToLower() == "pause")
						{
							hive.NotifyMembers(new PluginAction<PauseRequest>());
						}
					}
					Thread.Sleep(1000);
				}
			}

			Console.ReadLine();
		}
	}
}
