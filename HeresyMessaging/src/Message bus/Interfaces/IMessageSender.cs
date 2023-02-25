using System;

namespace HereticalSolutions.Messaging
{
    public interface IMessageSender
    {
        IMessageSender PopMessage(Type messageType, out IMessage message);

        IMessageSender PopMessage<TMessage>(out TMessage message) where TMessage : IMessage;
        
        //IMessageSender Write(IMessage message, params object[] args);
        IMessageSender Write(IMessage message, object[] args);
        
        void Send(IMessage message);

        void Send<TMessage>(TMessage message) where TMessage : IMessage;

        void SendImmediately(IMessage message);

        void SendImmediately<TMessage>(TMessage message) where TMessage : IMessage;
        
        void DeliverMessagesInMailbox();
    }
}