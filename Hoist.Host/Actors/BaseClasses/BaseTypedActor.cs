using Akka.Actor;
using Akka.Event;

namespace Hoist.Host.Actors.BaseClasses
{
	/// <summary>
	/// Just so weve always got the logger handy
	/// </summary>
	public abstract class BaseTypedActor : TypedActor
	{
		protected readonly LoggingAdapter Logger = Logging.GetLogger(Context);
	}
}
