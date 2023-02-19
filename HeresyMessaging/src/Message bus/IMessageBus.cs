using System;

namespace HereticalSolutions.Messaging
{
    public interface IMessageBus
    {
        IMessageBus PopMessage(Type messageType, out IMessage message);

        IMessageBus PopMessage<TMessage>(out TMessage message) where TMessage : IMessage;
        
        IMessageBus Write(IMessage message, params object[] args);
    }
}