using System.Threading;
using Akka.Actor;
using Akka.Configuration;

namespace Hoist.Plugin.Actors.Plugin
{
	/// <summary>
	/// Sets up the actor system in the plugin appdomain
	/// </summary>
	public class PluginActorSystemSetup
	{
		public void Initialize(int port)
		{
			var th = new Thread(DoInitialize);
			th.Start(port);
			Thread.Sleep(2000);
		}

		private void DoInitialize(object state)
		{
			int port = (int)state;
			var bind = new Akka.Cluster.ClusterEvent();// .Remote.AddressUid();//this serves no other purpose than to load the Akk.Remote assembly into this app domain
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
					seed-nodes = [""akka.tcp://hoist@localhost:50003""]
					roles = [plugin]
					auto-down-unreachable-after = 10s
				}
				}
			} 
			");
			using (var system = ActorSystem.Create("hoist", config))
			{
				var actor = system.ActorOf<PluginActor>("controller");
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
