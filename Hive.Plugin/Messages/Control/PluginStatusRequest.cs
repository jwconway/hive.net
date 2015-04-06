namespace Hive.Plugin.Messages.Control
{
	public class PluginStatusResponse
	{
		public PluginStatusResponse(PluginStatus pluginStatus)
		{
			PluginStatus = pluginStatus;
		}

		public PluginStatus PluginStatus { get; private set; }

		public override bool Equals(object obj)
		{
			var otherStatusMessage = obj as PluginStatusResponse;
			return otherStatusMessage != null && otherStatusMessage.PluginStatus == PluginStatus; 
		}

		public override string ToString()
		{
			return PluginStatus.ToString();
		}
	}
}