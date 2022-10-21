using System;
using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Managed;

namespace HereticalSolutions.Messaging
{
	public class Pinger
	{
		private INonAllocPool<PingerSubscription> subscriptionsPool;

		private IndexedPackedArray<PingerSubscription> subscriptionsArray;

		private PingerSubscription[] pingArray;

		private int pingCount = -1;

		private bool pingInProgress = false;

		public Pinger(
			INonAllocPool<PingerSubscription> subscriptionsPool,
			IndexedPackedArray<PingerSubscription> subscriptionsArray)
		{
			this.subscriptionsPool = subscriptionsPool;

			this.subscriptionsArray = subscriptionsArray;

			pingArray = new PingerSubscription[subscriptionsArray.Capacity];
		}

		public IPoolElement<PingerSubscription> Subscribe()
		{
			var subscription = subscriptionsPool.Pop();

			subscription.Value = null;

			return subscription;
		}

		public void Unsubscribe(IPoolElement<PingerSubscription> subscription)
		{
			if (pingInProgress)
			{
				for (int i = 0; i < pingCount; i++)
					if (pingArray[i] == subscription.Value)
					{
						pingArray[i] = null;

						break;
					}
			}

			subscription.Value = null;

			subscriptionsPool.Push(subscription);
		}

		public void Ping()
		{
			if (pingArray.Length < subscriptionsArray.Capacity)
				pingArray = new PingerSubscription[subscriptionsArray.Capacity];

			pingCount = subscriptionsArray.Count;

			for (int i = 0; i < pingCount; i++)
				pingArray[i] = subscriptionsArray[i].Value;

			pingInProgress = true;

			for (int i = 0; i < pingCount; i++)
			{
				if (pingArray[i] != null)
					pingArray[i].Handle();
			}

			pingInProgress = false;

			for (int i = 0; i < pingCount; i++)
				pingArray[i] = null;
		}
	}
}