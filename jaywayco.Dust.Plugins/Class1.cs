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
	[Serializable]
    public class ConsoleWriterPlugin : IPlugin
    {
	    public void Start()
	    {
		    throw new NotImplementedException();
	    }
    }

	[Serializable]
    public class ConsoleWriterPlugin2 : IPlugin
    {
	    public void Start()
	    {
		    throw new NotImplementedException();
	    }
    }

	[Serializable]
    public class ConsoleWriterPlugin3 : IPlugin
    {
	    public void Start()
	    {
		    throw new NotImplementedException();
	    }
    }

	[Serializable]
    public class ConsoleWriterPlugin4 : IPlugin
    {
	    public void Start()
	    {
		    throw new NotImplementedException();
	    }
    }

	[Serializable]
    public class ConsoleWriterPlugin5 : IPlugin
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
