using System;

namespace HereticalSolutions.Messaging
{
	public interface IMessageSubscribable
	{
		void SubscribeTo<TMessage>(Action<TMessage> receiverDelegate) where TMessage : IMessage;
	}
}