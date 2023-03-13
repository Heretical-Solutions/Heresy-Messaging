using System;

namespace HereticalSolutions.Delegates.Broadcasting
{
	public class BroadcasterGeneric<TValue>
		: IPublisherSingleArgGeneric<TValue>,
		  IPublisherSingleArg,
		  ISubscribableSingleArgGeneric<TValue>
	{
		private Action<TValue> multicastDelegate;
		
		#region IPublisherSingleArgGeneric

		public void Publish(TValue value)
		{
			multicastDelegate?.Invoke(value);
		}
		
		public void Publish(object value)
		{
			multicastDelegate?.Invoke((TValue)value);
		}
		
		public void Publish<TArgument>(TArgument value)
		{
			if (typeof(TArgument) != typeof(TValue))
				throw new Exception($"[BroadcasterGeneric] INVALID ARGUMENT TYPE. EXPECTED: \"{typeof(TValue).ToString()}\" RECEIVED: \"{typeof(TArgument).ToString()}\"");
			
			Publish((object)value); //It doesn't want to convert TArgument into TValue. Bastard
		}

		public void Publish(Type valueType, object value)
		{
			if (valueType != typeof(TValue))
				throw new Exception($"[BroadcasterGeneric] INVALID ARGUMENT TYPE. EXPECTED: \"{typeof(TValue).ToString()}\" RECEIVED: \"{valueType.ToString()}\"");
			
			Publish(value); //It doesn't want to convert TArgument into TValue. Bastard
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