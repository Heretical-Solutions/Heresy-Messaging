using System;
using System.Collections.Generic;
using HereticalSolutions.Repositories;

namespace HereticalSolutions.Messaging
{
	public class MessagingService
	{
		private IRepository<string, MessageBus> busRepository;

		public MessagingService(
			IRepository<string, MessageBus> busRepository)
		{
			this.busRepository = busRepository;
		}

		public MessageBus Get(string ID)
		{
			if (!busRepository.TryGet(ID, out MessageBus messageBus))
				throw new Exception($"[MessagingService] INVALID BUS ID: {ID}");

			return messageBus;
		}
	}
}