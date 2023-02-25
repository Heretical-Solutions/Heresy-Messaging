using System;

using HereticalSolutions.Pools;

namespace HereticalSolutions.Messaging
{
	public interface IMessageReceiver
	{
		IPoolElement<BroadcasterSubscription<IMessage>> SubscribeTo(Type messageType, Action<IMessage> receiverDelegate);
		
		IPoolElement<BroadcasterSubscription<TMessage>> SubscribeTo<TMessage>(Action<TMessage> receiverDelegate) where TMessage : IMessage;

		IPoolElement<BroadcasterSubscription<IMessage>> SubscribeToNonAlloc(Type messageType, BroadcasterSubscription<IMessage> subscription);
		
		IPoolElement<BroadcasterSubscription<TMessage>> SubscribeToNonAlloc<TMessage>(BroadcasterSubscription<TMessage> subscription) where TMessage : IMessage;

		void UnsubscribeFrom<TMessage>(IPoolElement<BroadcasterSubscription<TMessage>> subscriptionPoolElement) where TMessage : IMessage;
	}
}