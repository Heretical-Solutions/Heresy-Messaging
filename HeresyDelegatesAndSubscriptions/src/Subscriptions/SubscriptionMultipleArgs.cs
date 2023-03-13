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
        
        public void Activate(
            INonAllocSubscribableMultipleArgs publisher,
            IPoolElement<IInvokableMultipleArgs> poolElement)
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