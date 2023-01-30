namespace HereticalSolutions.Messaging
{
	public interface IMessageReceivable
	{
		void Receive(int maxAmount);

		void ReceiveAll();
	}
}