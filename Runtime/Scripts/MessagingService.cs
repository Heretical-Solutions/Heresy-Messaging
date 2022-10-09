using System;
using System.Collections.Generic;

namespace HereticalSolutions.Messaging
{
	public class MessagingService
	{
		private Dictionary<string, MessageBus> busRepository;

		public MessagingService(
			Dictionary<string, MessageBus> busRepository)
		{
			this.busRepository = busRepository;
		}

		public MessageBus Get(string ID)
		{
			if (!busRepository.TryGetValue(ID, out MessageBus messageBus))
				throw new Exception($"[MessagingService] INVALID BUS ID: {ID}");

			return messageBus;
		}
	}
}