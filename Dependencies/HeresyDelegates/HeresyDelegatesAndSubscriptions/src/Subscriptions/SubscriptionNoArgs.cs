using System;

using HereticalSolutions.Delegates.Factories;

using HereticalSolutions.Pools;

namespace HereticalSolutions.Delegates.Subscriptions
{
    public class SubscriptionNoArgs
        : ISubscription<INonAllocSubscribableNoArgs>,
          ISubscriptionHandler<INonAllocSubscribableNoArgs, IInvokableNoArgs>
    {
        public SubscriptionNoArgs(
            Action @delegate)
        {
            Delegate = DelegatesFactory.BuildDelegateWrapperNoArgs(@delegate);

            Active = false;

            Publisher = null;

            PoolElement = null;
        }

        #region ISubscription
        
        public bool Active { get; private set;  }
        
        public INonAllocSubscribableNoArgs Publisher { get; private set; }

        public void Subscribe(INonAllocSubscribableNoArgs publisher)
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

        public IInvokableNoArgs Delegate { get; private set; }

        public IPoolElement<IInvokableNoArgs> PoolElement { get; private set; }
        
        public void Activate(
            INonAllocSubscribableNoArgs publisher,
            IPoolElement<IInvokableNoArgs> poolElement)
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