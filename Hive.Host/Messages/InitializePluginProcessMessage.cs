using System;

namespace Hive.Host.Messages
{
	public class InitializePluginProcessMessage
	{
		public InitializePluginProcessMessage(AppDomain pluginAppDomain, string pluginAssemblyName)
		{
			PluginAssemblyName = pluginAssemblyName;
			PluginAppDomain = pluginAppDomain;
		}

		public AppDomain PluginAppDomain { get; private set; }
		public string PluginAssemblyName { get; private set; }
	}
}