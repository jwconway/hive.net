using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka;
using Akka.Actor;
using Akka.Event;
using jaywayco.Dust.Actors.Actors;
using jaywayco.Dust.Actors.Messages;

namespace Tester
{
	class Program
	{
		static void Main(string[] args)
		{
			var fluentConfig = 
				FluentConfig.Begin()
				//.LogLevel(LogLevel.DebugLevel)
				//.StdOutLogLevel(LogLevel.DebugLevel)
				.DebugUnhandled(true)
				//.LogLocal(
				//	receive: true,
				//	autoReceive: true,
				//	lifecycle: true,
				//	eventStream: true,
				//	unhandled: true
				//)
				.LogConfigOnStart(true);
			using (var system = ActorSystem.Create("test", fluentConfig.Build()))
			{
				system.ActorOf(Props.Create<NodeActor>(), "Node").Tell(new GetAllPluginsRequest(@"C:\GitRepo\jaywayco.Dust\jaywayco.Dust.Plugins\bin\Debug"));
				Console.ReadLine();
			}

			Console.ReadLine();
		}
	}
}
