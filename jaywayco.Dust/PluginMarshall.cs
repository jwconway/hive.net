using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Core;
using Tombola.Logging;

namespace jaywayco.Dust
{
	/// <summary>
	/// This is the hook that is used to initialize the plugin in its own process
	/// This is the first point of contact between the plugin appdomain and the host appdomain
	/// </summary>
	public class PluginMarshall
	{
		public static void InitializePlugin()
		{
			try
			{
				PluginHelper.ForceLoadAssemblies();
				ILoggingService logger = PluginHelper.InitialiseLogger();

				logger.LogDebug(new LoggingData("InitializePlugin"));

				var assemblies = AppDomain.CurrentDomain.GetAssemblies();
				foreach (var assembly in assemblies)
				{
					var pluginSetupType = assembly.GetTypes().FirstOrDefault(type => typeof(PluginSetupBase).IsAssignableFrom(type));
					if (pluginSetupType == null)
					{
						//this assembly doesnt have an implementation of PluginSetupBase
						continue;
					}
					if (pluginSetupType.IsAbstract)
					{
						//this is the PluginSetupBase abstract base class
						continue;
					}
					var pluginSetup = Activator.CreateInstance(pluginSetupType) as PluginSetupBase;
					pluginSetup.Initialize(logger);
				}
			}
			catch (Exception ex)
			{
				
			}
		}
		
	}
}
