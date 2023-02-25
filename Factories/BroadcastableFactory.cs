using System;
using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Allocations;

using HereticalSolutions.Messaging.Broadcasting;
using HereticalSolutions.Pools;
using HereticalSolutions.Pools.Factories;

namespace HereticalSolutions.Messaging.Factories
{
    public static partial class MessagingFactory
    {
        public static Broadcaster<T> BuildBroadcaster<T>()
        {
            Func<BroadcasterSubscription<T>> valueAllocationDelegate = PoolsFactory.NullAllocationDelegate<BroadcasterSubscription<T>>;

            var subscriptionsPool = PoolsFactory.BuildResizableNonAllocPool<BroadcasterSubscription<T>>(
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

            return BuildBroadcaster(subscriptionsPool);
        }

        public static Broadcaster<T> BuildBroadcaster<T>(
            AllocationCommandDescriptor initial,
            AllocationCommandDescriptor additional)
        {
            Func<BroadcasterSubscription<T>> valueAllocationDelegate = PoolsFactory.NullAllocationDelegate<BroadcasterSubscription<T>>;

            var subscriptionsPool = PoolsFactory.BuildResizableNonAllocPool<BroadcasterSubscription<T>>(
                valueAllocationDelegate,
                PoolsFactory.BuildIndexedContainer,
                initial,
                additional);

            return BuildBroadcaster(subscriptionsPool);
        }
        
        public static Broadcaster<T> BuildBroadcaster<T>(
            INonAllocPool<BroadcasterSubscription<T>> subscriptionsPool)
        {
            var contents = ((IModifiable<INonAllocPool<BroadcasterSubscription<T>>>)subscriptionsPool).Contents;
			
            return new Broadcaster<T>(
                subscriptionsPool,
                contents);
        }
    }
}