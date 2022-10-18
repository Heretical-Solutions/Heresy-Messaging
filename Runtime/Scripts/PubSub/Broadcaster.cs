using System;
using System.Collections.Generic;

using UniRx;

using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Managed;
using HereticalSolutions.Repositories;

namespace HereticalSolutions.Messaging
{
	public class Broadcaster<TValue>
	{
		private INonAllocPool<Action<TValue>> subscriptionsPool;

		private IIndexable<IPoolElement<Action<TValue>>> subscriptionsArray;

		private bool modificationAllowed = true;

		public Broadcaster(
			INonAllocPool<Action<TValue>> subscriptionsPool,
			IIndexable<IPoolElement<Action<TValue>>> subscriptionsArray)
		{
			this.subscriptionsPool = subscriptionsPool;

			this.subscriptionsArray = subscriptionsArray;
		}

		public IPoolElement<Action<TValue>> Subscribe()
		{
			if (!modificationAllowed)
				throw new Exception("[Pinger] Subscriber collection modification not allowed while ping is in progress");

			var subscription = subscriptionsPool.Pop();

			subscription.Value = null;

			return subscription;
		}

		public void Unsubscribe(IPoolElement<Action<TValue>> subscription)
		{
			if (!modificationAllowed)
				throw new Exception("[Pinger] Subscriber collection modification not allowed while ping is in progress");

			subscription.Value = null;
		}

		public void Broadcast(TValue value)
		{
			modificationAllowed = false;

			for (int i = 0; i < subscriptionsArray.Count; i++)
			{
				subscriptionsArray[i].Value.Invoke(value);
			}

			modificationAllowed = true;
		}
	}
}