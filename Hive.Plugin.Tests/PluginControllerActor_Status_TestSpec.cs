using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
	public class PluginControllerActor_Status_TestSpec : TestKitBase
	{
		public PluginControllerActor_Status_TestSpec() : base(new XunitAssertions())
		{
		}

		[Fact]
		private void Can_ask_for_status_and_receive_status_message()
		{
			var pluginControllerActorRef = ActorOf(Props.Create(() => new PluginControllerActor(Mock.Of<IPluginAppStart>())));
			pluginControllerActorRef.Tell(new PluginAction<StatusRequest>());
			ExpectMsg<PluginStatusResponse>();
		}

		[Fact]
		private void Default_status_must_be_Stopped()
		{
			var pluginControllerActorRef = ActorOf(Props.Create(() => new PluginControllerActor(Mock.Of<IPluginAppStart>())));
			var pluginStatus = pluginControllerActorRef.Ask<PluginStatusResponse>(new PluginAction<StatusRequest>()).Result;
			Assert.Equal(PluginStatus.Stopped, pluginStatus.PluginStatus);
		}
	}
}
