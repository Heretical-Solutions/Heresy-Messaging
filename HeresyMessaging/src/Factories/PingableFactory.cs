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
			Func<PingerSubscription> valueAllocationDelegate = PoolsFactory.NullAllocationDelegate<PingerSubscription>;

			var subscriptionsPool = PoolsFactory.BuildResizableNonAllocPool<PingerSubscription>(
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
			Func<PingerSubscription> valueAllocationDelegate = PoolsFactory.NullAllocationDelegate<PingerSubscription>;

			var subscriptionsPool = PoolsFactory.BuildResizableNonAllocPool<PingerSubscription>(
				valueAllocationDelegate,
				PoolsFactory.BuildIndexedContainer,
				initial,
				additional);

			return BuildPinger(subscriptionsPool);
		}
		
		public static Pinger BuildPinger(
			INonAllocPool<PingerSubscription> subscriptionsPool)
		{
			var contents = ((IModifiable<INonAllocPool<PingerSubscription>>)subscriptionsPool).Contents;
			
			return new Pinger(
				subscriptionsPool,
				contents);
		}
		
		#endregion
	}
}