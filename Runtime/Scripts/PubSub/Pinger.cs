using System;
using System.Collections.Generic;

using UniRx;

using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Managed;
using HereticalSolutions.Repositories;

namespace HereticalSolutions.Messaging
{
	public class Pinger
	{
		private INonAllocPool<Action> subscriptionsPool;

		private IIndexable<IPoolElement<Action>> subscriptionsArray;

		private bool modificationAllowed = true;

		public Pinger(
			INonAllocPool<Action> subscriptionsPool,
			IIndexable<IPoolElement<Action>> subscriptionsArray)
		{
			this.subscriptionsPool = subscriptionsPool;

			this.subscriptionsArray = subscriptionsArray;
		}

		public IPoolElement<Action> Subscribe()
		{
			if (!modificationAllowed)
				throw new Exception("[Pinger] Subscriber collection modification not allowed while ping is in progress");

			var subscription = subscriptionsPool.Pop();

			subscription.Value = null;

			return subscription;
		}

		public void Unsubscribe(IPoolElement<Action> subscription)
		{
			if (!modificationAllowed)
				throw new Exception("[Pinger] Subscriber collection modification not allowed while ping is in progress");

			subscription.Value = null;
		}

		public void Ping()
		{
			modificationAllowed = false;

			for (int i = 0; i < subscriptionsArray.Count; i++)
			{
				subscriptionsArray[i].Value.Invoke();
			}

			modificationAllowed = true;
		}
	}
}