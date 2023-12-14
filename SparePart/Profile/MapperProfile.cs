using AutoMapper;
using SparePart.Dto.Request;
using SparePart.Dto.Response;
using SparePart.ModelAndPersistance.Dtos;
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

            CreateMap<QuotePartAdd, QuotationPart>();
            CreateMap<int, QuotationPart>();


            CreateMap<Part, PartForAdditionalInfoDto>()
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.SupplierName))
            .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Storages.Select(s => s.Warehouse.WarehouseName).FirstOrDefault()))
            .ForMember(dest => dest.TotalQuantity, opt => opt.MapFrom(src => src.Storages.Sum(s => s.Quantity)));

        //    CreateMap<Part, PartResponse>()
        //        .ForMember(dest => dest.NewPrice, opt => opt.MapFrom(src => src.SellingPrice))
        //        .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.SupplierName))
        //        .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Storages.FirstOrDefault().Warehouse.WarehouseName))
        //        .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Storages.FirstOrDefault().Quantity))
        //        ;
        }

    }
}
