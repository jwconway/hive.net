using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Tombola.Logging;

namespace jaywayco.Dust.Plugins
{
    public class ConsoleWriterPlugin : IPlugin
    {
	    public void Start()
	    {
		    throw new NotImplementedException();
	    }
    }

	public class ConsoleWriterPluginSetup : PluginSetupBase
	{
		public override void Initialize(ILoggingService logger)
		{
			logger.LogDebug(new LoggingData("YEY - Initializing..."));
		}
	}
}
