using Akka.Cluster;

namespace Hoist.Host.Messages
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