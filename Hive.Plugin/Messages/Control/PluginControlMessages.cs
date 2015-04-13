using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hive.Plugin.Messages.Control
{
	internal class PluginControlMessages
	{
		internal class Start
		{
			public Start(PluginStatus currentStatus)
			{
				CurrentStatus = currentStatus;
			}

			public PluginStatus CurrentStatus { get; private set; }
		}

		public class Stop
		{
			public PluginStatus CurrentStatus { get; private set; }

			public Stop(PluginStatus currentStatus)
			{
				CurrentStatus = currentStatus;
			}
		}

		public class Pause
		{
			public PluginStatus CurrentStatus { get; private set; }

			public Pause(PluginStatus currentStatus)
			{
				CurrentStatus = currentStatus;
			}
		}

		public class Delete
		{
			public PluginStatus CurrentStatus { get; private set; }

			public Delete(PluginStatus currentStatus)
			{
				CurrentStatus = currentStatus;
			}
		}

		public class UnPause
		{
			public PluginStatus CurrentStatus { get; private set; }

			public UnPause(PluginStatus currentStatus)
			{
				CurrentStatus = currentStatus;
			}
		}
	}

	public abstract class PluginActionRequest { }
	
	public class PluginAction<TRequest> : ITargetPlugin where TRequest : PluginActionRequest
	{
		public PluginAction() { } 
		public PluginAction(string targetPluginId)
		{
			TargetPluginId = targetPluginId;
		}

		public string TargetPluginId { get; private set; }
	}

	public interface ITargetPlugin
	{
		string TargetPluginId { get; }
	}

	public abstract class PluginActionResponse { }
	public class PluginResponse<TResponse> where TResponse : PluginActionResponse { }

	public class StartRequest : PluginActionRequest
	{
	}

	public class AcknowledgedResponse : PluginActionResponse
	{
	}

	public class NotUnderstoodResponse : PluginActionResponse
	{
	}

	public class StatusRequest : PluginActionRequest
	{
	}

	public class StopRequest : PluginActionRequest
	{
	}

	public class PauseRequest : PluginActionRequest
	{
	}

	public class DeleteRequest : PluginActionRequest
	{
	}

	public class NotSupportedResponse : PluginActionResponse
	{
	}

	public class UnPauseRequest : PluginActionRequest
	{
	}
}
