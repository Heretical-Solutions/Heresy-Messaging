using System;
using HereticalSolutions.Pools;

namespace HereticalSolutions.Messaging
{
    public interface IMessageSenderNonAlloc
    {
        IMessageSenderNonAlloc PopMessage(Type messageType, out IPoolElement<IMessage> message);

        IMessageSenderNonAlloc PopMessage<TMessage>(out IPoolElement<TMessage> message) where TMessage : IMessage;
        
        IMessageSenderNonAlloc Write(IPoolElement<IMessage> message, params object[] args);
        
        void Send(IPoolElement<IMessage> message);

        void Send<TMessage>(IPoolElement<TMessage> message) where TMessage : IMessage;

        void SendImmediately(IPoolElement<IMessage> message);

        void SendImmediately<TMessage>(IPoolElement<TMessage> message) where TMessage : IMessage;
        
        void DeliverMessagesInMailbox();
    }
}