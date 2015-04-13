using System.Threading;
using Akka.Actor;
using Akka.Configuration;
using Hive.Plugin.Actors.Plugin;

namespace Hive.Plugin.Actors
{
	/// <summary>
	/// Sets up the actor system in the plugin appdomain
	/// </summary>
	public class PluginActorSystemSetup
	{
		public void Initialize()
		{
			var th = new Thread(DoInitialize);
			th.Start();
		}

		private void DoInitialize()
		{
			PluginHelper.ForceLoadAssemblies();
			
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
						port = 0
						hostname = localhost
					}
				}

				cluster {
					seed-nodes = [""akka.tcp://Hive@localhost:50003""]
					roles = [plugin]
					auto-down-unreachable-after = 10s
				}
				}
			} 
			");
			using (var system = ActorSystem.Create("Hive", config))
			{
				var dependencyResolver = PluginHelper.InitializePlugin(system);
				var actor = system.ActorOf(dependencyResolver.Create<PluginControllerActor>(), "controller");
				while (true)
				{
					Thread.Sleep(1000);
				}
			}
		}
	}

	public class RunMessage
	{
	}
}
