using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;

namespace jaywayco.Dust.Actors.Actors
{
	public abstract class BaseTypedActor : TypedActor
	{
		protected readonly LoggingAdapter Logger = Logging.GetLogger(Context);
	}
}
