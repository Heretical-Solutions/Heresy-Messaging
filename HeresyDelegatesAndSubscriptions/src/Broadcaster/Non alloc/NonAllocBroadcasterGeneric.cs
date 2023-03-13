using System;
using HereticalSolutions.Collections;
using HereticalSolutions.Pools;

namespace HereticalSolutions.Delegates.Broadcasting
{
	public class NonAllocBroadcasterGeneric<TValue>
		: IPublisherSingleArgGeneric<TValue>,
		  IPublisherSingleArg,
		  INonAllocSubscribableSingleArgGeneric<TValue>
	{
		#region Subscriptions
		
		private readonly INonAllocDecoratedPool<IInvokableSingleArgGeneric<TValue>> subscriptionsPool;

		private readonly IIndexable<IPoolElement<IInvokableSingleArgGeneric<TValue>>> subscriptionsAsIndexable;

		private readonly IFixedSizeCollection<IPoolElement<IInvokableSingleArgGeneric<TValue>>> subscriptionsWithCapacity;

		#endregion
		
		#region Buffer

		private IInvokableSingleArgGeneric<TValue>[] currentSubscriptionsBuffer;

		private int currentSubscriptionsBufferCount = -1;

		#endregion
		
		private bool broadcastInProgress = false;

		public NonAllocBroadcasterGeneric(
			INonAllocDecoratedPool<IInvokableSingleArgGeneric<TValue>> subscriptionsPool,
			INonAllocPool<IInvokableSingleArgGeneric<TValue>> subscriptionsContents)
		{
			this.subscriptionsPool = subscriptionsPool;

			subscriptionsAsIndexable = (IIndexable<IPoolElement<IInvokableSingleArgGeneric<TValue>>>)subscriptionsContents;

			subscriptionsWithCapacity =
				(IFixedSizeCollection<IPoolElement<IInvokableSingleArgGeneric<TValue>>>)subscriptionsContents;

			currentSubscriptionsBuffer = new IInvokableSingleArgGeneric<TValue>[subscriptionsWithCapacity.Capacity];
		}

		#region INonAllocSubscribableSingleArgGeneric
		
		public void Subscribe(ISubscriptionHandler<INonAllocSubscribableSingleArgGeneric<TValue>, IInvokableSingleArgGeneric<TValue>> subscription)
		{
			#region Validate
			
			if (subscription.Active)
				throw new Exception("[NonAllocBroadcasterGeneric] ATTEMPT TO ACTIVATE A SUBSCRIPTION THAT IS ALREADY ACTIVE");
			
			if (subscription.Publisher != null)
				throw new Exception("[NonAllocBroadcasterGeneric] SUBSCRIPTION ALREADY HAS A PUBLISHER");
			
			if (subscription.PoolElement != null)
				throw new Exception("[NonAllocBroadcasterGeneric] SUBSCRIPTION ALREADY HAS A POOL ELEMENT");
			
			if (subscription.Delegate == null)
				throw new Exception("[NonAllocBroadcasterGeneric] INVALID DELEGATE");
			
			#endregion
			
			var subscriptionElement = subscriptionsPool.Pop(null);

			subscriptionElement.Value = subscription.Delegate;

			subscription.Activate(this, subscriptionElement);
		}

		public void Unsubscribe(ISubscriptionHandler<INonAllocSubscribableSingleArgGeneric<TValue>, IInvokableSingleArgGeneric<TValue>> subscription)
		{
			#region Validate
			
			if (!subscription.Active)
				throw new Exception("[NonAllocBroadcasterGeneric] ATTEMPT TO TERMINATE A SUBSCRIPTION THAT IS ALREADY ACTIVE");
			
			if (subscription.Publisher != this)
				throw new Exception("[NonAllocBroadcasterGeneric] INVALID PUBLISHER");
			
			if (subscription.PoolElement == null)
				throw new Exception("[NonAllocBroadcasterGeneric] INVALID POOL ELEMENT");
			
			#endregion

			var poolElement = subscription.PoolElement;
			
			TryRemoveFromBuffer(poolElement);
			
			poolElement.Value = null;

			subscriptionsPool.Push(poolElement);
			
			subscription.Terminate();
		}

		private void TryRemoveFromBuffer(IPoolElement<IInvokableSingleArgGeneric<TValue>> subscriptionElement)
		{
			if (!broadcastInProgress)
				return;
				
			for (int i = 0; i < currentSubscriptionsBufferCount; i++)
				if (currentSubscriptionsBuffer[i] == subscriptionElement.Value)
				{
					currentSubscriptionsBuffer[i] = null;

					return;
				}
		}
		
		#endregion

		#region IPublisherSingleArgGeneric
		
		public void Publish(TValue value)
		{
			ValidateBufferSize();

			currentSubscriptionsBufferCount = subscriptionsAsIndexable.Count;

			CopySubscriptionsToBuffer();

			InvokeSubscriptions(value);

			EmptyBuffer();
		}

		public void Publish(object value)
		{
			Publish((TValue)value);
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

		private void ValidateBufferSize()
		{
			if (currentSubscriptionsBuffer.Length < subscriptionsWithCapacity.Capacity)
				currentSubscriptionsBuffer = new IInvokableSingleArgGeneric<TValue>[subscriptionsWithCapacity.Capacity];
		}

		private void CopySubscriptionsToBuffer()
		{
			for (int i = 0; i < currentSubscriptionsBufferCount; i++)
				currentSubscriptionsBuffer[i] = subscriptionsAsIndexable[i].Value;
		}

		private void InvokeSubscriptions(TValue value)
		{
			broadcastInProgress = true;

			for (int i = 0; i < currentSubscriptionsBufferCount; i++)
			{
				if (currentSubscriptionsBuffer[i] != null)
					currentSubscriptionsBuffer[i].Invoke(value);
			}

			broadcastInProgress = false;
		}

		private void EmptyBuffer()
		{
			for (int i = 0; i < currentSubscriptionsBufferCount; i++)
				currentSubscriptionsBuffer[i] = null;
		}
		
		#endregion
	}
}