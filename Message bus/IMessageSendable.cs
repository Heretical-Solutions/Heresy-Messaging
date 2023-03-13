using System;
using UniRx;

namespace HereticalSolutions.Messaging
{
	public interface IMessageSendable
	{
		MessageBus PopMessage(Type messageType, out IMessage message);

		MessageBus PopMessage<TMessage>(out TMessage message) where TMessage : IMessage;

		void Send(IMessage message);

		void Send<TMessage>(TMessage message) where TMessage : IMessage;

		void SendImmediately(IMessage message);

		void SendImmediately<TMessage>(TMessage message) where TMessage : IMessage;

		
	}
}