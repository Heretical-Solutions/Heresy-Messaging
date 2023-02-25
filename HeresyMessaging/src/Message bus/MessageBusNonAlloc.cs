using System;
using System.Collections.Generic;
using HereticalSolutions.Pools;
using HereticalSolutions.Repositories;

namespace HereticalSolutions.Messaging
{
    //TODO:
    //Right now message bus is in broadcast mode
    //To send messages to particular recepients (i.e. how Defold does it) I plan on adding the following:
    //1. IMessage shall contain a 'header' with EMessageDestination enum (UNICAST, BROADCAST) and a recepient address string
    //2. Message buses shall additionally contain a pool of IMessageBroker's for kinda 'personal mailboxes'
    //3. To resolve recepients on addressed messages I shall add a trie (prefix tree) with early return option
    //   i.e. if address is "Entities/Entity 12" and there is only one node behind starting E then it performs full string comparison with the node and sends it the message
    public class MessageBusNonAlloc
        : IMessageSenderNonAlloc, 
	      IMessageReceiver
    {
        private IBroadcastable<IMessage> broadcaster;

        private IRepository<Type, INonAllocPool<IMessage>> messageRepository;

        private Queue<IPoolElement<IMessage>> mailbox;

        public MessageBusNonAlloc(
	        IBroadcastable<IMessage> broadcaster,
			IRepository<Type, INonAllocPool<IMessage>> messageRepository,
			Queue<IPoolElement<IMessage>> mailbox)
        {
            this.broadcaster = broadcaster;

            this.messageRepository = messageRepository;

            this.mailbox = mailbox;
        }

        public IPoolElement<BroadcastHandler<TMessage>> SubscribeTo<TMessage>(Action<TMessage> receiverDelegate) where TMessage : IMessage
        {
	        var subscription = 
	        
	        var subscriptionPoolElement = broadcaster.Subscribe()
	        
			return broadcaster
                .Receive<TMessage>()
                .Subscribe(message => receiverDelegate(message));
				//.AddTo(disposables);
        }

        public void UnsubscribeFrom()
        {
	        
        }

        public IMessageSenderNonAlloc PopMessage(Type messageType, out IPoolElement<IMessage> message)
		{
			if (!messageRepository.TryGet(
				messageType,
				out INonAllocPool<IMessage> messagePool))
				throw new Exception($"[MessageBus] INVALID MESSAGE TYPE FOR PARTICULAR MESSAGE BUS: {messageType.ToString()}");

			message = messagePool.Pop();

			return this;
		}

        public IMessageSenderNonAlloc PopMessage<TMessage>(out IPoolElement<TMessage> message) where TMessage : IMessage
        {
            if (!messageRepository.TryGet(
                typeof(TMessage),
                out INonAllocPool<IMessage> messagePool))
                throw new Exception($"[MessageBus] INVALID MESSAGE TYPE FOR PARTICULAR MESSAGE BUS: {typeof(TMessage).ToString()}");

            message = (IPoolElement<TMessage>)messagePool.Pop();

            return this;
        }

		//public IMessageSenderNonAlloc Write(IPoolElement<IMessage> message, params object[] args)
		public IMessageSenderNonAlloc Write(IPoolElement<IMessage> message, object[] args)
		{
			if (message == null)
				throw new Exception($"[MessageBus] INVALID MESSAGE");

			message.Value.Write(args);

			return this;
		}

        public void Send(IPoolElement<IMessage> message)
        {
            mailbox.Enqueue(message);
        }

		public void Send<TMessage>(IPoolElement<TMessage> message) where TMessage : IMessage
		{
			mailbox.Enqueue(message);
		}

        public void SendImmediately(IPoolElement<IMessage> message)
        {
            broadcaster.Broadcast(message.Value);

            PushMessage(message);
        }

		public void SendImmediately<TMessage>(IPoolElement<TMessage> message) where TMessage : IMessage
		{
			broadcaster.Broadcast(message.Value);

            PushMessage<TMessage>(message);
		}

		public void DeliverMessagesInMailbox()
		{
			int messagesToReceive = mailbox.Count;

			for (int i = 0; i < messagesToReceive; i++)
			{
				var message = mailbox.Dequeue();

				broadcaster.Broadcast(message);

				PushMessage(message);
			}
		}

        private void PushMessage(IPoolElement<IMessage> message)
        {
            var messageType = message.GetType();

			if (!messageRepository.TryGet(
				messageType,
				out INonAllocPool<IMessage> messagePool))
				throw new Exception($"[MessageBus] INVALID MESSAGE TYPE FOR PARTICULAR MESSAGE BUS: {messageType.ToString()}");

			messagePool.Push(message);
        }

		private void PushMessage<TMessage>(IPoolElement<TMessage> message) where TMessage : IMessage
		{
			if (!messageRepository.TryGet(
				typeof(TMessage),
				out INonAllocPool<IMessage> messagePool))
				throw new Exception($"[MessageBus] INVALID MESSAGE TYPE FOR PARTICULAR MESSAGE BUS: {typeof(TMessage).ToString()}");

			messagePool.Push(message);
		}
    }
}