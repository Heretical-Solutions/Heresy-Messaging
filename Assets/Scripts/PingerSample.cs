using HereticalSolutions.Delegates;
using HereticalSolutions.Delegates.Factories;

using UnityEngine;

public class PingerSample : MonoBehaviour
{
    private IPublisherNoArgs pingerAsPublisher;

    private ISubscribableNoArgs pingerAsSubscribable;
    
    private bool subscriptionActive;

    // Start is called before the first frame update
    void Start()
    {
        #region Pinger

        var pinger = DelegatesFactory.BuildPinger();
        
        pingerAsPublisher = (IPublisherNoArgs)pinger;
        
        pingerAsSubscribable = (ISubscribableNoArgs)pinger;
        
        #endregion
    }

    void Print()
    {
        Debug.Log("Ping");
    }

    // Update is called once per frame
    void Update()
    {
        Ping();
        
        bool doSomething = UnityEngine.Random.Range(0f, 1f) < 0.1f;

        if (doSomething)
        {
            if (subscriptionActive)
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
            pingerAsSubscribable.Subscribe(Print);
        else
            pingerAsSubscribable.Subscribe(Print);

        subscriptionActive = true;
    }

    void Unsubscribe()
    {
        bool unsubscribeWithGeneric = UnityEngine.Random.Range(0f, 1f) > 0.5f;
        
        if (unsubscribeWithGeneric)
            pingerAsSubscribable.Unsubscribe(Print);
        else
            pingerAsSubscribable.Unsubscribe(Print);

        subscriptionActive = false;
    }
}
