using System;
using System.Threading;
using Akka.Actor;
using Akka.Configuration;
using Hive.Host.Actors;
using Hive.Host.Actors.System.Cluster;
using Hive.Host.Actors.System.Plugin.Store;
using Hive.Host.Messages;

namespace Hive.Tester
{
	class Program
	{
		static void Main(string[] args)
		{
			var config = ConfigurationFactory.ParseString(@"
			akka {
				actor {
				  provider = ""Akka.Cluster.ClusterActorRefProvider, Akka.Cluster""
				}
				remote {
					helios.tcp {
						transport-class = ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
						applied-adapters = []
						transport-protocol = tcp
						port = 50003
						hostname = localhost
					}
				}
				 cluster {
					seed-nodes = [""akka.tcp://Hive@localhost:50003""]
					    roles = [host]
					    auto-down-unreachable-after = 10s
				  }
				}
			}
			");

			using (var system = ActorSystem.Create("Hive", config))
			{
				//var hostActorRef = system.ActorOf(Props.Create<PluginManagerActor>(), "Node");

				var pluginStoreActor = system.ActorOf(Props.Create<PluginStoreActor>());
				var clusterRegistryRef = system.ActorOf(Props.Create<ClusterBusNodeActor>(), "bus");
				system.ActorOf(Props.Create(() => new ClusterMonitorActor(clusterRegistryRef)));

				system.Scheduler.ScheduleTellRepeatedly(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), pluginStoreActor, new CheckForNewPluginsMessage(@"C:\GitRepo\Hive\Hive.Test.Plugins\bin\Debug"), pluginStoreActor);
				
				Console.WriteLine("Waiting for more plugins");
				while (true)
				{
					//Console.WriteLine("Tick...");
					Console.Write("Enter a command:");
					var command = Console.ReadLine();
					if (command == "ls")
					{
						var plugins = clusterRegistryRef.Ask<ListPluginsResponse>(new ListPluginsRequest()).Result;
						foreach (var plugin in plugins.Members)
						{
							Console.WriteLine("{0}", plugin.Path.Address);
						}
					}
					if (command.StartsWith("send:"))
					{
						var message = command.Split(new char[] { ':' })[1];
						clusterRegistryRef.Tell(new HiveControlMessage(message));
					}
					Thread.Sleep(1000);
				}
			}
			Console.ReadLine();
		}
	}
}
