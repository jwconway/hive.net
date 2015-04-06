using Akka.Cluster;

namespace Hive.Host.Messages
{
	public class RegisterMessage
	{
		public RegisterMessage(Member member)
		{
			Member = member;
		}

		public Member Member { get; private set; }
	}
}