using System;
using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Managed;

namespace HereticalSolutions.Messaging
{
	public class Pinger
	{
		private INonAllocPool<PingerSubscription> subscriptionsPool;

		private IIndexable<IPoolElement<PingerSubscription>> subscriptionsArray;

		private bool modificationAllowed = true;

		public Pinger(
			INonAllocPool<PingerSubscription> subscriptionsPool,
			IIndexable<IPoolElement<PingerSubscription>> subscriptionsArray)
		{
			this.subscriptionsPool = subscriptionsPool;

			this.subscriptionsArray = subscriptionsArray;
		}

		public IPoolElement<PingerSubscription> Subscribe()
		{
			if (!modificationAllowed)
				throw new Exception("[Pinger] Subscriber collection modification not allowed while ping is in progress");

			var subscription = subscriptionsPool.Pop();

			subscription.Value = null;

			return subscription;
		}

		public void Unsubscribe(IPoolElement<PingerSubscription> subscription)
		{
			if (!modificationAllowed)
				throw new Exception("[Pinger] Subscriber collection modification not allowed while ping is in progress");

			subscription.Value = null;

			subscriptionsPool.Push(subscription);
		}

		public void Ping()
		{
			modificationAllowed = false;

			for (int i = 0; i < subscriptionsArray.Count; i++)
			{
				subscriptionsArray[i].Value.Handle();
			}

			modificationAllowed = true;
		}
	}
}