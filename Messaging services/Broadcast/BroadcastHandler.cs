using System;

namespace HereticalSolutions.Messaging
{
	public class BroadcastHandler<T>
	{
		private Action<T> @delegate;

		public BroadcastHandler(Action<T> @delegate)
		{
			this.@delegate = @delegate;
		}

		public void Handle(T value)
		{
			@delegate?.Invoke(value);
		}
	}
}