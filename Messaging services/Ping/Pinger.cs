using HereticalSolutions.Collections;
using HereticalSolutions.Pools;

namespace HereticalSolutions.Messaging.Pinging
{
	public class Pinger : IPingable
	{
		#region Subscriptions
		
		private INonAllocPool<PingHandler> subscriptionsPool;

		private IIndexable<IPoolElement<PingHandler>> subscriptionsAsIndexable;

		private IFixedSizeCollection<IPoolElement<PingHandler>> subscriptionsWithCapacity;

		#endregion
		
		#region Buffer

		private PingHandler[] currentSubscriptionsBuffer;

		private int currentSubscriptionsBufferCount = -1;

		#endregion
		
		private bool pingInProgress = false;

		public Pinger(
			INonAllocPool<PingHandler> subscriptionsPool,
			INonAllocPool<PingHandler> subscriptionsContents)
		{
			this.subscriptionsPool = subscriptionsPool;

			subscriptionsAsIndexable = (IIndexable<IPoolElement<PingHandler>>)subscriptionsContents;

			subscriptionsWithCapacity =
				(IFixedSizeCollection<IPoolElement<PingHandler>>)subscriptionsContents;

			currentSubscriptionsBuffer = new PingHandler[subscriptionsWithCapacity.Capacity];
		}

		#region IPoolSubscribable
		
		public IPoolElement<PingHandler> Subscribe(PingHandler handler)
		{
			var subscriptionElement = subscriptionsPool.Pop();

			subscriptionElement.Value = handler;

			return subscriptionElement;
		}

		public void Unsubscribe(IPoolElement<PingHandler> subscription)
		{
			TryUnsubscribeFromBuffer(subscription);
			
			subscription.Value = null;

			subscriptionsPool.Push(subscription);
		}

		private void TryUnsubscribeFromBuffer(IPoolElement<PingHandler> subscriptionElement)
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

			currentSubscriptionsBufferCount = subscriptionsAsIndexable.Count;

			CopySubscriptionsToBuffer();

			HandleSubscriptions();

			EmptyBuffer();
		}

		private void ValidateBufferSize()
		{
			if (currentSubscriptionsBuffer.Length < subscriptionsWithCapacity.Capacity)
				currentSubscriptionsBuffer = new PingHandler[subscriptionsWithCapacity.Capacity];
		}

		private void CopySubscriptionsToBuffer()
		{
			for (int i = 0; i < currentSubscriptionsBufferCount; i++)
				currentSubscriptionsBuffer[i] = subscriptionsAsIndexable[i].Value;
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