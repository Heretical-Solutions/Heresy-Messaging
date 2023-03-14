using HereticalSolutions.Delegates;
using HereticalSolutions.Delegates.Factories;

using HereticalSolutions.Messaging;
using HereticalSolutions.Messaging.Factories;

using UnityEngine;

public class NonAllocMessageBusSample : MonoBehaviour
{
    private INonAllocMessageSender messageBusAsSender;

    private INonAllocMessageReceiver messageBusAsReceiver;

    private ISubscription subscription;

    private readonly string messageText1 = "Message all generics sent";
    
    private readonly string messageText2 = "Message typeof sent";
    
    private object[] messageArgs;
    
    // Start is called before the first frame update
    void Start()
    {
        #region Message bus
        
        var builder = new NonAllocMessageBusBuilder();

        builder.AddMessageType<SampleMessage>();

        var messageBus = builder.Build();

        messageBusAsSender = (INonAllocMessageSender)messageBus;
        
        messageBusAsReceiver = (INonAllocMessageReceiver)messageBus;
        
        #endregion

        #region Subscription
        
        subscription = DelegatesFactory.BuildSubscriptionSingleArgGeneric<SampleMessage>(Print);
        
        #endregion

        #region Message

        string messageArgument = "Message contents";

        messageArgs = new[] { messageArgument };

        #endregion
    }

    void Print(SampleMessage message)
    {
        //Just imagine this. I need to ensure there are no allocations on 'non alloc' message bus so i leave this commented out
        
        //Debug.Log(message.Message);
    }

    // Update is called once per frame
    void Update()
    {
        DeliverMessagesInMailbox();
        
        if (subscription.Active)
            SendMessage();
        
        bool doSomething = UnityEngine.Random.Range(0f, 1f) < 0.1f;

        if (doSomething)
        {
            if (subscription.Active)
                Unsubscribe();
            else
                Subscribe();
        }
    }

    void DeliverMessagesInMailbox()
    {
        messageBusAsSender.DeliverMessagesInMailbox();
    }

    void SendMessage()
    {
        bool allGenerics = UnityEngine.Random.Range(0f, 1f) > 0.5f;
        
        bool sendIntoMailbox = UnityEngine.Random.Range(0f, 1f) > 0.5f;
        
        if (allGenerics)
        {
            messageArgs[0] = messageText1;

            if (sendIntoMailbox)
            {
                messageBusAsSender
                    .PopMessage<SampleMessage>(out var messageAllGenerics)
                    .Write<SampleMessage>(messageAllGenerics, messageArgs)
                    .Send<SampleMessage>(messageAllGenerics);
            }
            else
            {
                messageBusAsSender
                    .PopMessage<SampleMessage>(out var messageAllGenerics)
                    .Write<SampleMessage>(messageAllGenerics, messageArgs)
                    .SendImmediately<SampleMessage>(messageAllGenerics);
            }

            return;
        }
        
        messageArgs[0] = messageText2;

        if (sendIntoMailbox)
        {
            messageBusAsSender
                .PopMessage(typeof(SampleMessage), out var messageTypeofs)
                .Write<SampleMessage>(messageTypeofs, messageArgs)
                .Send<SampleMessage>(messageTypeofs);
        }
        else
        {
            messageBusAsSender
                .PopMessage(typeof(SampleMessage), out var messageTypeofs)
                .Write<SampleMessage>(messageTypeofs, messageArgs)
                .SendImmediately<SampleMessage>(messageTypeofs);            
        }
    }

    void Subscribe()
    {
        bool subscribeWithGeneric = UnityEngine.Random.Range(0f, 1f) > 0.5f;
        
        if (subscribeWithGeneric)
            messageBusAsReceiver.SubscribeTo<SampleMessage>(subscription);
        else
            messageBusAsReceiver.SubscribeTo(typeof(SampleMessage), subscription);
    }

    void Unsubscribe()
    {
        bool unsubscribeWithGeneric = UnityEngine.Random.Range(0f, 1f) > 0.5f;
        
        if (unsubscribeWithGeneric)
            messageBusAsReceiver.UnsubscribeFrom<SampleMessage>(subscription);
        else
            messageBusAsReceiver.UnsubscribeFrom(typeof(SampleMessage), subscription);
    }

    private class SampleMessage : IMessage
    {
        private string message;

        public string Message
        {
            get => message;
        }

        public void Write(object[] args)
        {
            message = (string)args[0];
        }
    }
}
