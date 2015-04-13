using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Actor.Dsl;
using Hive.Host.Actors.System.Plugin.Store;
using Hive.Host.Messages;

namespace Hive.Host
{
	public static class HiveExtensions
	{
		public static Hive WithPluginStore<TPluginStoreActor>(this Hive hive) where TPluginStoreActor : ActorBase
		{
			hive.System.ActorOf(Props.Create<TPluginStoreActor>(), "pluginStoreActor");
			return hive;
		}

		public static Hive StartWatchingForPlugins(this Hive hive, object pluginStoreActorMessage)
		{
			var pluginStoreActor = hive.System.ActorSelection("user/pluginStoreActor").ResolveOne(TimeSpan.FromMilliseconds(1000)).Result;
			hive.System.Scheduler.ScheduleTellRepeatedly(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), pluginStoreActor, pluginStoreActorMessage, pluginStoreActor);
			return hive;
		}
	}
}