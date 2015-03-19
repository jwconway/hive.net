using System;
using Akka.Actor;

namespace Hoist.Plugin
{
	/// <summary>
	/// This actor will handle control messages from the host in the plugins own app domain
	/// </summary>
	public class PluginActor : UntypedActor
	{
		protected override void OnReceive(object message)
		{
			if (message is RunMessage)
			{
				return; //ignore this one for now, it will load dependencies and stuff later
			}

			Console.WriteLine("Thanks for saying '" + message + "'!!");
		}
	}
}
