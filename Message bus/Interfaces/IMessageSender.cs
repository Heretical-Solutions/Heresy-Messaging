using System;

namespace HereticalSolutions.Messaging
{
    public interface IMessageSender
    {
        #region Pop
        
        IMessageSender PopMessage(Type messageType, out IMessage message);

        IMessageSender PopMessage<TMessage>(out IMessage message) where TMessage : IMessage;
        
        #endregion

        #region Write
        
        IMessageSender Write(IMessage message, object[] args);
        //IMessageSender Write(IMessage message, params object[] args);
        
        #endregion

        #region Send
        
        void Send(IMessage message);

        //void Send<TMessage>(IMessage message) where TMessage : IMessage;

        void SendImmediately(IMessage message);

        //void SendImmediately<TMessage>(IMessage message) where TMessage : IMessage;
        
        #endregion

        #region Deliver
        
        void DeliverMessagesInMailbox();
        
        #endregion
    }
}