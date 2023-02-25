using HereticalSolutions.Collections;
using HereticalSolutions.Pools;

namespace HereticalSolutions.Messaging.Pinging
{
	public class Pinger : IPingable
	{
		#region Subscriptions
		
		private INonAllocPool<PingerSubscription> subscriptionsPool;

		private IIndexable<IPoolElement<PingerSubscription>> indexableSubscriptions;

		private IFixedSizeCollection<IPoolElement<PingerSubscription>> subscriptionsWithCapacity;

		#endregion
		
		#region Buffer

		private PingerSubscription[] currentSubscriptionsBuffer;

		private int currentSubscriptionsBufferCount = -1;

		#endregion
		
		private bool pingInProgress = false;

		public Pinger(
			INonAllocPool<PingerSubscription> subscriptionsPool,
			INonAllocPool<PingerSubscription> subscriptionsContents)
		{
			this.subscriptionsPool = subscriptionsPool;

			indexableSubscriptions = (IIndexable<IPoolElement<PingerSubscription>>)subscriptionsContents;

			subscriptionsWithCapacity =
				(IFixedSizeCollection<IPoolElement<PingerSubscription>>)subscriptionsContents;

			currentSubscriptionsBuffer = new PingerSubscription[subscriptionsWithCapacity.Capacity];
		}

		#region IPoolSubscribable
		
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
		
		#endregion

		#region IPingable
		
		public void Ping()
		{
			ValidateBufferSize();

			currentSubscriptionsBufferCount = indexableSubscriptions.Count;

			CopySubscriptionsToBuffer();

			HandleSubscriptions();

			EmptyBuffer();
		}

		private void ValidateBufferSize()
		{
			if (currentSubscriptionsBuffer.Length < subscriptionsWithCapacity.Capacity)
				currentSubscriptionsBuffer = new PingerSubscription[subscriptionsWithCapacity.Capacity];
		}

		private void CopySubscriptionsToBuffer()
		{
			for (int i = 0; i < currentSubscriptionsBufferCount; i++)
				currentSubscriptionsBuffer[i] = indexableSubscriptions[i].Value;
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
		
		#endregion
	}
}