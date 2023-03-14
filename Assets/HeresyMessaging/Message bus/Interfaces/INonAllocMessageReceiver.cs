using System;

using HereticalSolutions.Delegates;

namespace HereticalSolutions.Messaging
{
    public interface INonAllocMessageReceiver
    {
        void SubscribeTo<TMessage>(ISubscription<INonAllocSubscribableSingleArgGeneric<TMessage>> subscription) where TMessage : IMessage;
        
        void SubscribeTo<TMessage>(ISubscription<INonAllocSubscribableSingleArg> subscription);
        
        void SubscribeTo(Type messageType, ISubscription<INonAllocSubscribableSingleArg> subscription);
		
        void UnsubscribeFrom<TMessage>(ISubscription<INonAllocSubscribableSingleArgGeneric<TMessage>> subscription) where TMessage : IMessage;
        
        void UnsubscribeFrom<TMessage>(ISubscription<INonAllocSubscribableSingleArg> subscription);
        
        void UnsubscribeFrom(Type messageType, ISubscription<INonAllocSubscribableSingleArg> subscription);
    }
}