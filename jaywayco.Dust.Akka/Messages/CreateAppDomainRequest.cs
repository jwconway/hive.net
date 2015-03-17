namespace jaywayco.Dust.Actors.Actors
{
	public class CreateAppDomainRequest
	{
		public CreateAppDomainRequest(string appDomainName, string appDomainBasePath)
		{
			AppDomainBasePath = appDomainBasePath;
			AppDomainName = appDomainName;
		}

		public string AppDomainName { get; private set; }
		public string AppDomainBasePath { get; private set; }
	}
}