using System;

namespace HereticalSolutions.Messaging
{
	public class PingerSubscription
	{
		private Action @delegate;

		public PingerSubscription(Action @delegate)
		{
			this.@delegate = @delegate;
		}

		public void Handle()
		{
			@delegate?.Invoke();
		}
	}
}