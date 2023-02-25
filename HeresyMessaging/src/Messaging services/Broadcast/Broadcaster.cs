using HereticalSolutions.Collections;
using HereticalSolutions.Pools;

namespace HereticalSolutions.Messaging.Broadcasting
{
	public class Broadcaster<TValue> : IBroadcastable<TValue>
	{
		#region Subscriptions
		
		private INonAllocPool<BroadcastHandler<TValue>> subscriptionsPool;

		private IIndexable<IPoolElement<BroadcastHandler<TValue>>> subscriptionsAsIndexable;
		
		private IFixedSizeCollection<IPoolElement<BroadcastHandler<TValue>>> subscriptionsWithCapacity;

		#endregion
		
		#region Buffer
		
		private BroadcastHandler<TValue>[] currentSubscriptionsBuffer;

		private int currentSubscriptionsBufferCount = -1;

		#endregion
		
		private bool broadcastInProgress = false;

		public Broadcaster(
			INonAllocPool<BroadcastHandler<TValue>> subscriptionsPool,
			INonAllocPool<BroadcastHandler<TValue>> subscriptionsContents)
		{
			this.subscriptionsPool = subscriptionsPool;

			subscriptionsAsIndexable = (IIndexable<IPoolElement<BroadcastHandler<TValue>>>)subscriptionsContents;

			subscriptionsWithCapacity =
				(IFixedSizeCollection<IPoolElement<BroadcastHandler<TValue>>>)subscriptionsContents;

			currentSubscriptionsBuffer = new BroadcastHandler<TValue>[subscriptionsWithCapacity.Capacity];
		}

		#region IPoolSubscribable
		
		public IPoolElement<BroadcastHandler<TValue>> Subscribe(BroadcastHandler<TValue> handler)
		{
			var subscriptionElement = subscriptionsPool.Pop();

			subscriptionElement.Value = handler;

			return subscriptionElement;
		}

		public void Unsubscribe(IPoolElement<BroadcastHandler<TValue>> subscription)
		{
			TryUnsubscribeFromBuffer(subscription);

			subscription.Value = null;

			subscriptionsPool.Push(subscription);
		}
		
		private void TryUnsubscribeFromBuffer(IPoolElement<BroadcastHandler<TValue>> subscriptionElement)
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

			currentSubscriptionsBufferCount = subscriptionsAsIndexable.Count;

			CopySubscriptionsToBuffer();

			HandleSubscriptions(value);

			EmptyBuffer();
		}
		
		private void ValidateBufferSize()
		{
			if (currentSubscriptionsBuffer.Length < subscriptionsWithCapacity.Capacity)
				currentSubscriptionsBuffer = new BroadcastHandler<TValue>[subscriptionsWithCapacity.Capacity];
		}

		private void CopySubscriptionsToBuffer()
		{
			for (int i = 0; i < currentSubscriptionsBufferCount; i++)
				currentSubscriptionsBuffer[i] = subscriptionsAsIndexable[i].Value;
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