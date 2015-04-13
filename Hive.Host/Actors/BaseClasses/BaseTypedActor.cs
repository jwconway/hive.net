using Akka.Actor;
using Akka.Event;

namespace Hive.Host.Actors.BaseClasses
{
	/// <summary>
	/// Just so weve always got the logger handy
	/// </summary>
	public abstract class BaseTypedActor : TypedActor
	{
		protected readonly ILoggingAdapter Logger = Logging.GetLogger(Context);
	}
}
