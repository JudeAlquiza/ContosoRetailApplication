using AutoMapper;
using BC_ContosoRecordsModule.Application.DTOs;
using ContosoRetail.WebAPI.ViewModels.BC_ContosoRecordsModule_ViewModels;

namespace ContosoRetail.WebAPI.MappingProfile
{
    public class ContosoRetailWebAPIMappingProfile : Profile
    {
        public ContosoRetailWebAPIMappingProfile()
        {
            CreateMap<CustomerOrdersViewModel, CustomerOrdersDTO>().ReverseMap();
        }
    }
}
