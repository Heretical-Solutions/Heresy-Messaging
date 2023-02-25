namespace HereticalSolutions.Messaging
{
    public interface IBroadcastable<TValue> : ISubscribableNonAlloc<BroadcastHandler<TValue>>
    {
        void Broadcast(TValue value);
    }
}