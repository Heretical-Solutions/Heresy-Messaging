namespace HereticalSolutions.Messaging
{
    public interface IPingable : IPoolSubscribable<PingerSubscription>
    {
        void Ping();
    }
}