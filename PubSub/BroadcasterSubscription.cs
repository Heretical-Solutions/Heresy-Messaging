using System;

namespace HereticalSolutions.Messaging
{
	public class BroadcasterSubscription<T>
	{
		private Action<T> @delegate;

		public BroadcasterSubscription(Action<T> @delegate)
		{
			this.@delegate = @delegate;
		}

		public void Handle(T value)
		{
			@delegate?.Invoke(value);
		}
	}
}