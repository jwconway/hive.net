using System;
using Hive.Plugin.Actors;

namespace Hive.Plugin.Plugin.Process
{
	/// <summary>
	/// This is the hook that is used to initialize the plugin in its own process
	/// This is the first point of contact between the plugin appdomain and the host appdomain
	/// </summary>
	[Serializable]
	public class PluginProcessMarshaller : MarshalByRefObject
	{
		public void SetupPluginProcessActorSystem()
		{
			try
			{
				var systemSetup = new PluginActorSystemSetup();
				systemSetup.Initialize();
			}
			catch (Exception ex)
			{
				
			}
		}
		
	}
}
