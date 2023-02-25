using System;

using HereticalSolutions.Pools;

namespace HereticalSolutions.Messaging
{
	public interface IMessageReceiver
	{
		IPoolElement<BroadcastHandler<IMessage>> SubscribeTo(Type messageType, Action<IMessage> receiverDelegate);
		
		IPoolElement<BroadcastHandler<IMessage>> SubscribeTo<TMessage>(Action<IMessage> receiverDelegate) where TMessage : IMessage;

		IPoolElement<BroadcastHandler<IMessage>> SubscribeToNonAlloc(Type messageType, BroadcastHandler<IMessage> subscription);
		
		IPoolElement<BroadcastHandler<IMessage>> SubscribeToNonAlloc<TMessage>(BroadcastHandler<TMessage> subscription) where TMessage : IMessage;

		void UnsubscribeFrom<TMessage>(IPoolElement<BroadcastHandler<TMessage>> subscriptionPoolElement) where TMessage : IMessage;
	}
}