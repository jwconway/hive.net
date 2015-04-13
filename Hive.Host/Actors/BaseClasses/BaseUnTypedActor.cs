using Akka.Actor;
using Akka.Event;

namespace Hive.Host.Actors.BaseClasses
{
	/// <summary>
	/// just so weve always got the logger handy
	/// </summary>
	public abstract class BaseUntypedActor : UntypedActor
	{
		protected readonly ILoggingAdapter Logger = Logging.GetLogger(Context);
	}
}
