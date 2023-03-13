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
    public class MessageBusBuilder
    {
        private readonly IObjectRepository messagePoolRepository;

        private readonly BroadcasterWithRepositoryBuilder broadcasterBuilder;
        
        private readonly AllocationCommand<IMessage> initialAllocationCommand;
        
        private readonly AllocationCommand<IMessage> additionalAllocationCommand;

        public MessageBusBuilder()
        {
            messagePoolRepository = RepositoriesFactory.BuildDictionaryObjectRepository();

            broadcasterBuilder = new BroadcasterWithRepositoryBuilder();
            
            Func<IMessage> valueAllocationDelegate= PoolsFactory.NullAllocationDelegate<IMessage>;

            initialAllocationCommand = new AllocationCommand<IMessage>
            {
                Descriptor = new AllocationCommandDescriptor
                {
                    Rule = EAllocationAmountRule.ADD_ONE
                },

                AllocationDelegate = valueAllocationDelegate
            };
            
            additionalAllocationCommand = new AllocationCommand<IMessage>
            {
                Descriptor = new AllocationCommandDescriptor
                {
                    Rule = EAllocationAmountRule.DOUBLE_AMOUNT
                },

                AllocationDelegate = valueAllocationDelegate
            };
        }

        public MessageBusBuilder AddMessageType<TMessage>()
        {
            IPool<IMessage> messagePool = PoolsFactory.BuildStackPool<IMessage>(
                initialAllocationCommand,
                additionalAllocationCommand);
            
            messagePoolRepository.Add(
                typeof(TMessage),
                messagePool);

            broadcasterBuilder.Add<TMessage>();

            return this;
        }

        public MessageBus Build()
        {
            return new MessageBus(
                broadcasterBuilder.Build(),
                (IReadOnlyObjectRepository)messagePoolRepository,
                new Queue<IMessage>());
        }
    }
}