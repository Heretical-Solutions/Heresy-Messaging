using HereticalSolutions.Delegates;
using HereticalSolutions.Delegates.Factories;

using UnityEngine;

public class NonAllocPingerSample : MonoBehaviour
{
    private IPublisherNoArgs pingerAsPublisher;

    private INonAllocSubscribableNoArgs pingerAsSubscribable;

    private ISubscription subscription;

    // Start is called before the first frame update
    void Start()
    {
        #region Pinger

        var pinger = DelegatesFactory.BuildNonAllocPinger();
        
        pingerAsPublisher = (IPublisherNoArgs)pinger;
        
        pingerAsSubscribable = (INonAllocSubscribableNoArgs)pinger;
        
        #endregion

        #region Subscription
        
        subscription = DelegatesFactory.BuildSubscriptionNoArgs(Print);
        
        #endregion
    }

    void Print()
    {
        //Just imagine this. I need to ensure there are no allocations on 'non alloc' message bus so i leave this commented out
        //Debug.Log("Ping");
    }

    // Update is called once per frame
    void Update()
    {
        Ping();
        
        bool doSomething = UnityEngine.Random.Range(0f, 1f) < 0.1f;

        if (doSomething)
        {
            if (subscription.Active)
                Unsubscribe();
            else
                Subscribe();
        }
    }

    void Ping()
    {
        pingerAsPublisher.Publish();
    }

    void Subscribe()
    {
        bool subscribeWithGeneric = UnityEngine.Random.Range(0f, 1f) > 0.5f;
        
        if (subscribeWithGeneric)
            pingerAsSubscribable.Subscribe(subscription);
        else
            pingerAsSubscribable.Subscribe(subscription);
    }

    void Unsubscribe()
    {
        bool unsubscribeWithGeneric = UnityEngine.Random.Range(0f, 1f) > 0.5f;
        
        if (unsubscribeWithGeneric)
            pingerAsSubscribable.Unsubscribe(subscription);
        else
            pingerAsSubscribable.Unsubscribe(subscription);
    }
}
