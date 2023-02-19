using System;

using HereticalSolutions.Collections.Allocations;

using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Managed;
using HereticalSolutions.Collections.Factories;

using HereticalSolutions.Messaging.Pinging;

namespace HereticalSolutions.Messaging.Factories
{
	public static partial class MessagingFactory
	{
		public static IPingable BuildPingable()
		{
			Func<PingerSubscription> valueAllocationDelegate = () => { return null; };

			var arrayPool = CollectionsFactory.BuildPackedArrayPool<PingerSubscription>(
				valueAllocationDelegate,
				CollectionsFactory.BuildIndexedContainer,
				new AllocationCommandDescriptor
				{
					Rule = EAllocationAmountRule.ZERO
				},
				new AllocationCommandDescriptor
				{
					Rule = EAllocationAmountRule.DOUBLE_AMOUNT
				});

			return BuildPinger(arrayPool);
		}

		public static Pinger BuildPinger(
			INonAllocPool<PingerSubscription> pool)
		{
			var subscriptionsArray = ((IContentsRetrievable<IndexedPackedArray<PingerSubscription>>)pool).Contents;
			
			return new Pinger(
				pool,
				subscriptionsArray);
		}

		public static Pinger BuildPinger(
			AllocationCommandDescriptor initial,
			AllocationCommandDescriptor additional)
		{
			Func<PingerSubscription> valueAllocationDelegate = () => { return null; };

			var arrayPool = CollectionFactory.BuildPackedArrayPool<PingerSubscription>(
				valueAllocationDelegate,
				CollectionFactory.BuildIndexedContainer,
				initial,
				additional);

			var packedArray = ((IContentsRetrievable<IndexedPackedArray<PingerSubscription>>)arrayPool).Contents;

			return new Pinger(
				arrayPool,
				packedArray);
		}

		public static Broadcaster<T> BuildBroadcaster<T>()
		{
			Func<BroadcasterSubscription<T>> valueAllocationDelegate = () => { return null; };

			var arrayPool = CollectionFactory.BuildPackedArrayPool<BroadcasterSubscription<T>>(
				valueAllocationDelegate,
				CollectionFactory.BuildIndexedContainer,
				new AllocationCommandDescriptor
				{
					Rule = EAllocationAmountRule.ZERO
				},
				new AllocationCommandDescriptor
				{
					Rule = EAllocationAmountRule.DOUBLE_AMOUNT
				});

			var packedArray = ((IContentsRetrievable<IndexedPackedArray<BroadcasterSubscription<T>>>)arrayPool).Contents;

			return new Broadcaster<T>(
				arrayPool,
				packedArray);
		}

		public static Broadcaster<T> BuildBroadcaster<T>(
			AllocationCommandDescriptor initial,
			AllocationCommandDescriptor additional)
		{
			Func<BroadcasterSubscription<T>> valueAllocationDelegate = () => { return null; };

			var arrayPool = CollectionFactory.BuildPackedArrayPool<BroadcasterSubscription<T>>(
				valueAllocationDelegate,
				CollectionFactory.BuildIndexedContainer,
				initial,
				additional);

			var packedArray = ((IContentsRetrievable<IndexedPackedArray<BroadcasterSubscription<T>>>)arrayPool).Contents;

			return new Broadcaster<T>(
				arrayPool,
				packedArray);
		}
	}
}