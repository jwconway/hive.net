using Akka.Actor;
using Akka.Event;

namespace Hoist.Host.Actors
{
	/// <summary>
	/// just so weve always got the logger handy
	/// </summary>
	public abstract class BaseUntypedActor : UntypedActor
	{
		protected readonly LoggingAdapter Logger = Logging.GetLogger(Context);
	}
}
