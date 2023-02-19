namespace HereticalSolutions.Messaging
{
    public interface IBroadcastable<TValue> : IPoolSubscribable<BroadcasterSubscription<TValue>>
    {
        void Broadcast(TValue value);
    }
}