using System;

using HereticalSolutions.Delegates.Factories;

using HereticalSolutions.Pools;

namespace HereticalSolutions.Delegates.Subscriptions
{
    public class SubscriptionSingleArg<TValue>
        : ISubscription<INonAllocSubscribableSingleArg>,
          ISubscriptionHandler<INonAllocSubscribableSingleArg, IInvokableSingleArg>
    {
        public SubscriptionSingleArg(
            Action<TValue> @delegate)
        {
            Delegate = DelegatesFactory.BuildDelegateWrapperSingleArg<TValue>(@delegate);

            Active = false;

            Publisher = null;

            PoolElement = null;
        }

        #region ISubscription
        
        public bool Active { get; private set;  }
        
        public INonAllocSubscribableSingleArg Publisher { get; private set; }

        public void Subscribe(INonAllocSubscribableSingleArg publisher)
        {
            if (Active)
                return;
            
            publisher.Subscribe(typeof(TValue), this);
        }

        public void Unsubscribe()
        {
            if (!Active)
                return;

            Publisher.Unsubscribe(typeof(TValue), this);
        }
        
        #endregion

        #region ISubscriptionHandler

        public IInvokableSingleArg Delegate { get; private set; }

        public IPoolElement<IInvokableSingleArg> PoolElement { get; private set; }
        
        public bool ValidateActivation(INonAllocSubscribableSingleArg publisher)
        {
            if (Active)
                throw new Exception("[SubscriptionSingleArg] ATTEMPT TO ACTIVATE A SUBSCRIPTION THAT IS ALREADY ACTIVE");
			
            if (Publisher != null)
                throw new Exception("[SubscriptionSingleArg] SUBSCRIPTION ALREADY HAS A PUBLISHER");
			
            if (PoolElement != null)
                throw new Exception("[SubscriptionSingleArg] SUBSCRIPTION ALREADY HAS A POOL ELEMENT");
			
            if (Delegate == null)
                throw new Exception("[SubscriptionSingleArg] INVALID DELEGATE");

            return true;
        }
        
        public void Activate(
            INonAllocSubscribableSingleArg publisher,
            IPoolElement<IInvokableSingleArg> poolElement)
        {
            PoolElement = poolElement;

            Publisher = publisher;
            
            Active = true;
        }
        
        public bool ValidateTermination(INonAllocSubscribableSingleArg publisher)
        {
            if (!Active)
                throw new Exception("[SubscriptionSingleArg] ATTEMPT TO TERMINATE A SUBSCRIPTION THAT IS ALREADY ACTIVE");
			
            if (Publisher != publisher)
                throw new Exception("[SubscriptionSingleArg] INVALID PUBLISHER");
			
            if (PoolElement == null)
                throw new Exception("[SubscriptionSingleArg] INVALID POOL ELEMENT");

            return true;
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