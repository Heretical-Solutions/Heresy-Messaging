using System;

using HereticalSolutions.Delegates.Factories;

using HereticalSolutions.Pools;

namespace HereticalSolutions.Delegates.Subscriptions
{
    public class SubscriptionSingleArgGeneric<TValue>
        : ISubscription<INonAllocSubscribableSingleArgGeneric<TValue>>,
          ISubscriptionHandler<INonAllocSubscribableSingleArgGeneric<TValue>, IInvokableSingleArgGeneric<TValue>>
    {
        public SubscriptionSingleArgGeneric(
            Action<TValue> @delegate)
        {
            Delegate = DelegatesFactory.BuildDelegateWrapperSingleArgGeneric(@delegate);

            Active = false;

            Publisher = null;

            PoolElement = null;
        }

        #region ISubscription
        
        public bool Active { get; private set;  }
        
        public INonAllocSubscribableSingleArgGeneric<TValue> Publisher { get; private set; }

        public void Subscribe(INonAllocSubscribableSingleArgGeneric<TValue> publisher)
        {
            if (Active)
                return;
            
            publisher.Subscribe(this);
        }

        public void Unsubscribe()
        {
            if (!Active)
                return;

            Publisher.Unsubscribe(this);
        }
        
        #endregion

        #region ISubscriptionHandler

        public IInvokableSingleArgGeneric<TValue> Delegate { get; private set; }

        public IPoolElement<IInvokableSingleArgGeneric<TValue>> PoolElement { get; private set; }
        
        public void Activate(
            INonAllocSubscribableSingleArgGeneric<TValue> publisher,
            IPoolElement<IInvokableSingleArgGeneric<TValue>> poolElement)
        {
            PoolElement = poolElement;

            Publisher = publisher;
            
            Active = true;
        }
        
        public void Terminate()
        {
            PoolElement = null;
            
            Publisher = null;
            
            Active = false;
        }

        #endregion
    }
}