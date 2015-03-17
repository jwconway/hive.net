using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Tombola.Logging;

namespace jaywayco.Dust
{
	public abstract class PluginSetupBase
	{
		public abstract void Initialize(ILoggingService logger);
	}
}
