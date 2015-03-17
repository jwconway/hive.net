using System;
using System.IO;
using System.Security.Policy;

namespace jaywayco.Dust
{
	public class PluginManager : IPluginManager
	{
		private readonly IPluginStore pluginStore;
		private readonly IPluginRegistry pluginRegistry;

		public PluginManager(IPluginStore pluginStore, IPluginRegistry pluginRegistry)
		{
			this.pluginStore = pluginStore;
			this.pluginRegistry = pluginRegistry;
		}

		public void BootstrapPlugins()
		{
			var pluginInfos = pluginStore.GetPlugins();
			foreach (var pluginInfo in pluginInfos)
			{
				BootstrapPlugin(pluginInfo);
			}
		}

		private void BootstrapPlugin(IPluginInfo pluginInfo)
		{
			AppDomain appDomain = AppDomain.CreateDomain(pluginInfo.FullyQualifiedClassName, new Evidence(), Path.GetDirectoryName(pluginInfo.AssemblyPath), AppDomain.CurrentDomain.BaseDirectory, false);
			appDomain.DoCallBack(new CrossAppDomainDelegate(PluginMarshall.InitializePlugin));
		}
	}
}