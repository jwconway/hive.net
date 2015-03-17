using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jaywayco.Dust
{
	/// <summary>
	/// Represents a single node that manages plugins
	/// </summary>
    public class Node
    {
	    private readonly IPluginStore pluginStore;
	    private readonly IPluginRegistry pluginRegistry;
	    private readonly INodeComms nodeComms;
		private readonly IPluginManager pluginManager;

		public Node(IPluginStore pluginStore, IPluginRegistry pluginRegistry, INodeComms nodeComms, IPluginManager pluginManager)
	    {
		    this.pluginStore = pluginStore;
		    this.pluginRegistry = pluginRegistry;
		    this.nodeComms = nodeComms;
		    this.pluginManager = pluginManager;
	    }

	    public void Initialise()
		{
			pluginManager.BootstrapPlugins();
		}
    }

	/// <summary>
	/// Responsible for the initial bootstrapping of the plugin
	/// that is all
	/// </summary>
	public interface IPluginManager
	{
		void BootstrapPlugins();
	}

	/// <summary>
	/// Represents the location where plugins are kept.
	/// An implementation of this could find plugins in a referenced DLL or a nuget feed
	/// </summary>
	public interface IPluginStore
	{
		IEnumerable<IPluginInfo> GetPlugins(); 
	}
	
	/// <summary>
	/// A class or set of classes that represent a unit of work
	/// </summary>
	public interface IPlugin
	{
		void Start();
	}

	/// <summary>
	/// Metadata about plugins
	/// </summary>
	public interface IPluginInfo
	{
		string FullyQualifiedClassName { get; }
		string AssemblyPath { get; }
	}

	public class PluginInfo : IPluginInfo
	{
		public string FullyQualifiedClassName { get; set; }
		public string AssemblyPath { get; set; }
	}

	/// <summary>
	/// Represents the communication interface between nodes
	/// </summary>
	public interface INodeComms
	{
		
	}

	/// <summary>
	/// Represents the communication interface between plugins
	/// </summary>
	public interface IPluginComms
	{
		
	}

	/// <summary>
	/// Represents a record of all plugins that have been loaded and their states
	/// </summary>
	public interface IPluginRegistry
	{
		void AddPlugin(IPluginInfo pluginInfo);
	}
}
