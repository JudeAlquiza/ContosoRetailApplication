using AutoMapper;
using BC_ContosoRecordsModule.Application.DTOs;
using BC_ContosoRecordsModule.Application.Queries.Interfaces;
using BC_ContosoRecordsModule.Core.Entities;
using BC_ContosoRecordsModule.DataAccess.Repositories.Interfaces;
using ContosoRetail.SharedKernel.ValueObjects.DataRequestOptions.Grouping;
using ContosoRetail.SharedKernel.ValueObjects.DataRequestOptions.LoadOptions;
using ContosoRetail.SharedKernel.ValueObjects.DataRequestOptions.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;

namespace BC_ContosoRecordsModule.Application.Queries
{
    public class GetCustomerOrdersDTOsWithOptionsQuery : IGetCustomerOrdersDTOsWithOptionsQuery
    {
        private ICustomerOrdersRepository _customerOrdersRepo;
        private IMapper _mapper;

        public GetCustomerOrdersDTOsWithOptionsQuery(ICustomerOrdersRepository customerOrdersRepo, IMapper mapper)
        {
            _customerOrdersRepo = customerOrdersRepo;
            _mapper = mapper;
        }

        public dynamic Execute(DataSourceLoadOptionsBase options)
        {
            IQueryable<CustomerOrders> entities = _customerOrdersRepo.GetAll();

            if (entities != null)
            {
                int totalCount = entities.Count();

                // TODO: need to improve this to check and execute only when needed.
                decimal totalAmount = entities.Sum(e => e.Amount);

                if (totalCount != 0)
                {
                    IQueryable<CustomerOrders> responseEntities;
                    List<Group> groupedResponseEntities;

                    // sorting
                    responseEntities = Sort(
                                entities, 
                                options.Sort != null 
                                    ? options.Sort : 
                                      new SortingInfo[] { new SortingInfo { Selector = "Region", Desc = false } }
                                );
                    
                    if (options.Group != null)
                    {
                        // grouping
                        groupedResponseEntities = Group(responseEntities, options.Group);

                        // TODO: paging (for single column grouping only for now)
                        groupedResponseEntities = GroupPage(groupedResponseEntities, entities, options.Group[0], options.Skip, options.Take);

                        // TODO: map to dto (for single column grouping only for now)
                        foreach (Group group in groupedResponseEntities)
                        {
                            group.items = _mapper.Map<List<CustomerOrdersDTO>>(group.items);
                        }

                        var responseData = new 
                            { 
                                Items = groupedResponseEntities, 
                                TotalCount = totalCount, 
                                GroupCount = groupedResponseEntities.Count, 
                                TotalAmount = totalAmount 
                            };

                        return responseData;
                    }
                    else
                    {
                        // paging
                        responseEntities = responseEntities.Skip(options.Skip).Take(options.Take == 0 ? 100 : options.Take);

                        // map to dto
                        IEnumerable<CustomerOrdersDTO> dtos = _mapper.Map<IEnumerable<CustomerOrdersDTO>>(responseEntities.ToList());

                        var responseData = new 
                            { 
                                Items = dtos, 
                                TotalCount = totalCount, 
                                GroupCount = 0, 
                                TotalAmount = totalAmount 
                            };

                        return responseData;
                    }                    
                }
            }

            return entities;
        }

        private IQueryable<CustomerOrders> Sort(IQueryable<CustomerOrders> customerOrders, SortingInfo[] sort)
        {
            customerOrders = customerOrders.AsQueryable();

            if (sort != null || sort.Length != 0)
            {
                string columnName = string.Empty;
                bool desc;
                string sortString = string.Empty;

                for (int i = 0; i < sort.Length; i++)
                {
                    columnName = char.ToUpper(sort[i].Selector[0]) + sort[i].Selector.Substring(1);
                    desc = sort[i].Desc;
                    sortString += $"{columnName} " + (desc ? "descending" : string.Empty) + ",";
                }

                sortString = sortString.Remove(sortString.Length - 1);
                customerOrders = ((IOrderedQueryable<CustomerOrders>)customerOrders).OrderBy(sortString);
            }

            return customerOrders;
        }

        private List<Group> Group(IQueryable<CustomerOrders> data, IEnumerable<GroupingInfo> groupInfo)
        {
            var groups = Group(data, groupInfo.First());

            if (groupInfo.Count() > 1)
            {
                groups = groups
                    .Select(g => new Group
                    {
                        key = g.key,
                        items = Group(g.items.AsQueryable().Cast<CustomerOrders>(), groupInfo.Skip(1))
                    })
                    .ToList();
            }

            return groups;
        }

        private List<Group> Group(IQueryable<CustomerOrders> customerOrders, GroupingInfo groupInfo)
        {
            var columnName = char.ToUpper(groupInfo.Selector[0]) + groupInfo.Selector.Substring(1);
            var vm = Expression.Parameter(typeof(CustomerOrders), "vm");
            var prop = Expression.PropertyOrField(vm, columnName);
            var lambda = Expression.Lambda<Func<CustomerOrders, string>>(prop, vm);

            return customerOrders
                        .GroupBy(lambda)
                        .OrderBy(g => g.Key)
                        .Select(g =>
                            new Group
                            {
                                key = g.Key,
                                count = g.Count(),
                                isContinuationOnNextPage = false
                            })
                        .ToList();
        }

        // TODO: this method only works for single column grouping only for now.
        private List<Group> GroupPage(List<Group> groupedCustomerOrders, IQueryable<CustomerOrders> customerOrders, GroupingInfo groupInfo, int skip, int take)
        {
            int emptyRows = take;
            var columnName = char.ToUpper(groupInfo.Selector[0]) + groupInfo.Selector.Substring(1);

            // paging
            foreach (Group groupItem in groupedCustomerOrders)
            {
                var vm = Expression.Parameter(typeof(CustomerOrders), "vm");
                var prop = Expression.Property(vm, columnName);
                var val = Expression.Constant(groupItem.key.ToString());
                BinaryExpression equal = Expression.Equal(prop, val);

                if (groupItem.count + 1 > emptyRows)
                {
                    groupItem.items = customerOrders
                                        .Where(Expression.Lambda<Func<CustomerOrders, bool>>(equal, vm))
                                        .Take(emptyRows - 1)
                                        .ToList();

                    groupItem.isContinuationOnNextPage = true;

                    break;
                }
                else
                {
                    groupItem.items = customerOrders
                                        .Where(Expression.Lambda<Func<CustomerOrders, bool>>(equal, vm))
                                        .Take(groupItem.count)
                                        .ToList();

                    emptyRows = emptyRows - 1 - groupItem.count;
                }
            }

            return groupedCustomerOrders;
        }
    }
}
