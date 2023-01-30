using System;

using HereticalSolutions.Collections.Allocations;

using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Managed;
using HereticalSolutions.Collections.Factories;

namespace HereticalSolutions.Messaging.Factories
{
	public static class PubSubFactory
	{
		public static Pinger BuildPinger()
		{
			Func<PingerSubscription> valueAllocationDelegate = () => { return null; };

			var arrayPool = CollectionFactory.BuildPackedArrayPool<PingerSubscription>(
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

			var packedArray = ((IContentsRetrievable<IndexedPackedArray<PingerSubscription>>)arrayPool).Contents;

			return new Pinger(
				arrayPool,
				packedArray);
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