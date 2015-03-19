using System;
using System.Threading;
using Akka.Actor;
using Akka.Configuration;
using Hoist.Host.Actors;
using Hoist.Host.Messages;

namespace Hoist.Tester
{
	class Program
	{
		static void Main(string[] args)
		{
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
						port = 50003
						hostname = localhost
					}
				}
			}
			");

			using (var system = ActorSystem.Create("test", config))
			{
				var broadCasterRef = system.ActorOf(Props.Create<PluginBroadCaster>(), "pluginbroadcaster");
				system.ActorOf(Props.Create<NodeActor>(), "Node").Tell(new GetAllPluginsRequest(@"C:\GitRepo\Hoist\Hoist.Test.Plugins\bin\Debug"));
				Thread.Sleep(6000);
				while (true)
				{
					Console.WriteLine("Why dont you say hello to the plugins?:");
					var message = Console.ReadLine();
					broadCasterRef.Tell(message);
				}
			}

			Console.ReadLine();
		}
	}
}
