using System;
using System.IO;
using System.Security.Policy;
using Akka.Actor;

namespace jaywayco.Dust.Actors.Actors
{
	public class AppDomainActor : BaseTypedActor, IHandle<CreateAppDomainRequest>
	{
		public void Handle(CreateAppDomainRequest message)
		{
			try
			{
				Sender.Tell(new CreateAppDomainResponse(AppDomain.CreateDomain(message.AppDomainName, new Evidence(), message.AppDomainBasePath, AppDomain.CurrentDomain.BaseDirectory, false)));
			}
			catch(Exception ex)
			{
				Logger.Error(ex, "Exception happened!!");
				Sender.Forward(new Failure() { Exception = ex, Timestamp = DateTime.UtcNow });
			}
		}
	}
}
