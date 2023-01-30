using System;
using System.Threading;
using System.Collections.Generic;

using UniRx;

using HereticalSolutions.Collections.Managed;

namespace HereticalSolutions.Messaging.UNFINISHED
{
	/*
	public class ConcurrentMessageBus
		: IMessageSubscribable,
		  IMessageSendable,
		  IMessageReceivable
	{
		private IMessageBroker broker;

		private Dictionary<Type, StackPool<IMessage>> messageRepository; //TODO: check whether this one should be concurrent too

		private ConcurrentQueue<IMessage> mailbox;

		public bool MailboxNotEmpty
		{
			get
			{
				return mailbox.Count != 0;
			}
		}

		private object lockObject;

		public MessageBus(
			IMessageBroker broker,
			Dictionary<Type, StackPool<IMessage>> messageRepository,
			ConcurrentQueue<IMessage> mailbox,
			object lockObject)
		{
			this.broker = broker;

			this.messageRepository = messageRepository;

			this.mailbox = mailbox;

			this.lockObject = lockObject;
		}

		public IDisposable SubscribeTo<TMessage>(Action<TMessage> receiverDelegate) where TMessage : IMessage
		{
			return broker
				.Receive<TMessage>()
				.Subscribe(message => receiverDelegate(message));
			//.AddTo(disposables);
		}

		public MessageBus PopMessage(Type messageType, out IMessage message)
		{
			if (!messageRepository.TryGetValue(
				messageType,
				out StackPool<IMessage> messagePool))
				throw new Exception($"[MessageBus] INVALID MESSAGE TYPE FOR PARTICULAR MESSAGE BUS: {messageType.ToString()}");

			lock (lockObject)
			{
				message = messagePool.Pop();
			}

			return this;
		}

		public MessageBus PopMessage<TMessage>(out TMessage message) where TMessage : IMessage
		{
			if (!messageRepository.TryGetValue(
				typeof(TMessage),
				out StackPool<IMessage> messagePool))
				throw new Exception($"[MessageBus] INVALID MESSAGE TYPE FOR PARTICULAR MESSAGE BUS: {typeof(TMessage).ToString()}");

			message = (TMessage)messagePool.Pop();

			return this;
		}

		public void Send(IMessage message)
		{
			mailbox.Enqueue(message);
		}

		public void Send<TMessage>(TMessage message) where TMessage : IMessage
		{
			mailbox.Enqueue(message);
		}

		public void SendImmediately(IMessage message)
		{
			broker.Publish(message);

			PushMessage(message);
		}

		public void SendImmediately<TMessage>(TMessage message) where TMessage : IMessage
		{
			broker.Publish(message);

			PushMessage<TMessage>(message);
		}

		public void Receive(int maxAmount)
		{
			int messagesToReceive = Math.Min(maxAmount, mailbox.Count);

			for (int i = 0; i < messagesToReceive; i++)
			{
				var message = mailbox.Dequeue();

				broker.Publish(message);

				PushMessage(message);
			}
		}

		public void ReceiveAll()
		{
			int messagesToReceive = mailbox.Count;

			for (int i = 0; i < messagesToReceive; i++)
			{
				var message = mailbox.Dequeue();

				broker.Publish(message);

				PushMessage(message);
			}
		}

		private void PushMessage(IMessage message)
		{
			var messageType = message.GetType();

			if (!messageRepository.TryGetValue(
				messageType,
				out StackPool<IMessage> messagePool))
				throw new Exception($"[MessageBus] INVALID MESSAGE TYPE FOR PARTICULAR MESSAGE BUS: {messageType.ToString()}");

			messagePool.Push(message);
		}

		private void PushMessage<TMessage>(TMessage message) where TMessage : IMessage
		{
			if (!messageRepository.TryGetValue(
				typeof(TMessage),
				out StackPool<IMessage> messagePool))
				throw new Exception($"[MessageBus] INVALID MESSAGE TYPE FOR PARTICULAR MESSAGE BUS: {typeof(TMessage).ToString()}");

			messagePool.Push(message);
		}
	}
	*/
}