﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Akka.Actor;
using Akka.DI.Core;
using Hive.Plugin.Messages.Control;
using Hive.Plugin.Plugin;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using Tombola.Logging;
using Tombola.Logging.Log4Net;

namespace Hive.Plugin
{
	public class PluginHelper
	{
		public static void ForceLoadAssemblies()
		{
			PreLoadAssembliesFromPath(AppDomain.CurrentDomain.BaseDirectory);
		}

		public static ILoggingService InitialiseLogger()
		{
			var layout = new PatternLayout("%-4timestamp [%thread] %-5level %logger %ndc - %message%newline");
			var appender = new RollingFileAppender
			{
				File = @"C:\TEMP\my.log",
				Layout = layout
			};
			layout.ActivateOptions();
			appender.ActivateOptions();
			BasicConfigurator.Configure(appender);
			return new LoggingService(LogManager.GetLogger(typeof(PluginHelper)));
		}

		private static void PreLoadAssembliesFromPath(string p)
		{
			//get all .dll files from the specified path and load the lot
			FileInfo[] files = null;
			//you might not want recursion - handy for localised assemblies 
			//though especially.
			files = new DirectoryInfo(p).GetFiles("*.dll",
				SearchOption.AllDirectories);

			AssemblyName a = null;
			string s = null;
			foreach (var fi in files)
			{
				s = fi.FullName;
				//now get the name of the assembly you've found, without loading it
				//though (assuming .Net 2+ of course).
				a = AssemblyName.GetAssemblyName(s);
				//sanity check - make sure we don't already have an assembly loaded
				//that, if this assembly name was passed to the loaded, would actually
				//be resolved as that assembly.  Might be unnecessary - but makes me
				//happy :)
				if (!AppDomain.CurrentDomain.GetAssemblies().Any(assembly =>
				  AssemblyName.ReferenceMatchesDefinition(a, assembly.GetName())))
				{
					//crucial - USE THE ASSEMBLY NAME.
					//in a web app, this assembly will automatically be bound from the 
					//Asp.Net Temporary folder from where the site actually runs.
					Assembly.Load(a);
				}
			}
		}

		public static IDependencyResolver InitializePlugin(ActorSystem system)
		{
			ForceLoadAssemblies();
			//get implementations of IPlugin
			var pluginImplementation = AppDomain.CurrentDomain
				.GetAssemblies()
				.SelectMany(e => e.GetTypes())
				.Where(type => type.IsClass && !type.IsAbstract)
				.First(type => type.GetInterfaces().Any(interfaceType => interfaceType.FullName == typeof(IPlugin).FullName));

			var pluginBase = Activator.CreateInstance(pluginImplementation) as PluginBase;

			return pluginBase.Initialise(system);
		}
	}

	public static class ActorExtension
	{
		internal static bool IsForMe(this object message, string pluginId)
		{
			if(message is ITargetPlugin)
			{
				//message is for plugin if TargetPluginId is empty or equal to pluginId
				return (((ITargetPlugin)message).TargetPluginId ?? pluginId) == pluginId;
			}
			return false;
		}
	}
}
