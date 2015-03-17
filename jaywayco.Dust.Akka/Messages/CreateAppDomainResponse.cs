using System;

namespace jaywayco.Dust.Actors.Actors
{
	public class CreateAppDomainResponse
	{
		public CreateAppDomainResponse(AppDomain appDomain)
		{
			AppDomain = appDomain;
		}

		public AppDomain AppDomain { get; private set; }
	}
}