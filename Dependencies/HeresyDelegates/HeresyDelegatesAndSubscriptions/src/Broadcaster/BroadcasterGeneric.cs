using System;

namespace HereticalSolutions.Delegates.Broadcasting
{
	public class BroadcasterGeneric<TValue>
		: IPublisherSingleArgGeneric<TValue>,
		  ISubscribableSingleArgGeneric<TValue>
	{
		private Action<TValue> multicastDelegate;
		
		#region IPublisherSingleArgGeneric

		public void Publish(TValue value)
		{
			multicastDelegate?.Invoke(value);
		}

		#endregion

		#region ISubscribableSingleArgGeneric
		
		public void Subscribe(Action<TValue> @delegate)
		{
			multicastDelegate += @delegate;
		}

		public void Unsubscribe(Action<TValue> @delegate)
		{
			multicastDelegate -= @delegate;
		}
		
		#endregion
	}
}