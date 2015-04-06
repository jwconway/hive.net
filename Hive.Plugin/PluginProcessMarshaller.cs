using System;
using Hive.Plugin.Actors.Plugin;

namespace Hive.Plugin
{
	/// <summary>
	/// This is the hook that is used to initialize the plugin in its own process
	/// This is the first point of contact between the plugin appdomain and the host appdomain
	/// </summary>
	[Serializable]
	public class PluginProcessMarshaller : MarshalByRefObject
	{
		public void SetupPluginProcessActorSystem(int port)
		{
			try
			{
				var systemSetup = new PluginActorSystemSetup();
				systemSetup.Initialize(port);
			}
			catch (Exception ex)
			{
				
			}
		}
		
	}
}
