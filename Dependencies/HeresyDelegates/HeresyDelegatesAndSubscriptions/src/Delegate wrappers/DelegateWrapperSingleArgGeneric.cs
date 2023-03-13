using System;

namespace HereticalSolutions.Delegates.Wrappers
{
    public class DelegateWrapperSingleArgGeneric<TValue> : IInvokableSingleArgGeneric<TValue>
    {
        private readonly Action<TValue> @delegate;

        public DelegateWrapperSingleArgGeneric(Action<TValue> @delegate)
        {
            this.@delegate = @delegate;
        }

        public void Invoke(TValue argument)
        {
            @delegate?.Invoke(argument);
        }
    }
}