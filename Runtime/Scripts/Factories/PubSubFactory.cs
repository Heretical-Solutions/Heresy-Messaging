using System;

using HereticalSolutions.Allocations;
using HereticalSolutions.Allocations.Internal;

using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Managed;
using HereticalSolutions.Collections.Factories;

namespace HereticalSolutions.Messaging.Factories
{
	public static class PubSubFactory
	{
		public static Pinger BuildPinger()
		{
			Func<Action> valueAllocationDelegate = () => { return null; };

			var arrayPool = CollectionFactory.BuildNonAllocPool<Action>(
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

			var indexable = (IIndexable<IPoolElement<Action>>)((IContentsRetrievable<IndexedPackedArray<Action>>)arrayPool).Contents;

			return new Pinger(
				arrayPool,
				indexable);
		}

		public static Broadcaster<T> BuildBroadcaster<T>()
		{
			Func<Action<T>> valueAllocationDelegate = () => { return null; };

			var arrayPool = CollectionFactory.BuildNonAllocPool<Action<T>>(
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

			var indexable = (IIndexable<IPoolElement<Action<T>>>)((IContentsRetrievable<IndexedPackedArray<Action<T>>>)arrayPool).Contents;

			return new Broadcaster<T>(
				arrayPool,
				indexable);
		}
	}
}