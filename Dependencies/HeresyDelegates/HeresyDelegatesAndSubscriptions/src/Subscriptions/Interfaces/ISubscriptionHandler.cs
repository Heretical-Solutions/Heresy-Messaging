using HereticalSolutions.Pools;

namespace HereticalSolutions.Delegates
{
    public interface ISubscriptionHandler<TSubscribable, TInvokable> : ISubscription<TSubscribable>
    {
        TInvokable Delegate { get; }

        IPoolElement<TInvokable> PoolElement { get; }

        void Activate(
            TSubscribable publisher,
            IPoolElement<TInvokable> poolElement);

        void Terminate();
    }
}