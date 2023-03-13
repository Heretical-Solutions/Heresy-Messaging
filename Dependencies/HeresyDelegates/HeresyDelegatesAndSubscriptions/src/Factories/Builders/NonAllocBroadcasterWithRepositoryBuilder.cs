using System.Collections.Generic;

using HereticalSolutions.Delegates.Broadcasting;

namespace HereticalSolutions.Delegates.Factories
{
    public class NonAllocBroadcasterWithRepositoryBuilder
    {
        private List<object> broadcastersList;

        public NonAllocBroadcasterWithRepositoryBuilder()
        {
            broadcastersList = new List<object>();
        }

        public NonAllocBroadcasterWithRepositoryBuilder Add<TBroadcaster>()
        {
            broadcastersList.Add(DelegatesFactory.BuildNonAllocBroadcasterGeneric<TBroadcaster>());

            return this;
        }

        public NonAllocBroadcasterWithRepository Build()
        {
            return DelegatesFactory.BuildNonAllocBroadcasterWithRepository(broadcastersList.ToArray());
        }
    }
}