namespace HereticalSolutions.Messaging
{
    public interface IPingable : ISubscribableNonAlloc<PingHandler>
    {
        void Ping();
    }
}