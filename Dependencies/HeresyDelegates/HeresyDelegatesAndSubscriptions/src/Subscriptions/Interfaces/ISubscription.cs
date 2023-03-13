namespace HereticalSolutions.Delegates
{
    public interface ISubscription<TSubscribable>
    {
        bool Active { get; }

        TSubscribable Publisher { get; }

        void Subscribe(TSubscribable publisher);
        
        void Unsubscribe();
    }
}