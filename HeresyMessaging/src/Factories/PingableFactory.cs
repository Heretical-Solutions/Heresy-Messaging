using System;
using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Allocations;

using HereticalSolutions.Messaging.Pinging;
using HereticalSolutions.Pools;
using HereticalSolutions.Pools.Factories;

namespace HereticalSolutions.Messaging.Factories
{
	public static partial class MessagingFactory
	{
		#region IPingable
		
		public static IPingable BuildPingable()
		{
			Func<PingHandler> valueAllocationDelegate = PoolsFactory.NullAllocationDelegate<PingHandler>;

			var subscriptionsPool = PoolsFactory.BuildResizableNonAllocPool<PingHandler>(
				valueAllocationDelegate,
				PoolsFactory.BuildIndexedContainer,
				new AllocationCommandDescriptor
				{
					Rule = EAllocationAmountRule.ZERO
				},
				new AllocationCommandDescriptor
				{
					Rule = EAllocationAmountRule.DOUBLE_AMOUNT
				});

			return BuildPinger(subscriptionsPool);
		}

		public static Pinger BuildPinger(
			AllocationCommandDescriptor initial,
			AllocationCommandDescriptor additional)
		{
			Func<PingHandler> valueAllocationDelegate = PoolsFactory.NullAllocationDelegate<PingHandler>;

			var subscriptionsPool = PoolsFactory.BuildResizableNonAllocPool<PingHandler>(
				valueAllocationDelegate,
				PoolsFactory.BuildIndexedContainer,
				initial,
				additional);

			return BuildPinger(subscriptionsPool);
		}
		
		public static Pinger BuildPinger(
			INonAllocPool<PingHandler> subscriptionsPool)
		{
			var contents = ((IModifiable<INonAllocPool<PingHandler>>)subscriptionsPool).Contents;
			
			return new Pinger(
				subscriptionsPool,
				contents);
		}
		
		#endregion
	}
}