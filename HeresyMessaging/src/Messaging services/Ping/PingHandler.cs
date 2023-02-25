using System;

namespace HereticalSolutions.Messaging
{
	public class PingHandler
	{
		private Action @delegate;

		public PingHandler(Action @delegate)
		{
			this.@delegate = @delegate;
		}

		public void Handle()
		{
			@delegate?.Invoke();
		}
	}
}