using HereticalSolutions.Delegates;
using HereticalSolutions.Delegates.Factories;
using HereticalSolutions.Messaging;
using HereticalSolutions.Messaging.Factories;
using UnityEngine;

public class PingerSample : MonoBehaviour
{
    private INonAllocMessageSender messageBusAsSender;

    private INonAllocMessageReceiver messageBusAsReceiver;

    private ISubscription<INonAllocSubscribableSingleArg> subscription;

    private string messageText1 = "Message single generic sent";
    
    private string messageText2 = "Message all generics sent";
    
    private string messageText3 = "Message typeof sent";
    
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
        
        subscription = DelegatesFactory.BuildSubscriptionSingleArg<SampleMessage>(Print);
        
        #endregion

        #region Message

        string messageArgument = "Message contents";

        messageArgs = new[] { messageArgument };

        #endregion
    }

    void Print(SampleMessage message)
    {
        Debug.Log("Ping");
    }

    // Update is called once per frame
    void Update()
    {
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

    void SendMessage()
    {
        bool singleGeneric = UnityEngine.Random.Range(0f, 1f) > 0.5f;

        if (singleGeneric)
        {
            messageArgs[0] = messageText1;
            
            messageBusAsSender
                .PopMessage<SampleMessage>(out var messageSingleGeneric)
                .Write(messageSingleGeneric, messageArgs)
                .SendImmediately(messageSingleGeneric);
            
            return;
        }
        
        bool allGenerics = UnityEngine.Random.Range(0f, 1f) > 0.5f;
        
        if (allGenerics)
        {
            messageArgs[0] = messageText2;
            
            messageBusAsSender
                .PopMessage<SampleMessage>(out var messageAllGenerics)
                .Write<SampleMessage>(messageAllGenerics, messageArgs)
                .SendImmediately<SampleMessage>(messageAllGenerics);
            
            return;
        }
        
        messageArgs[0] = messageText3;
        
        messageBusAsSender
            .PopMessage(typeof(SampleMessage), out var messageTypeofs)
            .Write<SampleMessage>(messageTypeofs, messageArgs)
            .SendImmediately<SampleMessage>(messageTypeofs);
    }

    void Subscribe()
    {
        /*
        bool subscribeFromSubscription = UnityEngine.Random.Range(0f, 1f) > 0.5f;

        if (subscribeFromSubscription)
        {
            subscription.Subscribe((INonAllocSubscribableSingleArg)messageBusAsReceiver);
            
            return;
        }
        */
        
        bool subscribeWithGeneric = UnityEngine.Random.Range(0f, 1f) > 0.5f;
        
        if (subscribeWithGeneric)
            messageBusAsReceiver.SubscribeTo<SampleMessage>(subscription);
        else
            messageBusAsReceiver.SubscribeTo(typeof(SampleMessage), subscription);
    }

    void Unsubscribe()
    {
        bool unsubscribeFromSubscription = UnityEngine.Random.Range(0f, 1f) > 0.5f;

        if (unsubscribeFromSubscription)
        {
            subscription.Unsubscribe();
            
            return;
        }

        bool unsubscribeWithGeneric = UnityEngine.Random.Range(0f, 1f) > 0.5f;
        
        if (unsubscribeWithGeneric)
            messageBusAsReceiver.UnsubscribeFrom<SampleMessage>(subscription);
        else
            messageBusAsReceiver.UnsubscribeFrom(typeof(SampleMessage), subscription);
    }

    private class SampleMessage : IMessage
    {
        private string message;
        
        public void Write(object[] args)
        {
            message = (string)args[0];
        }
    }
}
