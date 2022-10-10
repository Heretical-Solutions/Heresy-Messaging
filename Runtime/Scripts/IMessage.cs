namespace HereticalSolutions.Messaging
{
	public interface IMessage
	{
		IMessage Write(params object[] args);
	}
}