using System;
using System.Reflection;

namespace jaywayco.Dust.Actors.Messages
{
	public class ImplementationsOfTypeRequest
	{
		public ImplementationsOfTypeRequest(Type interfaceType, Assembly assembly)
		{
			Assembly = assembly;
			InterfaceType = interfaceType;
		}

		public Type InterfaceType { get; private set; }
		public Assembly Assembly { get; private set; }
	}
}