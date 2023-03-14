using System;
using HereticalSolutions.Delegates;
using HereticalSolutions.Delegates.Factories;

using HereticalSolutions.Messaging;
using HereticalSolutions.Messaging.Factories;

using UnityEngine;

public class MessageBusSample : MonoBehaviour
{
    private IMessageSender messageBusAsSender;

    private IMessageReceiver messageBusAsReceiver;

    private readonly string messageText1 = "Message all generics sent";
    
    private readonly string messageText2 = "Message typeof sent";
    
    private object[] messageArgs;

    private bool subscriptionActive;
    
    // Start is called before the first frame update
    void Start()
    {
        #region Message bus
        
        var builder = new MessageBusBuilder();

        builder.AddMessageType<SampleMessage>();

        var messageBus = builder.Build();

        messageBusAsSender = (IMessageSender)messageBus;
        
        messageBusAsReceiver = (IMessageReceiver)messageBus;
        
        #endregion

        #region Message

        string messageArgument = "Message contents";

        messageArgs = new[] { messageArgument };

        #endregion
    }

    void Print(SampleMessage message)
    {
        Debug.Log(message.Message);
    }

    // Update is called once per frame
    void Update()
    {
        DeliverMessagesInMailbox();
        
        SendMessage();
        
        bool doSomething = UnityEngine.Random.Range(0f, 1f) < 0.1f;

        if (doSomething)
        {
            if (subscriptionActive)
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
                .Write(messageTypeofs, messageArgs)
                .Send(messageTypeofs);
        }
        else
        {
            messageBusAsSender
                .PopMessage(typeof(SampleMessage), out var messageTypeofs)
                .Write(messageTypeofs, messageArgs)
                .SendImmediately(messageTypeofs);            
        }
    }

    void Subscribe()
    {
        bool subscribeWithGeneric = UnityEngine.Random.Range(0f, 1f) > 0.5f;
        
        if (subscribeWithGeneric)
            messageBusAsReceiver.SubscribeTo<SampleMessage>(Print);
        else
        {
            Action<SampleMessage> subscription = Print;
            
            messageBusAsReceiver.SubscribeTo(typeof(SampleMessage), subscription);
        }

        subscriptionActive = true;
    }

    void Unsubscribe()
    {
        bool unsubscribeWithGeneric = UnityEngine.Random.Range(0f, 1f) > 0.5f;
        
        if (unsubscribeWithGeneric)
            messageBusAsReceiver.UnsubscribeFrom<SampleMessage>(Print);
        else
        {
            Action<SampleMessage> subscription = Print;
            
            messageBusAsReceiver.UnsubscribeFrom(typeof(SampleMessage), subscription);
        }

        subscriptionActive = false;
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
