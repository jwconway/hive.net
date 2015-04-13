using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit;
using Xunit;

namespace Hive.Plugin.Tests
{
	public class Plugin_DI_TestSpec : TestKitBase
	{
		public Plugin_DI_TestSpec() : base(new XunitAssertions())
		{
		}

		[Fact]
		private void Plugin_initialises_DI_Hook_Called()
		{
			
		}
	}
}
