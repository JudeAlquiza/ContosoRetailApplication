using System;
using System.Collections.Generic;
using System.Linq;

namespace ContosoRetail.SharedKernel.DataAccess.DataLoader
{
    class GroupHelper<T>
    {
        readonly static object NULL_KEY = new object();

        IAccessor<T> _accessor;

        public GroupHelper(IAccessor<T> accessor)
        {
            _accessor = accessor;
        }

        public List<ContosoRetail.SharedKernel.DataAccess.DataLoader.Group> Group(IEnumerable<T> data, IEnumerable<GroupingInfo> groupInfo)
        {
            var groups = Group(data, groupInfo.First());

            if (groupInfo.Count() > 1)
            {
                groups = groups
                    .Select(g => new ContosoRetail.SharedKernel.DataAccess.DataLoader.Group
                    {
                        Key = g.Key,
                        Items = Group(g.Items.Cast<T>(), groupInfo.Skip(1))
                    })
                    .ToList();
            }

            return groups;
        }


        List<ContosoRetail.SharedKernel.DataAccess.DataLoader.Group> Group(IEnumerable<T> data, GroupingInfo groupInfo)
        {
            var groupsIndex = new Dictionary<object, ContosoRetail.SharedKernel.DataAccess.DataLoader.Group>();
            var groups = new List<ContosoRetail.SharedKernel.DataAccess.DataLoader.Group>();

            foreach (var item in data)
            {
                var groupKey = GetKey(item, groupInfo);
                var groupIndexKey = groupKey ?? NULL_KEY;

                if (!groupsIndex.ContainsKey(groupIndexKey))
                {
                    var newGroup = new ContosoRetail.SharedKernel.DataAccess.DataLoader.Group { Key = groupKey };
                    groupsIndex[groupIndexKey] = newGroup;
                    groups.Add(newGroup);
                }

                var group = groupsIndex[groupIndexKey];
                if (group.Items == null)
                    group.Items = new List<T>();
                group.Items.Add(item);
            }

            return groups;
        }

        object GetKey(T obj, GroupingInfo groupInfo)
        {
            var memberValue = _accessor.Read(obj, groupInfo.Selector);

            var intervalString = groupInfo.GroupInterval;
            if (String.IsNullOrEmpty(intervalString) || memberValue == null)
                return memberValue;

            if (Char.IsDigit(intervalString[0]))
            {
                var number = Convert.ToDecimal(memberValue);
                var interval = Decimal.Parse(intervalString);
                return number - number % interval;
            }

            switch (intervalString)
            {
                case "year":
                    return Convert.ToDateTime(memberValue).Year;
                case "quarter":
                    return (Convert.ToDateTime(memberValue).Month + 2) / 3;
                case "month":
                    return Convert.ToDateTime(memberValue).Month;
                case "day":
                    return Convert.ToDateTime(memberValue).Day;
                case "dayOfWeek":
                    return (int)Convert.ToDateTime(memberValue).DayOfWeek;
                case "hour":
                    return Convert.ToDateTime(memberValue).Hour;
                case "minute":
                    return Convert.ToDateTime(memberValue).Minute;
                case "second":
                    return Convert.ToDateTime(memberValue).Second;
            }

            throw new NotSupportedException();
        }
    }
}
