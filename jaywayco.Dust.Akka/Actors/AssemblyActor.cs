using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using jaywayco.Dust.Actors.Messages;

namespace jaywayco.Dust.Actors.Actors
{
	public class AssemblyActor : BaseTypedActor, IHandle<ImplementationsOfTypeRequest>
	{
		public void Handle(ImplementationsOfTypeRequest message)
		{
			try
			{
				var types = message.Assembly
									.GetTypes()
									.Where(type => type.GetInterfaces()
													.Any(type1 => type1.FullName == message.InterfaceType.FullName)
										);
				Sender.Tell(new ImplementationsOfTypeResponse(types), Self);
			}
			catch(Exception ex)
			{
				Logger.Error(ex, "Exception happened!!");
				Sender.Tell(new Failure() { Exception = ex, Timestamp = DateTime.UtcNow }, Self);
			}
		}
	}
}
