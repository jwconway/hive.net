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
using Moq;
using Xunit;

namespace Hive.Plugin.Tests
{
    public class PluginControllerActorSpec : TestKitBase
    {
	    public PluginControllerActorSpec() : base(new XunitAssertions())
	    {
	    }

		[Fact]
		private void Can_ask_for_status_and_receive_status_message()
		{
			var pluginControllerActorRef = ActorOf(Props.Create(() => new PluginControllerActor(new PluginAppStart())));
			pluginControllerActorRef.Tell(new PluginActions.StatusRequest());
			ExpectMsg<PluginStatusResponse>();
		}

		[Fact]
		private void Can_send_start_message()
		{
			var pluginControllerActorRef = ActorOf(Props.Create(() => new PluginControllerActor(new PluginAppStart())));
			pluginControllerActorRef.Tell(new PluginActions.StartRequest());
		}

		[Fact]
		private void Can_send_start_message_and_recieve_acknowledgemnt()
		{
			var pluginControllerActorRef = ActorOf(Props.Create(() => new PluginControllerActor(new PluginAppStart())));
			pluginControllerActorRef.Tell(new PluginActions.StartRequest());
			this.ExpectMsg<PluginActions.AcknowledgedResponse>();
		}

		[Fact]
		private void Can_send_start_message_and_recieve_acknowledgement_message_then_get_status()
		{
			var moqPluginStarter = new Mock<IPluginAppStart>();
			var pluginControllerActorRef = ActorOf(Props.Create(() => new PluginControllerActor(moqPluginStarter.Object)));
			pluginControllerActorRef.Tell(new PluginActions.StartRequest());
			this.ExpectMsg<PluginActions.AcknowledgedResponse>();
			var pluginStatus = pluginControllerActorRef.Ask(new PluginActions.StatusRequest()).Result;
			Assert.NotNull(pluginStatus);
		}

		[Fact]
		private void Send_start_request_to_plugin_get_ack_get_running_status()
		{
			var moqPluginStarter = new Mock<IPluginAppStart>();
			var pluginControllerActorRef = ActorOf(Props.Create(() => new PluginControllerActor(moqPluginStarter.Object)));
			pluginControllerActorRef.Tell(new PluginActions.StartRequest());
			this.ExpectMsg<PluginActions.AcknowledgedResponse>();
			//WaitForStatus(PluginStatus.Starting, pluginControllerActorRef);
			//pluginControllerActorRef.Tell(new PluginActions.StatusRequest());
			//ExpectMsg(new PluginStatusResponse(PluginStatus.Starting));
			//Thread.Sleep(2000);
			WaitForStatus(PluginStatus.Running, pluginControllerActorRef);
			//pluginControllerActorRef.Tell(new PluginActions.StatusRequest());
			//ExpectMsg(new PluginStatusResponse(PluginStatus.Running));
		}

		[Fact]
		private void Send_start_request_to_faulty_plugin_get_ack_get_faulted_status()
		{
			var moqPluginStarter = new Mock<IPluginAppStart>();
			moqPluginStarter.Setup(pas => pas.StartPluginApp()).Throws(new Exception("IM FAKE!!!"));
			var pluginControllerActorRef = ActorOf(Props.Create(() => new PluginControllerActor(moqPluginStarter.Object)));
			pluginControllerActorRef.Tell(new PluginActions.StartRequest());
			this.ExpectMsg<PluginActions.AcknowledgedResponse>();
			//WaitForStatus(PluginStatus.Starting, pluginControllerActorRef);
			//pluginControllerActorRef.Tell(new PluginActions.StatusRequest());
			//ExpectMsg(new PluginStatusResponse(PluginStatus.Starting));
			WaitForStatus(PluginStatus.Faulted, pluginControllerActorRef);
			//pluginControllerActorRef.Tell(new PluginActions.StatusRequest());
			//ExpectMsg(new PluginStatusResponse(PluginStatus.Faulted));
		}

		//[Fact]
		//private void Send_start_request_to_slow_to_start_plugin_get_ack_get_starting_status_eventually_get_running()
		//{
		//	var moqPluginStarter = new Mock<IPluginAppStart>();
		//	moqPluginStarter.Setup(pas => pas.StartPluginApp()).Callback(() => Thread.Sleep(2000));
		//	var pluginControllerActorRef = ActorOf(Props.Create(() => new PluginControllerActor(moqPluginStarter.Object)));
		//	pluginControllerActorRef.Tell(new PluginActions.StartRequest());
		//	this.ExpectMsg<PluginActions.AcknowledgedResponse>();
		//	pluginControllerActorRef.Tell(new PluginActions.StatusRequest());
		//	ExpectMsg(new PluginStatusResponse(PluginStatus.Starting));
		//	Thread.Sleep(500);//not time for slow plugin app to start
		//	pluginControllerActorRef.Tell(new PluginActions.StatusRequest());
		//	ExpectMsg(new PluginStatusResponse(PluginStatus.Starting));
		//	Thread.Sleep(500);//time for slow plugin app to start
		//	pluginControllerActorRef.Tell(new PluginActions.StatusRequest());
		//	ExpectMsg(new PluginStatusResponse(PluginStatus.Starting));
		//}

		private void WaitForStatus(PluginStatus pluginStatus, IActorRef pluginControllerActorRef)
		{
			int attempts = 1;
			while (true)
			{
				try
				{
					Trace.WriteLine("=====================trying==================...");
					pluginControllerActorRef.Tell(new PluginActions.StatusRequest());
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
