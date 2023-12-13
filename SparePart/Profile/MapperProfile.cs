using AutoMapper;
using SparePart.Dto.Request;
using SparePart.Dto.Response;
using SparePart.ModelAndPersistance.Entities;
using SparePart.ModelAndPersistance.Models;
using SparePart.ModelAndPersistance.Repository;

namespace SparePart.Profile
{
    public class MapperProfile : AutoMapper.Profile
    {
        public MapperProfile() 
        {
            // customer
            CreateMap<Dto.RegisterCustomerRequest, ModelAndPersistance.Entities.Customer>();
            CreateMap<Customer, CustomersInfo>();

            CreateMap<QuotationPart, PartsInQuotationList>()
                .ForMember(dest => dest.PartName, opt => opt.MapFrom(src => src.Part.PartName));

            CreateMap<QuotationListForCreation, QuotationList>();
            CreateMap<QuotationListForSubmition, QuotationList>();
            CreateMap<QuotationList, QuotationListForSubmition>();

            CreateMap<Part, PartResponse>()
                .ForMember(dest => dest.NewPrice, opt => opt.MapFrom(src => src.SellingPrice));
        }

    }
}
