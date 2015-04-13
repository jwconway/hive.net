using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Akka.Configuration;
using Hive.Host.Actors.System.Cluster;

namespace Hive.Host
{
	public class Hive : IDisposable
	{
		protected Hive()
		{
		}

		public static Hive Join()
		{
			var hive = new Hive();
			hive.Configure();
			return hive;
		}

		internal ActorSystem System { get; private set; }
		private IActorRef ClusterRegistryRef { get; set; }

		private void Configure()
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

			System = ActorSystem.Create("Hive", config);
			ClusterRegistryRef = System.ActorOf(Props.Create<ClusterBusNodeActor>(), "bus");
			System.ActorOf(Props.Create(() => new ClusterMonitorActor(ClusterRegistryRef)));
		}

		

		public void NotifyMembers(object message)
		{
			ClusterRegistryRef.Tell(message);
		}

		public IEnumerable<PluginInfo> GetMembers()
		{
			var plugins = ClusterRegistryRef.Ask<ListPluginsResponse>(new ListPluginsRequest()).Result;
			return plugins.Members.Select(actor => new PluginInfo(actor.Path.Address.ToString()));
		}

		public void Dispose()
		{
			System.Dispose();
			System = null;
		}
	}

	public class PluginInfo
	{
		public PluginInfo(string address)
		{
			Address = address;
		}

		public string Address { get; private set; }
	}
}