using System;
using System.Diagnostics;
using System.Threading;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit;
using Hive.Plugin.Actors.Plugin;
using Hive.Plugin.Messages.Control;
using Hive.Plugin.Plugin;
using Moq;

namespace Hive.Plugin.Tests
{
	public class PluginControllerActor_Base_TestSpec : TestKitBase
	{
		protected PluginControllerActor_Base_TestSpec(XunitAssertions xunitAssertions)
			: base(xunitAssertions)
		{
		}

		protected void StartPlugin(IActorRef pluginControllerActorRef)
		{
			pluginControllerActorRef.Tell(new PluginAction<StartRequest>());
			this.ExpectMsg<PluginResponse<AcknowledgedResponse>>();
			Thread.Sleep(1000);
			WaitForStatus(PluginStatus.Running, pluginControllerActorRef);
		}

		protected IActorRef SendRequestGetAck(object pluginControllerRequest, Mock<IPluginAppStart> moqPluginStarter = null, IActorRef pluginControllerActorRef = null)
		{
			if(moqPluginStarter==null)
			{
				moqPluginStarter = new Mock<IPluginAppStart>();
			}
			if (pluginControllerActorRef == null)
			{
				pluginControllerActorRef = ActorOf(Props.Create(() => new PluginControllerActor(moqPluginStarter.Object)));
			}
			pluginControllerActorRef.Tell(pluginControllerRequest);
			this.ExpectMsg<PluginResponse<AcknowledgedResponse>>();
			return pluginControllerActorRef;
		}

		protected void WaitForStatus(PluginStatus pluginStatus, IActorRef pluginControllerActorRef)
		{
			int attempts = 1;
			while (true)
			{
				try
				{
					Trace.WriteLine(string.Format("=====================trying [Expect {0}]==================...", pluginStatus));
					pluginControllerActorRef.Tell(new PluginAction<StatusRequest>());
					var result = ExpectMsg(new PluginStatusResponse(pluginStatus));
					break;
				}
				catch(Exception ex)
				{
					Trace.Write(ex);
					attempts++;
					if(attempts > 5)
						throw;
				}
				Thread.Sleep(100);
				Trace.WriteLine("=============================================...");
			}
		}
	}
}