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
    public class MessageBus
        : IMessageSender, 
	      IMessageReceiver
    {
	    private IRepository<Type, IBroadcastable<IMessage>> broadcasterRepository;

        private IRepository<Type, IPool<IMessage>> messageRepository;

        private Queue<IMessage> mailbox;

        public MessageBus(
	        IRepository<Type, IBroadcastable<IMessage>> broadcasterRepository,
			IRepository<Type, IPool<IMessage>> messageRepository,
			Queue<IMessage> mailbox)
        {
            this.broadcasterRepository = broadcasterRepository;

            this.messageRepository = messageRepository;

            this.mailbox = mailbox;
        }

        #region IMessageSender

        public IMessageSender PopMessage(Type messageType, out IMessage message)
        {
	        if (!messageRepository.TryGet(
		            messageType,
		            out IPool<IMessage> messagePool))
		        throw new Exception($"[MessageBus] INVALID MESSAGE TYPE FOR PARTICULAR MESSAGE BUS: {messageType.ToString()}");

	        message = messagePool.Pop();

	        return this;
        }

        public IMessageSender PopMessage<TMessage>(out IMessage message) where TMessage : IMessage
        {
	        if (!messageRepository.TryGet(
		            typeof(TMessage),
		            out IPool<IMessage> messagePool))
		        throw new Exception($"[MessageBus] INVALID MESSAGE TYPE FOR PARTICULAR MESSAGE BUS: {typeof(TMessage).ToString()}");

	        message = messagePool.Pop();

	        return this;
        }

        IMessageSender IMessageSender.Write(IMessage message, object[] args)
        {
	        if (message == null)
		        throw new Exception($"[MessageBus] INVALID MESSAGE");

	        message.Write(args);

	        return this;
        }
        
        public void Send(IMessage message)
        {
	        mailbox.Enqueue(message);
        }

        /*
        public void Send<TMessage>(IMessage message) where TMessage : IMessage
        {
	        mailbox.Enqueue(message);
        }
		*/
        
        public void SendImmediately(IMessage message)
        {
	        BroadcastMessage(message);

	        PushMessageToPool(message);
        }

        /*
        public void SendImmediately<TMessage>(TMessage message) where TMessage : IMessage
        {
	        broadcaster.Broadcast(message);

	        PushMessage<TMessage>(message);
        }
        */
        
        public void DeliverMessagesInMailbox()
        {
	        int messagesToReceive = mailbox.Count;

	        for (int i = 0; i < messagesToReceive; i++)
	        {
		        var message = mailbox.Dequeue();

		        SendImmediately(message);
	        }
        }
        
        private void BroadcastMessage(IMessage message)
        {
	        var messageType = message.GetType();
	        
	        if (!broadcasterRepository.TryGet(
		            messageType,
		            out IBroadcastable<IMessage> broadcaster))
		        throw new Exception($"[MessageBus] INVALID MESSAGE TYPE FOR PARTICULAR MESSAGE BUS: {messageType.ToString()}");
	        
	        broadcaster.Broadcast<IMessage>(message);
        }
        
        private void BroadcastMessage<TMessage>(TMessage message) where TMessage : IMessage
        {
	        var messageType = typeof(TMessage);
	        
	        if (!broadcasterRepository.TryGet(
		            messageType,
		            out IBroadcastable<IMessage> broadcaster))
		        throw new Exception($"[MessageBus] INVALID MESSAGE TYPE FOR PARTICULAR MESSAGE BUS: {messageType.ToString()}");
	        
	        broadcaster.Broadcast(message);
        }
        
        private void PushMessageToPool(IMessage message)
        {
	        var messageType = message.GetType();

	        if (!messageRepository.TryGet(
		            messageType,
		            out IPool<IMessage> messagePool))
		        throw new Exception($"[MessageBus] INVALID MESSAGE TYPE FOR PARTICULAR MESSAGE BUS: {messageType.ToString()}");

	        messagePool.Push(message);
        }

        private void PushMessageToPool<TMessage>(TMessage message) where TMessage : IMessage
        {
	        var messageType = typeof(TMessage);
	        
	        if (!messageRepository.TryGet(
		            messageType,
		            out IPool<IMessage> messagePool))
		        throw new Exception($"[MessageBus] INVALID MESSAGE TYPE FOR PARTICULAR MESSAGE BUS: {typeof(TMessage).ToString()}");

	        messagePool.Push(message);
        }

        #endregion

        #region IMessageReceiver
        
        public IPoolElement<BroadcastHandler<IMessage>> SubscribeTo(Type messageType, Action<IMessage> receiverDelegate)
        {
	        throw new NotImplementedException();
        }

        public IPoolElement<BroadcastHandler<IMessage>> SubscribeTo<TMessage>(Action<IMessage> receiverDelegate) where TMessage : IMessage
        {
	        var messageType = typeof(TMessage);
	        
	        if (!broadcasterRepository.TryGet(
		            messageType,
		            out IBroadcastable<IMessage> broadcaster))
		        throw new Exception($"[MessageBus] INVALID MESSAGE TYPE FOR PARTICULAR MESSAGE BUS: {messageType.ToString()}");

	        var subscription = new BroadcastHandler<TMessage>(receiverDelegate);
	        
	        var subscriptionPoolElement = broadcaster.Subscribe(subscription);

	        return subscriptionPoolElement;
        }

        public IPoolElement<BroadcastHandler<IMessage>> SubscribeToNonAlloc(Type messageType, BroadcastHandler<IMessage> subscription)
        {
	        throw new NotImplementedException();
        }

        public IPoolElement<BroadcastHandler<IMessage>> SubscribeToNonAlloc<TMessage>(BroadcastHandler<TMessage> subscription) where TMessage : IMessage
        {
	        throw new NotImplementedException();
        }

        public void UnsubscribeFrom<TMessage>(IPoolElement<BroadcastHandler<TMessage>> subscriptionPoolElement) where TMessage : IMessage
        {
	        throw new NotImplementedException();
        }

        public void UnsubscribeFrom()
        {
	        
        }
        
        #endregion
    }
}