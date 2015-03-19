using System.Threading;
using Akka.Actor;
using Akka.Configuration;

namespace Hoist.Plugin
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
			var bind = new Akka.Remote.AddressUid();//this serves no other purpose than to load the Akk.Remote assembly into this app domain
			PluginHelper.ForceLoadAssemblies();
			
			var config = ConfigurationFactory.ParseString(@"
			akka {
				actor {
					provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
				}

				remote {
					helios.tcp {
						transport-class = ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
						applied-adapters = []
						transport-protocol = tcp
						port = " + port.ToString() + @"
						hostname = localhost
					}
				}
			} 
			");
			using (var system = ActorSystem.Create("testclient", config))
			{
				system.ActorOf<PluginActor>("actorSetup");
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
