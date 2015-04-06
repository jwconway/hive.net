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
	}

	public class PluginActions
	{
		public class StartRequest
		{	
		}

		public class AcknowledgedResponse
		{
		}

		public class NotUnderstoodResponse
		{
		}

		public class StatusRequest
		{
			
		}
	}
}
