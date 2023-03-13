namespace HereticalSolutions.Delegates
{
    public interface INonAllocSubscribableNoArgs
    {
        void Subscribe(ISubscriptionHandler<INonAllocSubscribableNoArgs, IInvokableNoArgs> subscription);

        void Unsubscribe(ISubscriptionHandler<INonAllocSubscribableNoArgs, IInvokableNoArgs> subscription);
    }
}