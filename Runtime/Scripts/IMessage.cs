namespace HereticalSolutions.Messaging
{
	public interface IMessage
	{
		void Write(params object[] args);
	}
}