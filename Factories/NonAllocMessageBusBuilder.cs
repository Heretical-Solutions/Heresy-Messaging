using System;
using System.Collections.Generic;
using HereticalSolutions.Collections.Allocations;
using HereticalSolutions.Delegates;
using HereticalSolutions.Delegates.Factories;
using HereticalSolutions.Pools;
using HereticalSolutions.Pools.Factories;
using HereticalSolutions.Repositories;
using HereticalSolutions.Repositories.Factories;

namespace HereticalSolutions.Messaging.Factories
{
    public class NonAllocMessageBusBuilder
    {
        private readonly IObjectRepository messagePoolRepository;

        private readonly NonAllocBroadcasterWithRepositoryBuilder broadcasterBuilder;
        
        private Func<IMessage> valueAllocationDelegate = PoolsFactory.NullAllocationDelegate<IMessage>;

        public NonAllocMessageBusBuilder()
        {
            messagePoolRepository = RepositoriesFactory.BuildDictionaryObjectRepository();

            broadcasterBuilder = new NonAllocBroadcasterWithRepositoryBuilder();
            
            valueAllocationDelegate= PoolsFactory.NullAllocationDelegate<IMessage>;
        }

        public NonAllocMessageBusBuilder AddMessageType<TMessage>()
        {
            INonAllocDecoratedPool<IMessage> messagePool = PoolsFactory.BuildResizableNonAllocPool<IMessage>(
                valueAllocationDelegate,
                new []
                {
                    PoolsFactory.BuildIndexedMetadataDescriptor()
                },
                new AllocationCommandDescriptor
                {
                    Rule = EAllocationAmountRule.ADD_ONE
                },
                new AllocationCommandDescriptor
                {
                    Rule = EAllocationAmountRule.DOUBLE_AMOUNT
                });
            
            messagePoolRepository.Add(
                typeof(TMessage),
                messagePool);

            broadcasterBuilder.Add<TMessage>();

            return this;
        }

        public NonAllocMessageBus Build()
        {
            return new NonAllocMessageBus(
                broadcasterBuilder.Build(),
                (IReadOnlyObjectRepository)messagePoolRepository,
                new Queue<IPoolElement<IMessage>>());
        }
    }
}