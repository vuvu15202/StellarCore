using AutoMapper;
using CategoryService.Application.DTOs;
using CategoryService.Domain.Entities;

namespace CategoryService.Application.Mappings
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<CategoryType, CategoryTypeDTO>().ReverseMap();
            CreateMap<BaseCategory, CategoryDTO>().ReverseMap();
            CreateMap<CommonCategory, CategoryDTO>().IncludeBase<BaseCategory, CategoryDTO>().ReverseMap();
            CreateMap<BloodTypeCategory, CategoryDTO>().IncludeBase<BaseCategory, CategoryDTO>().ReverseMap();
            CreateMap<EthnicCategory, CategoryDTO>().IncludeBase<BaseCategory, CategoryDTO>().ReverseMap();
            CreateMap<MaritalStatusCategory, CategoryDTO>().IncludeBase<BaseCategory, CategoryDTO>().ReverseMap();
            CreateMap<NationCategory, CategoryDTO>().IncludeBase<BaseCategory, CategoryDTO>().ReverseMap();
            CreateMap<RelationShipCategory, CategoryDTO>().IncludeBase<BaseCategory, CategoryDTO>().ReverseMap();
            CreateMap<ReligionCategory, CategoryDTO>().IncludeBase<BaseCategory, CategoryDTO>().ReverseMap();
            CreateMap<SexualCategory, CategoryDTO>().IncludeBase<BaseCategory, CategoryDTO>().ReverseMap();
            CreateMap<UserManualCategory, CategoryDTO>().IncludeBase<BaseCategory, CategoryDTO>().ReverseMap();
            CreateMap<FaqCategory, CategoryDTO>().IncludeBase<BaseCategory, CategoryDTO>().ReverseMap();
            CreateMap<ProvinceCategory, CategoryDTO>().IncludeBase<BaseCategory, CategoryDTO>().ReverseMap();
            CreateMap<DistrictCategory, CategoryDTO>().IncludeBase<BaseCategory, CategoryDTO>().ReverseMap();
            CreateMap<WardCategory, CategoryDTO>().IncludeBase<BaseCategory, CategoryDTO>().ReverseMap();

        }
    }
}
