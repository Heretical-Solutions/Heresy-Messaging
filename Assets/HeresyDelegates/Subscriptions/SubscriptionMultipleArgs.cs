using System;

using HereticalSolutions.Delegates.Factories;

using HereticalSolutions.Pools;

namespace HereticalSolutions.Delegates.Subscriptions
{
    public class SubscriptionMultipleArgs
        : ISubscription<INonAllocSubscribableMultipleArgs>,
          ISubscriptionHandler<INonAllocSubscribableMultipleArgs, IInvokableMultipleArgs>
    {
        public SubscriptionMultipleArgs(
            Action<object[]> @delegate)
        {
            Delegate = DelegatesFactory.BuildDelegateWrapperMultipleArgs(@delegate);

            Active = false;

            Publisher = null;

            PoolElement = null;
        }

        #region ISubscription
        
        public bool Active { get; private set;  }
        
        public INonAllocSubscribableMultipleArgs Publisher { get; private set; }

        public void Subscribe(INonAllocSubscribableMultipleArgs publisher)
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

        public IInvokableMultipleArgs Delegate { get; private set; }

        public IPoolElement<IInvokableMultipleArgs> PoolElement { get; private set; }
        
        public bool ValidateActivation(INonAllocSubscribableMultipleArgs publisher)
        {
            if (Active)
                throw new Exception("[SubscriptionMultipleArgs] ATTEMPT TO ACTIVATE A SUBSCRIPTION THAT IS ALREADY ACTIVE");
			
            if (Publisher != null)
                throw new Exception("[SubscriptionMultipleArgs] SUBSCRIPTION ALREADY HAS A PUBLISHER");
			
            if (PoolElement != null)
                throw new Exception("[SubscriptionMultipleArgs] SUBSCRIPTION ALREADY HAS A POOL ELEMENT");
			
            if (Delegate == null)
                throw new Exception("[SubscriptionMultipleArgs] INVALID DELEGATE");

            return true;
        }
        
        public void Activate(
            INonAllocSubscribableMultipleArgs publisher,
            IPoolElement<IInvokableMultipleArgs> poolElement)
        {
            PoolElement = poolElement;

            Publisher = publisher;
            
            Active = true;
        }
        
        public bool ValidateTermination(INonAllocSubscribableMultipleArgs publisher)
        {
            if (!Active)
                throw new Exception("[SubscriptionMultipleArgs] ATTEMPT TO TERMINATE A SUBSCRIPTION THAT IS ALREADY ACTIVE");
			
            if (Publisher != publisher)
                throw new Exception("[SubscriptionMultipleArgs] INVALID PUBLISHER");
			
            if (PoolElement == null)
                throw new Exception("[SubscriptionMultipleArgs] INVALID POOL ELEMENT");

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