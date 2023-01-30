using System;

namespace HereticalSolutions.Messaging
{
	public interface IMessageSubscribable
	{
		IDisposable SubscribeTo<TMessage>(Action<TMessage> receiverDelegate) where TMessage : IMessage;
	}
}