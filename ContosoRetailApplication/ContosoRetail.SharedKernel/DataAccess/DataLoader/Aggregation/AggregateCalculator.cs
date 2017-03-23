
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ContosoRetail.SharedKernel.DataAccess.DataLoader.Aggregation
{
    
    public class AggregateCalculator<T>
    {
        IEnumerable _data;
        IAccessor<T> _accessor;

        ContosoRetail.SharedKernel.DataAccess.DataLoader.Aggregation.Aggregator<T>[] _totalAggregators;
        string[] _totalSelectors;

        string[] _groupSummaryTypes;
        string[] _groupSelectors;
        Stack<ContosoRetail.SharedKernel.DataAccess.DataLoader.Aggregation.Aggregator<T>[]> _groupAggregatorsStack;


        public AggregateCalculator(IEnumerable data, IAccessor<T> accessor, IEnumerable<SummaryInfo> totalSummary, IEnumerable<SummaryInfo> groupSummary)
        {
            _data = data;
            _accessor = accessor;

            if (totalSummary != null)
            {
                _totalAggregators = totalSummary.Select(i => CreateAggregator(i.SummaryType)).ToArray();
                _totalSelectors = totalSummary.Select(i => i.Selector).ToArray();
            }

            if (groupSummary != null)
            {
                _groupSummaryTypes = groupSummary.Select(i => i.SummaryType).ToArray();
                _groupSelectors = groupSummary.Select(i => i.Selector).ToArray();
                _groupAggregatorsStack = new Stack<ContosoRetail.SharedKernel.DataAccess.DataLoader.Aggregation.Aggregator<T>[]>();
            }
        }

        public object[] Run()
        {
            foreach (var item in _data)
                ProcessItem(item);

            if (_totalAggregators != null)
                return Finish(_totalAggregators);

            return null;
        }

        void ProcessItem(object item)
        {
            var group = item as Group;
            if (group != null)
            {
                ProcessGroup(group);
            }
            else
            {
                if (_groupAggregatorsStack != null)
                {
                    foreach (var groupAggregators in _groupAggregatorsStack)
                        Step(item, groupAggregators, _groupSelectors);
                }

                if (_totalAggregators != null)
                    Step(item, _totalAggregators, _totalSelectors);
            }
        }

        void ProcessGroup(Group group)
        {
            if (_groupAggregatorsStack != null)
                _groupAggregatorsStack.Push(_groupSummaryTypes.Select(CreateAggregator).ToArray());

            foreach (var i in group.Items)
                ProcessItem(i);

            if (_groupAggregatorsStack != null)
                group.Summary = Finish(_groupAggregatorsStack.Pop());
        }

        void Step(object obj, ContosoRetail.SharedKernel.DataAccess.DataLoader.Aggregation.Aggregator<T>[] aggregators, string[] selectors)
        {
            var typed = (T)obj;
            for (var i = 0; i < aggregators.Length; i++)
                aggregators[i].Step(typed, selectors[i]);
        }

        object[] Finish(ContosoRetail.SharedKernel.DataAccess.DataLoader.Aggregation.Aggregator<T>[] aggregators)
        {
            return aggregators.Select(a => a.Finish()).ToArray();
        }


        ContosoRetail.SharedKernel.DataAccess.DataLoader.Aggregation.Aggregator<T> CreateAggregator(string summaryType)
        {
            switch (summaryType)
            {
                case "sum":
                    return new ContosoRetail.SharedKernel.DataAccess.DataLoader.Aggregation.SumAggregator<T>(_accessor);
                case "min":
                    return new ContosoRetail.SharedKernel.DataAccess.DataLoader.Aggregation.MinAggregator<T>(_accessor);
                case "max":
                    return new ContosoRetail.SharedKernel.DataAccess.DataLoader.Aggregation.MaxAggregator<T>(_accessor);
                case "avg":
                    return new ContosoRetail.SharedKernel.DataAccess.DataLoader.Aggregation.AvgAggregator<T>(_accessor, new ContosoRetail.SharedKernel.DataAccess.DataLoader.Aggregation.CountAggregator<T>(_accessor, true));
                case "count":
                    return new ContosoRetail.SharedKernel.DataAccess.DataLoader.Aggregation.CountAggregator<T>(_accessor, false);

                case "remoteCount":
                    return new ContosoRetail.SharedKernel.DataAccess.DataLoader.RemoteGrouping.RemoteCountAggregator<T>(_accessor);
                case "remoteAvg":
                    return new ContosoRetail.SharedKernel.DataAccess.DataLoader.Aggregation.AvgAggregator<T>(_accessor, new ContosoRetail.SharedKernel.DataAccess.DataLoader.RemoteGrouping.RemoteCountAggregator<T>(_accessor));

            }

            throw new NotSupportedException();
        }

    }
}
