using HereticalSolutions.Repositories;

namespace HereticalSolutions.Delegates.Broadcasting
{
    public class NonAllocBroadcasterWithRepository
        : IPublisherSingleArg,
          INonAllocSubscribableSingleArg
    {
        private readonly IReadOnlyObjectRepository broadcasterRepository;

        public NonAllocBroadcasterWithRepository(IReadOnlyObjectRepository broadcasterRepository)
        {
            this.broadcasterRepository = broadcasterRepository;
        }

        #region IPublisherSingleArg

        public void Publish<TValue>(TValue value)
        {
            var messageType = typeof(TValue);
            
            if (!broadcasterRepository.TryGet(
                    messageType,
                    out object broadcasterObject))
                return;

            var broadcaster = (IPublisherSingleArgGeneric<TValue>)broadcasterObject;
            
            broadcaster.Publish(value);
        }

        #endregion

        #region INonAllocSubscribableSingleArg
		
        public void Subscribe<TValue>(ISubscriptionHandler<INonAllocSubscribableSingleArgGeneric<TValue>, IInvokableSingleArgGeneric<TValue>> subscription)
        {
            var messageType = typeof(TValue);
            
            if (!broadcasterRepository.TryGet(
                    messageType,
                    out object broadcasterObject))
                return;

            var broadcaster = (INonAllocSubscribableSingleArgGeneric<TValue>)broadcasterObject;
            
            broadcaster.Subscribe(subscription);
        }

        public void Unsubscribe<TValue>(ISubscriptionHandler<INonAllocSubscribableSingleArgGeneric<TValue>, IInvokableSingleArgGeneric<TValue>> subscription)
        {
            var messageType = typeof(TValue);
            
            if (!broadcasterRepository.TryGet(
                    messageType,
                    out object broadcasterObject))
                return;

            var broadcaster = (INonAllocSubscribableSingleArgGeneric<TValue>)broadcasterObject;
            
            broadcaster.Unsubscribe(subscription);
        }

        #endregion
    }
}