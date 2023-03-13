using System;

namespace HereticalSolutions.Delegates.Broadcasting
{
	public class BroadcasterGeneric<TValue>
		: IPublisherSingleArgGeneric<TValue>,
		  IPublisherSingleArg,
		  ISubscribableSingleArgGeneric<TValue>,
		  ISubscribableSingleArg
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

		#endregion

		#region IPublisherSingleArg

		public void Publish<TArgument>(TArgument value)
		{
			if (!(typeof(TArgument).Equals(typeof(TValue))))
				throw new Exception($"[BroadcasterGeneric] INVALID ARGUMENT TYPE. EXPECTED: \"{typeof(TValue).ToString()}\" RECEIVED: \"{typeof(TArgument).ToString()}\"");
			
			Publish((object)value); //It doesn't want to convert TArgument into TValue. Bastard
		}

		public void Publish(Type valueType, object value)
		{
			if (!(valueType.Equals(typeof(TValue))))
				throw new Exception($"[BroadcasterGeneric] INVALID ARGUMENT TYPE. EXPECTED: \"{typeof(TValue).ToString()}\" RECEIVED: \"{valueType.ToString()}\"");
			
			Publish(value); //It doesn't want to convert TArgument into TValue. Bastard
		}

		#endregion

		#region ISubscribableSingleArgGeneric
		
		public void Subscribe(Action<TValue> @delegate)
		{
			multicastDelegate += @delegate;
		}

		public void Subscribe(object @delegate)
		{
			multicastDelegate += (Action<TValue>)@delegate;
		}

		public void Unsubscribe(Action<TValue> @delegate)
		{
			multicastDelegate -= @delegate;
		}
		
		public void Unsubscribe(object @delegate)
		{
			multicastDelegate -= (Action<TValue>)@delegate;
		}
		
		#endregion

		#region ISubscribableSingleArg

		public void Subscribe<TArgument>(Action<TArgument> @delegate)
		{
			if (!(typeof(TArgument).Equals(typeof(TValue))))
				throw new Exception($"[BroadcasterGeneric] INVALID ARGUMENT TYPE. EXPECTED: \"{typeof(TValue).ToString()}\" RECEIVED: \"{typeof(TArgument).ToString()}\"");

			//DIRTY HACKS DO NOT REPEAT
			object delegateObject = (object)@delegate;
			
			multicastDelegate += (Action<TValue>)delegateObject;
		}

		public void Subscribe(Type valueType, object @delegate)
		{
			if (!(valueType.Equals(typeof(TValue))))
				throw new Exception($"[BroadcasterGeneric] INVALID ARGUMENT TYPE. EXPECTED: \"{typeof(TValue).ToString()}\" RECEIVED: \"{valueType.ToString()}\"");

			//DIRTY HACKS DO NOT REPEAT
			object delegateObject = (object)@delegate;
			
			multicastDelegate += (Action<TValue>)delegateObject;
		}

		public void Unsubscribe<TArgument>(Action<TArgument> @delegate)
		{
			if (!(typeof(TArgument).Equals(typeof(TValue))))
				throw new Exception($"[BroadcasterGeneric] INVALID ARGUMENT TYPE. EXPECTED: \"{typeof(TValue).ToString()}\" RECEIVED: \"{typeof(TArgument).ToString()}\"");

			//DIRTY HACKS DO NOT REPEAT
			object delegateObject = (object)@delegate;
			
			multicastDelegate -= (Action<TValue>)delegateObject;
		}

		public void Unsubscribe(Type valueType, object @delegate)
		{
			if (!(valueType.Equals(typeof(TValue))))
				throw new Exception($"[BroadcasterGeneric] INVALID ARGUMENT TYPE. EXPECTED: \"{typeof(TValue).ToString()}\" RECEIVED: \"{valueType.ToString()}\"");

			//DIRTY HACKS DO NOT REPEAT
			object delegateObject = (object)@delegate;
			
			multicastDelegate -= (Action<TValue>)delegateObject;
		}

		#endregion
	}
}