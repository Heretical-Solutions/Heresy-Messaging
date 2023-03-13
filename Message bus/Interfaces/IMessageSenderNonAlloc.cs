using System;

using HereticalSolutions.Pools;

namespace HereticalSolutions.Messaging
{
    public interface IMessageSenderNonAlloc
    {
        #region Pop
        
        IMessageSenderNonAlloc PopMessage(Type messageType, out IPoolElement<IMessage> message);

        IMessageSenderNonAlloc PopMessage<TMessage>(out IPoolElement<IMessage> message) where TMessage : IMessage;
        
        #endregion

        #region Write
        
        IMessageSenderNonAlloc Write(IPoolElement<IMessage> message, object[] args);
        
        IMessageSenderNonAlloc Write<TMessage>(IPoolElement<IMessage> message, object[] args) where TMessage : IMessage;
        
        #endregion

        #region Send
        
        void Send(IPoolElement<IMessage> message);

        void Send<TMessage>(IPoolElement<IMessage> message) where TMessage : IMessage;

        void SendImmediately(IPoolElement<IMessage> message);

        void SendImmediately<TMessage>(IPoolElement<IMessage> message) where TMessage : IMessage;
        
        #endregion

        #region Deliver
        
        void DeliverMessagesInMailbox();
        
        #endregion
    }
}