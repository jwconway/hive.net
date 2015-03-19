using System;

namespace Hoist.Host.Messages
{
	public class InitializePluginProcessMessage
	{
		public InitializePluginProcessMessage(AppDomain pluginAppDomain, string pluginAssemblyName, int port)
		{
			Port = port;
			PluginAssemblyName = pluginAssemblyName;
			PluginAppDomain = pluginAppDomain;
		}

		public AppDomain PluginAppDomain { get; private set; }
		public string PluginAssemblyName { get; private set; }
		public int Port { get; private set; }
	}
}