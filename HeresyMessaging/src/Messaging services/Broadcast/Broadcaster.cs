using HereticalSolutions.Collections;
using HereticalSolutions.Pools;

namespace HereticalSolutions.Messaging.Broadcasting
{
	public class Broadcaster<TValue> : IBroadcastable<TValue>
	{
		#region Subscriptions
		
		private INonAllocPool<BroadcasterSubscription<TValue>> subscriptionsPool;

		private IIndexable<IPoolElement<BroadcasterSubscription<TValue>>> indexableSubscriptions;
		
		private IFixedSizeCollection<IPoolElement<BroadcasterSubscription<TValue>>> subscriptionsWithCapacity;

		#endregion
		
		#region Buffer
		
		private BroadcasterSubscription<TValue>[] currentSubscriptionsBuffer;

		private int currentSubscriptionsBufferCount = -1;

		#endregion
		
		private bool broadcastInProgress = false;

		public Broadcaster(
			INonAllocPool<BroadcasterSubscription<TValue>> subscriptionsPool,
			INonAllocPool<BroadcasterSubscription<TValue>> subscriptionsContents)
		{
			this.subscriptionsPool = subscriptionsPool;

			indexableSubscriptions = (IIndexable<IPoolElement<BroadcasterSubscription<TValue>>>)subscriptionsContents;

			subscriptionsWithCapacity =
				(IFixedSizeCollection<IPoolElement<BroadcasterSubscription<TValue>>>)subscriptionsContents;

			currentSubscriptionsBuffer = new BroadcasterSubscription<TValue>[subscriptionsWithCapacity.Capacity];
		}

		#region IPoolSubscribable
		
		public IPoolElement<BroadcasterSubscription<TValue>> Subscribe(BroadcasterSubscription<TValue> subscription)
		{
			var subscriptionElement = subscriptionsPool.Pop();

			subscriptionElement.Value = subscription;

			return subscriptionElement;
		}

		public void Unsubscribe(IPoolElement<BroadcasterSubscription<TValue>> subscriptionElement)
		{
			TryUnsubscribeFromBuffer(subscriptionElement);

			subscriptionElement.Value = null;

			subscriptionsPool.Push(subscriptionElement);
		}
		
		private void TryUnsubscribeFromBuffer(IPoolElement<BroadcasterSubscription<TValue>> subscriptionElement)
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

		#region IBroadcastable
		
		public void Broadcast(TValue value)
		{
			ValidateBufferSize();

			currentSubscriptionsBufferCount = indexableSubscriptions.Count;

			CopySubscriptionsToBuffer();

			HandleSubscriptions(value);

			EmptyBuffer();
		}
		
		private void ValidateBufferSize()
		{
			if (currentSubscriptionsBuffer.Length < subscriptionsWithCapacity.Capacity)
				currentSubscriptionsBuffer = new BroadcasterSubscription<TValue>[subscriptionsWithCapacity.Capacity];
		}

		private void CopySubscriptionsToBuffer()
		{
			for (int i = 0; i < currentSubscriptionsBufferCount; i++)
				currentSubscriptionsBuffer[i] = indexableSubscriptions[i].Value;
		}

		private void HandleSubscriptions(TValue value)
		{
			broadcastInProgress = true;

			for (int i = 0; i < currentSubscriptionsBufferCount; i++)
			{
				if (currentSubscriptionsBuffer[i] != null)
					currentSubscriptionsBuffer[i].Handle(value);
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