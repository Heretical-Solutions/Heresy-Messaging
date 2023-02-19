using System;

namespace HereticalSolutions.Messaging
{
	public interface IMessageSendable
	{
		void Send(IMessage message);

		void Send<TMessage>(TMessage message) where TMessage : IMessage;

		void SendImmediately(IMessage message);

		void SendImmediately<TMessage>(TMessage message) where TMessage : IMessage;
	}
}