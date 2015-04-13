using System;
using Akka.Actor;
using Hive.Plugin.Messages.Control;
using Hive.Plugin.Plugin;

namespace Hive.Plugin.Actors.Plugin
{
	public class PluginActor : UntypedActor
	{
		private readonly IPluginAppStart pluginAppStart;

		public PluginActor(IPluginAppStart pluginAppStart)
		{
			this.pluginAppStart = pluginAppStart;
		}

		protected override void OnReceive(object message)
		{
			if (message is PluginControlMessages.Start)
			{
				var startMessage = (PluginControlMessages.Start)message;
				try
				{	
					Sender.Tell(new PluginStatusChangeMessage(PluginStatus.Starting));
					pluginAppStart.StartPluginApp();
					Sender.Tell(new PluginStatusChangeMessage(PluginStatus.Running));
				}
				catch(Exception ex)
				{
					Sender.Tell(new PluginStatusChangeMessage(PluginStatus.Faulted));
				}
			}
			else if (message is PluginControlMessages.Stop)
			{
				var stopMessage = (PluginControlMessages.Stop)message;
				try
				{
					Sender.Tell(new PluginStatusChangeMessage(PluginStatus.Stopping));
					pluginAppStart.StopPluginApp();
					Sender.Tell(new PluginStatusChangeMessage(PluginStatus.Stopped));
				}
				catch (Exception ex)
				{
					Sender.Tell(new PluginStatusChangeMessage(PluginStatus.Faulted));
				}
			}
			else if (message is PluginControlMessages.Pause)
			{
				var pauseMessage = (PluginControlMessages.Pause)message;
				try
				{
					Sender.Tell(new PluginStatusChangeMessage(PluginStatus.Pausing));
					pluginAppStart.PausePluginApp();
					Sender.Tell(new PluginStatusChangeMessage(PluginStatus.Paused));
				}
				catch (Exception ex)
				{
					Sender.Tell(new PluginStatusChangeMessage(PluginStatus.Faulted));
				}
			}
			else if (message is PluginControlMessages.UnPause)
			{
				var pauseMessage = (PluginControlMessages.UnPause)message;
				try
				{
					Sender.Tell(new PluginStatusChangeMessage(PluginStatus.Unpausing));
					pluginAppStart.UnPausePluginApp();
					Sender.Tell(new PluginStatusChangeMessage(PluginStatus.Running));
				}
				catch (Exception ex)
				{
					Sender.Tell(new PluginStatusChangeMessage(PluginStatus.Faulted));
				}
			}
			else if (message is PluginControlMessages.Delete)
			{
				var pauseMessage = (PluginControlMessages.Delete)message;
				try
				{
					Sender.Tell(new PluginStatusChangeMessage(PluginStatus.Deleting));
					pluginAppStart.DeletePluginApp();
					Sender.Tell(new PluginStatusChangeMessage(PluginStatus.Deleted));
				}
				catch (Exception ex)
				{
					Sender.Tell(new PluginStatusChangeMessage(PluginStatus.Faulted));
				}
			}
			else
			{
				Unhandled(message);
			}
		}
	}
}