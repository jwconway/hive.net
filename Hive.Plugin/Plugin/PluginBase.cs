using System;
using Akka.Actor;
using Akka.DI.Core;
using Akka.DI.Unity;
using Hive.Plugin.Actors.Plugin;
using Microsoft.Practices.Unity;

namespace Hive.Plugin.Plugin
{
	public abstract class PluginBase : IPlugin
	{
		public abstract void RegisterPluginDependencies(UnityContainer builder);
		public abstract IPluginAppStart GetPluginAppStart();

		internal IDependencyResolver Initialise(ActorSystem system)
		{
			var container = new UnityContainer();

			//TODO: register common services here
			container.RegisterInstance(GetPluginAppStart());
			container.RegisterType<PluginControllerActor>();
			container.RegisterInstance(new PluginIdentity(Guid.NewGuid().ToString()), new ContainerControlledLifetimeManager());
			RegisterPluginDependencies(container);

			return new UnityDependencyResolver(container, system);
		}
	}

	public class PluginIdentity : IPluginIdentity
	{
		public PluginIdentity(string pluginId)
		{
			PluginId = pluginId;
		}

		public string PluginId { get; private set; }
	}
}
