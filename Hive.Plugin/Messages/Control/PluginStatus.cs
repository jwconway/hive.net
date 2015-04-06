using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hive.Plugin.Messages.Control
{
	public enum PluginStatus
	{
		Stopped,
		Starting,
		Running,
		Pausing,
		Paused,
		Unpausing,
		Stopping,
		Faulted
	}
}
