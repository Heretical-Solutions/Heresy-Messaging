using System;
using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Managed;

namespace HereticalSolutions.Messaging
{
	public class Broadcaster<TValue>
	{
		private INonAllocPool<BroadcasterSubscription<TValue>> subscriptionsPool;

		private IIndexable<IPoolElement<BroadcasterSubscription<TValue>>> subscriptionsArray;

		private bool modificationAllowed = true;

		public Broadcaster(
			INonAllocPool<BroadcasterSubscription<TValue>> subscriptionsPool,
			IIndexable<IPoolElement<BroadcasterSubscription<TValue>>> subscriptionsArray)
		{
			this.subscriptionsPool = subscriptionsPool;

			this.subscriptionsArray = subscriptionsArray;
		}

		public IPoolElement<BroadcasterSubscription<TValue>> Subscribe()
		{
			if (!modificationAllowed)
				throw new Exception("[Pinger] Subscriber collection modification not allowed while ping is in progress");

			var subscription = subscriptionsPool.Pop();

			subscription.Value = null;

			return subscription;
		}

		public void Unsubscribe(IPoolElement<BroadcasterSubscription<TValue>> subscription)
		{
			if (!modificationAllowed)
				throw new Exception("[Pinger] Subscriber collection modification not allowed while ping is in progress");

			subscription.Value = null;

			subscriptionsPool.Push(subscription);
		}

		public void Broadcast(TValue value)
		{
			modificationAllowed = false;

			for (int i = 0; i < subscriptionsArray.Count; i++)
			{
				subscriptionsArray[i].Value.Handle(value);
			}

			modificationAllowed = true;
		}
	}
}