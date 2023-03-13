using System;

namespace HereticalSolutions.Delegates
{
    public interface ISubscribableSingleArgGeneric<TValue>
    {
        void Subscribe(Action<TValue> @delegate);
        
        void Subscribe(object @delegate);

        void Unsubscribe(Action<TValue> @delegate);
        
        void Unsubscribe(object @delegate);
    }
}