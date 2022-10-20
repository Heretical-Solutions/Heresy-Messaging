using System;

using HereticalSolutions.Allocations;

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

			var indexable = (IIndexable<IPoolElement<PingerSubscription>>)((IContentsRetrievable<IndexedPackedArray<PingerSubscription>>)arrayPool).Contents;

			return new Pinger(
				arrayPool,
				indexable);
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

			var indexable = (IIndexable<IPoolElement<PingerSubscription>>)((IContentsRetrievable<IndexedPackedArray<PingerSubscription>>)arrayPool).Contents;

			return new Pinger(
				arrayPool,
				indexable);
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

			var indexable = (IIndexable<IPoolElement<BroadcasterSubscription<T>>>)((IContentsRetrievable<IndexedPackedArray<BroadcasterSubscription<T>>>)arrayPool).Contents;

			return new Broadcaster<T>(
				arrayPool,
				indexable);
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

			var indexable = (IIndexable<IPoolElement<BroadcasterSubscription<T>>>)((IContentsRetrievable<IndexedPackedArray<BroadcasterSubscription<T>>>)arrayPool).Contents;

			return new Broadcaster<T>(
				arrayPool,
				indexable);
		}
	}
}