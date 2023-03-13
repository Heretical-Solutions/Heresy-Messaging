using System;

namespace HereticalSolutions.Delegates
{
    public interface ISubscribableSingleArg
    {
        void Subscribe<TValue>(Action<TValue> @delegate);

        void Unsubscribe<TValue>(Action<TValue> @delegate);
    }
}