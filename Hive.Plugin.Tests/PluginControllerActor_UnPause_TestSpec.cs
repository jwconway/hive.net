using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.TestKit;
using Akka.TestKit.Xunit;
using Hive.Plugin.Actors.Plugin;
using Hive.Plugin.Messages.Control;
using Hive.Plugin.Plugin;
using Moq;
using Xunit;

namespace Hive.Plugin.Tests
{
    public class PluginControllerActor_UnPause_TestSpec : PluginControllerActor_Base_TestSpec
    {
	    public PluginControllerActor_UnPause_TestSpec() : base(new XunitAssertions())
	    {
	    }

		[Fact]
		private void Can_send_upause_message()
		{
			var pluginControllerActorRef = ActorOf(Props.Create(() => new PluginControllerActor(Mock.Of<IPluginAppStart>())));
			pluginControllerActorRef.Tell(new PluginAction<UnPauseRequest>());
		}

		[Fact]
		private void Can_send_pause_message_and_recieve_acknowledgement_message_then_get_status()
		{
			var moqPluginStarter = new Mock<IPluginAppStart>();
			var pluginControllerActorRef = ActorOf(Props.Create(() => new PluginControllerActor(moqPluginStarter.Object)));
			StartPlugin(pluginControllerActorRef);
			pluginControllerActorRef.Tell(new PluginAction<PauseRequest>());
			ExpectMsg<PluginResponse<AcknowledgedResponse>>();
			var pluginStatus = pluginControllerActorRef.Ask(new PluginAction<StatusRequest>()).Result;
			Assert.NotNull(pluginStatus);
		}

		[Fact]
		private void Send_unpause_request_to_paused_plugin_get_ack()
		{
			var moqPluginStarter = new Mock<IPluginAppStart>();
			var pluginControllerActorRef = ActorOf(Props.Create(() => new PluginControllerActor(moqPluginStarter.Object)));
			StartPlugin(pluginControllerActorRef);
			pluginControllerActorRef.Tell(new PluginAction<PauseRequest>());
			ExpectMsg<PluginResponse<AcknowledgedResponse>>();
			WaitForStatus(PluginStatus.Paused, pluginControllerActorRef);
			pluginControllerActorRef.Tell(new PluginAction<UnPauseRequest>());
			ExpectMsg<PluginResponse<AcknowledgedResponse>>();
		}

		[Fact]
		private void Send_unpause_request_to_stopped_plugin_get_invalid_request()
		{
			var moqPluginStarter = new Mock<IPluginAppStart>();
			var pluginControllerActorRef = ActorOf(Props.Create(() => new PluginControllerActor(moqPluginStarter.Object)));
			StartPlugin(pluginControllerActorRef);
			pluginControllerActorRef.Tell(new PluginAction<PauseRequest>());
			ExpectMsg<PluginResponse<AcknowledgedResponse>>();
			WaitForStatus(PluginStatus.Paused, pluginControllerActorRef);
			pluginControllerActorRef.Tell(new PluginAction<DeleteRequest>());
			ExpectMsg<PluginResponse<NotSupportedResponse>>();
		}

		[Fact]
		private void Send_unpause_request_to_starting_plugin_get_invalid_request()
		{
			var mre = new ManualResetEventSlim();
			var moqPluginStarter = new Mock<IPluginAppStart>();
			moqPluginStarter.Setup(e => e.StartPluginApp()).Callback(() => mre.Wait(10000));
			var pluginControllerActorRef = ActorOf(Props.Create(() => new PluginControllerActor(moqPluginStarter.Object)));
			pluginControllerActorRef.Tell(new PluginAction<StartRequest>());
			ExpectMsg<PluginResponse<AcknowledgedResponse>>();
			WaitForStatus(PluginStatus.Starting, pluginControllerActorRef);
			pluginControllerActorRef.Tell(new PluginAction<UnPauseRequest>());
			ExpectMsg<PluginResponse<NotSupportedResponse>>();
			mre.Set();
		}
		
		[Fact]
		private void Send_unpause_request_to_running_plugin_get_invalid_request()
		{
			var moqPluginStarter = new Mock<IPluginAppStart>();
			var pluginControllerActorRef = ActorOf(Props.Create(() => new PluginControllerActor(moqPluginStarter.Object)));
			StartPlugin(pluginControllerActorRef);
			WaitForStatus(PluginStatus.Running, pluginControllerActorRef);
			pluginControllerActorRef.Tell(new PluginAction<UnPauseRequest>());
			ExpectMsg<PluginResponse<NotSupportedResponse>>();
		}
		
		[Fact]
		private void Send_unpause_request_to_pausing_plugin_get_invalid_request()
		{
			var mre = new ManualResetEventSlim();
			var moqPluginStarter = new Mock<IPluginAppStart>();
			moqPluginStarter.Setup(e => e.PausePluginApp()).Callback(() => mre.Wait(10000));
			var pluginControllerActorRef = ActorOf(Props.Create(() => new PluginControllerActor(moqPluginStarter.Object)));
			StartPlugin(pluginControllerActorRef);
			pluginControllerActorRef.Tell(new PluginAction<PauseRequest>());
			ExpectMsg<PluginResponse<AcknowledgedResponse>>();
			WaitForStatus(PluginStatus.Pausing, pluginControllerActorRef);
			pluginControllerActorRef.Tell(new PluginAction<UnPauseRequest>());
			ExpectMsg<PluginResponse<NotSupportedResponse>>();
			mre.Set();
		}
		
		[Fact]
		private void Send_unpause_request_to_unpausing_plugin_get_invalid_request()
		{
			var mre = new ManualResetEventSlim();
			var moqPluginStarter = new Mock<IPluginAppStart>();
			moqPluginStarter.Setup(e => e.UnPausePluginApp()).Callback(() => mre.Wait(10000));
			var pluginControllerActorRef = ActorOf(Props.Create(() => new PluginControllerActor(moqPluginStarter.Object)));
			StartPlugin(pluginControllerActorRef);
			pluginControllerActorRef.Tell(new PluginAction<PauseRequest>());
			ExpectMsg<PluginResponse<AcknowledgedResponse>>();
			WaitForStatus(PluginStatus.Paused, pluginControllerActorRef);
			pluginControllerActorRef.Tell(new PluginAction<UnPauseRequest>());
			ExpectMsg<PluginResponse<AcknowledgedResponse>>();
			WaitForStatus(PluginStatus.Unpausing, pluginControllerActorRef);
			pluginControllerActorRef.Tell(new PluginAction<UnPauseRequest>());
			ExpectMsg<PluginResponse<NotSupportedResponse>>();
		}
		
		[Fact]
		private void Send_unpause_request_to_stopping_plugin_get_invalid_request()
		{
			var mre = new ManualResetEventSlim();
			var moqPluginStarter = new Mock<IPluginAppStart>();
			moqPluginStarter.Setup(e => e.StopPluginApp()).Callback(() => mre.Wait(10000));
			var pluginControllerActorRef = ActorOf(Props.Create(() => new PluginControllerActor(moqPluginStarter.Object)));
			StartPlugin(pluginControllerActorRef);
			pluginControllerActorRef.Tell(new PluginAction<StopRequest>());
			ExpectMsg<PluginResponse<AcknowledgedResponse>>();
			WaitForStatus(PluginStatus.Stopping, pluginControllerActorRef);
			pluginControllerActorRef.Tell(new PluginAction<UnPauseRequest>());
			ExpectMsg<PluginResponse<NotSupportedResponse>>();
		}
		
		[Fact]
		private void Send_unpause_request_to_faulted_plugin_get_invalid_request()
		{
			var mre = new ManualResetEventSlim();
			var moqPluginStarter = new Mock<IPluginAppStart>();
			moqPluginStarter.Setup(e => e.StartPluginApp()).Throws(new Exception("FAKE!!!"));
			var pluginControllerActorRef = ActorOf(Props.Create(() => new PluginControllerActor(moqPluginStarter.Object)));
			pluginControllerActorRef.Tell(new PluginAction<StartRequest>());
			ExpectMsg<PluginResponse<AcknowledgedResponse>>();
			WaitForStatus(PluginStatus.Faulted, pluginControllerActorRef);
			pluginControllerActorRef.Tell(new PluginAction<UnPauseRequest>());
			ExpectMsg<PluginResponse<NotSupportedResponse>>();
		}
    }

	
}
