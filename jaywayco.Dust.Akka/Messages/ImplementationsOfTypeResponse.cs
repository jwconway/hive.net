using System;
using System.Collections.Generic;

namespace jaywayco.Dust.Actors.Messages
{
	public class ImplementationsOfTypeResponse
	{
		public ImplementationsOfTypeResponse(IEnumerable<Type> implementations)
		{
			Implementations = implementations;
		}

		public IEnumerable<Type> Implementations { get; private set; } 
	}
}