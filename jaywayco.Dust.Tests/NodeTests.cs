using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jaywayco.Dust.PluginStores;
using Moq;
using NUnit.Framework;

namespace jaywayco.Dust.Tests
{
	[TestFixture]
    public class NodeTests
    {
		private Mock<IPluginStore> pluginStoreMock;
		private Mock<IPluginRegistry> pluginRegistryMock;
		private Mock<INodeComms> nodeCommsMock;
		private Mock<IPluginManager> pluginManagerMock;
		private Node node;

		[TestFixtureSetUp]
		public void Setup()
		{
			pluginStoreMock = new Mock<IPluginStore>();
			pluginRegistryMock = new Mock<IPluginRegistry>();
			nodeCommsMock = new Mock<INodeComms>();
			pluginManagerMock = new Mock<IPluginManager>();
		}

		[Test]
		public void Call_initialise_loads_no_plugin_from_pluginstore_add_plugin_not_called()
		{
			node = new Node(pluginStoreMock.Object, pluginRegistryMock.Object, nodeCommsMock.Object, pluginManagerMock.Object);
			node.Initialise();

			pluginStoreMock.Verify(store => store.GetPlugins()); 
			Assert.Throws<MockException>(()=> pluginManagerMock.Verify(manager => manager.BootstrapPlugins()));
			Assert.Throws<MockException>(()=> pluginRegistryMock.Verify(registry => registry.AddPlugin(It.IsAny<IPluginInfo>())));
		}

		[Test]
		public void Call_initialise_loads_single_plugin_from_pluginstore_calls_pluginregistry_addplugin()
		{
			pluginStoreMock.Setup(store => store.GetPlugins()).Returns(new List<IPluginInfo>() { Mock.Of<IPluginInfo>() });
			node = new Node(pluginStoreMock.Object, pluginRegistryMock.Object, nodeCommsMock.Object, pluginManagerMock.Object);
			node.Initialise();

			pluginStoreMock.Verify(store => store.GetPlugins());
			pluginManagerMock.Verify(manager => manager.BootstrapPlugins());
			pluginRegistryMock.Verify(registry => registry.AddPlugin(It.IsAny<IPluginInfo>()));
		}

		[Test]
		public void Test()
		{
			var localFolderPluginStoreSettingsMock = new Mock<ILocalFolderPluginStoreSettings>();
			localFolderPluginStoreSettingsMock.Setup(settings => settings.FolderPath).Returns(@"C:\GitRepo\jaywayco.Dust\jaywayco.Dust.Plugins\bin\Debug");

			node = new Node(new LocalFolderPluginStore(localFolderPluginStoreSettingsMock.Object), pluginRegistryMock.Object, nodeCommsMock.Object, new PluginManager(new LocalFolderPluginStore(localFolderPluginStoreSettingsMock.Object), pluginRegistryMock.Object));
			node.Initialise(); 
		}
    }
}
