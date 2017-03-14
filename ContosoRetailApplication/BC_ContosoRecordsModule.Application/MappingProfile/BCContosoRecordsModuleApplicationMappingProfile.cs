using AutoMapper;
using BC_ContosoRecordsModule.Application.DTOs;
using BC_ContosoRecordsModule.Core.Entities;

namespace BC_ContosoRecordsModule.Application.MappingProfile
{
    public class BCContosoRecordsModuleApplicationMappingProfile : Profile
    {
        public BCContosoRecordsModuleApplicationMappingProfile()
        {
            CreateMap<CustomerOrdersDTO, CustomerOrders>().ReverseMap();
        }
    }
}
