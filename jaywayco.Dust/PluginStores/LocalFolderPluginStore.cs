using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace jaywayco.Dust.PluginStores
{
	public class LocalFolderPluginStore : IPluginStore
	{
		private readonly ILocalFolderPluginStoreSettings localFolderPluginStoreSettings;

		public LocalFolderPluginStore(ILocalFolderPluginStoreSettings localFolderPluginStoreSettings)
		{
			this.localFolderPluginStoreSettings = localFolderPluginStoreSettings;
		}

		public IEnumerable<IPluginInfo> GetPlugins()
		{
			var pluginInstances = new List<IPluginInfo>();
			var dlls = Directory.GetFiles(localFolderPluginStoreSettings.FolderPath, "*.dll");
			foreach (var dll in dlls)
			{
				var assembly = Assembly.LoadFrom(dll);
				var plugins = assembly.GetTypes().Where(type => type.GetInterfaces().Any(type1 => type1.FullName == typeof(IPlugin).FullName));
				foreach (var plugin in plugins)
				{
					pluginInstances.Add(new PluginInfo()
					{
						AssemblyPath = Path.Combine(localFolderPluginStoreSettings.FolderPath, dll),
						FullyQualifiedClassName = plugin.FullName
					});
				}
			}

			return pluginInstances;
		}
	}

	public interface ILocalFolderPluginStoreSettings
	{
		string FolderPath { get; }
	}
}
