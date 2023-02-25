using HereticalSolutions.Pools;

namespace HereticalSolutions.Messaging
{
    public interface ISubscribableNonAlloc<TValue>
    {
        IPoolElement<TValue> Subscribe(TValue handler);

        void Unsubscribe(IPoolElement<TValue> subscription);
    }
}