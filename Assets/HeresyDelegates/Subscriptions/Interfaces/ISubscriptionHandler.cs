using HereticalSolutions.Pools;

namespace HereticalSolutions.Delegates
{
    public interface ISubscriptionHandler<TSubscribable, TInvokable>
    {
        TInvokable Delegate { get; }

        IPoolElement<TInvokable> PoolElement { get; }

        bool ValidateActivation(TSubscribable publisher);
        
        void Activate(
            TSubscribable publisher,
            IPoolElement<TInvokable> poolElement);

        bool ValidateTermination(TSubscribable publisher);
        
        void Terminate();
    }
}