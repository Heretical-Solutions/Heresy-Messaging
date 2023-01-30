using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Managed;

namespace HereticalSolutions.Messaging
{
	public class Broadcaster<TValue>
	{
		private INonAllocPool<BroadcasterSubscription<TValue>> subscriptionsPool;

		private IndexedPackedArray<BroadcasterSubscription<TValue>> subscriptionsArray;

		private BroadcasterSubscription<TValue>[] broadcastArray;

		private int broadcastCount = -1;

		private bool broadcastInProgress = false;

		public Broadcaster(
			INonAllocPool<BroadcasterSubscription<TValue>> subscriptionsPool,
			IndexedPackedArray<BroadcasterSubscription<TValue>> subscriptionsArray)
		{
			this.subscriptionsPool = subscriptionsPool;

			this.subscriptionsArray = subscriptionsArray;

			broadcastArray = new BroadcasterSubscription<TValue>[subscriptionsArray.Capacity];
		}

		public IPoolElement<BroadcasterSubscription<TValue>> Subscribe(BroadcasterSubscription<TValue> subscription)
		{
			var subscriptionElement = subscriptionsPool.Pop();

			subscriptionElement.Value = subscription;

			return subscriptionElement;
		}

		public void Unsubscribe(IPoolElement<BroadcasterSubscription<TValue>> subscriptionElement)
		{
			if (broadcastInProgress)
			{
				for (int i = 0; i < broadcastCount; i++)
					if (broadcastArray[i] == subscriptionElement.Value)
					{
						broadcastArray[i] = null;

						break;
					}
			}

			subscriptionElement.Value = null;

			subscriptionsPool.Push(subscriptionElement);
		}

		public void Broadcast(TValue value)
		{
			if (broadcastArray.Length < subscriptionsArray.Capacity)
				broadcastArray = new BroadcasterSubscription<TValue>[subscriptionsArray.Capacity];

			broadcastCount = subscriptionsArray.Count;

			for (int i = 0; i < broadcastCount; i++)
				broadcastArray[i] = subscriptionsArray[i].Value;

			broadcastInProgress = true;

			for (int i = 0; i < broadcastCount; i++)
			{
				if (broadcastArray[i] != null)
					broadcastArray[i].Handle(value);
			}

			broadcastInProgress = false;

			for (int i = 0; i < broadcastCount; i++)
				broadcastArray[i] = null;
		}
	}
}