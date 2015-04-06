namespace Hive.Plugin.Messages.Control
{
	public class PluginStatusChangeMessage
	{
		public PluginStatusChangeMessage(PluginStatus oldStatus, PluginStatus newStatus)
		{
			NewStatus = newStatus;
			OldStatus = oldStatus;
		}

		public PluginStatus OldStatus { get; private set; }
		public PluginStatus NewStatus { get; private set; }
	}
}