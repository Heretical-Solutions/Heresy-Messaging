using System;

using HereticalSolutions.Delegates;

namespace HereticalSolutions.Messaging
{
    public interface IMessageReceiverNonAlloc
    {
        void SubscribeTo<TMessage>(ISubscriptionHandler<INonAllocSubscribableSingleArgGeneric<TMessage>, IInvokableSingleArgGeneric<TMessage>> subscription) where TMessage : IMessage;
        
        void SubscribeTo(Type messageType, ISubscriptionHandler<INonAllocSubscribableSingleArg, IInvokableSingleArg> subscription);
		
        void UnsubscribeFrom<TMessage>(ISubscriptionHandler<INonAllocSubscribableSingleArgGeneric<TMessage>, IInvokableSingleArgGeneric<TMessage>> subscription) where TMessage : IMessage;
        
        void UnsubscribeFrom(Type messageType, ISubscriptionHandler<INonAllocSubscribableSingleArg, IInvokableSingleArg> subscription);
    }
}