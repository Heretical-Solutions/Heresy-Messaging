using HereticalSolutions.Collections;

namespace HereticalSolutions.Messaging
{
    public interface IPoolSubscribable<TValue>
    {
        IPoolElement<TValue> Subscribe(TValue subscription);

        void Unsubscribe(IPoolElement<TValue> subscriptionElement);
    }
}