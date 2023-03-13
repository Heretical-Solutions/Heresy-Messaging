namespace HereticalSolutions.Messaging
{
	public interface IMessage
	{
		void Write(object[] args);
		
		//params are a rather bad idea
		//void Write(params object[] args);
	}
}