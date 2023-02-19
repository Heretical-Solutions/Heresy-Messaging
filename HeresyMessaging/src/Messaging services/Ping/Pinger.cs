using System;
using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Managed;

namespace HereticalSolutions.Messaging.Pinging
{
	public class Pinger : IPingable
	{
		private INonAllocPool<PingerSubscription> subscriptionsPool;

		private IIndexable<IPoolElement<PingerSubscription>> subscriptionsArray;

		private IndexedPackedArray<PingerSubscription> subscriptionsArray;

		#region Buffer

		private PingerSubscription[] currentSubscriptionsBuffer;

		private int currentSubscriptionsBufferCount = -1;

		#endregion
		
		private bool pingInProgress = false;

		public Pinger(
			INonAllocPool<PingerSubscription> subscriptionsPool,
			IndexedPackedArray<PingerSubscription> subscriptionsArray)
		{
			this.subscriptionsPool = subscriptionsPool;

			this.subscriptionsArray = subscriptionsArray;

			currentSubscriptionsBuffer = new PingerSubscription[subscriptionsArray.Capacity];
		}

		public IPoolElement<PingerSubscription> Subscribe(PingerSubscription subscription)
		{
			var subscriptionElement = subscriptionsPool.Pop();

			subscriptionElement.Value = subscription;

			return subscriptionElement;
		}

		public void Unsubscribe(IPoolElement<PingerSubscription> subscriptionElement)
		{
			TryUnsubscribeFromBuffer(subscriptionElement);
			
			subscriptionElement.Value = null;

			subscriptionsPool.Push(subscriptionElement);
		}

		private void TryUnsubscribeFromBuffer(IPoolElement<PingerSubscription> subscriptionElement)
		{
			if (!pingInProgress)
				return;
				
			for (int i = 0; i < currentSubscriptionsBufferCount; i++)
				if (currentSubscriptionsBuffer[i] == subscriptionElement.Value)
				{
					currentSubscriptionsBuffer[i] = null;

					return;
				}
		}

		public void Ping()
		{
			ValidateBufferSize();

			currentSubscriptionsBufferCount = subscriptionsArray.Count;

			CopySubscriptionsToBuffer();

			HandleSubscriptions();

			EmptyBuffer();
		}

		private void ValidateBufferSize()
		{
			if (currentSubscriptionsBuffer.Length < subscriptionsArray.Capacity)
				currentSubscriptionsBuffer = new PingerSubscription[subscriptionsArray.Capacity];
		}

		private void CopySubscriptionsToBuffer()
		{
			for (int i = 0; i < currentSubscriptionsBufferCount; i++)
				currentSubscriptionsBuffer[i] = subscriptionsArray[i].Value;
		}

		private void HandleSubscriptions()
		{
			pingInProgress = true;

			for (int i = 0; i < currentSubscriptionsBufferCount; i++)
			{
				if (currentSubscriptionsBuffer[i] != null)
					currentSubscriptionsBuffer[i].Handle();
			}

			pingInProgress = false;
		}

		private void EmptyBuffer()
		{
			for (int i = 0; i < currentSubscriptionsBufferCount; i++)
				currentSubscriptionsBuffer[i] = null;
		}
	}
}