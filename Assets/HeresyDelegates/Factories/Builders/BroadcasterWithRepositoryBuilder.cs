using System.Collections.Generic;

using HereticalSolutions.Delegates.Broadcasting;

namespace HereticalSolutions.Delegates.Factories
{
    public class BroadcasterWithRepositoryBuilder
    {
        private List<object> broadcastersList;

        public BroadcasterWithRepositoryBuilder()
        {
            broadcastersList = new List<object>();
        }

        public BroadcasterWithRepositoryBuilder Add<TBroadcaster>()
        {
            broadcastersList.Add(DelegatesFactory.BuildBroadcasterGeneric<TBroadcaster>());

            return this;
        }

        public BroadcasterWithRepository Build()
        {
            return DelegatesFactory.BuildBroadcasterWithRepository(broadcastersList.ToArray());
        }
    }
}