using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jaywayco.Dust.PluginStores
{
	public abstract class PluginStoreBase : IPluginStore
	{
		public abstract IEnumerable<IPluginInfo> GetPlugins();
	}
}
