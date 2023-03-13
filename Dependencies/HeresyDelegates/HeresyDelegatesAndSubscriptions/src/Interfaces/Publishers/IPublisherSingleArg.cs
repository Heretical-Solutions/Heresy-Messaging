namespace HereticalSolutions.Delegates
{
    public interface IPublisherSingleArg
    {
        void Publish<TValue>(TValue value);
    }
}