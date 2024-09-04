using AutoMapper;
using Homee.BusinessLayer.Commons;
using Homee.DataLayer.RequestModels;
using Homee.DataLayer.ResponseModels;
using Homee.DataLayer.Models;

namespace Homee.API.AppStart
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            #region Category
            CreateMap<CategoryRequest, Category>().ReverseMap();
            #endregion

            #region Place
            CreateMap<Place, PlaceResponse>()
                .ForMember(dest => dest.Direction, opt => opt.MapFrom(src => ((PlaceDirection)src.Direction).ToString()))
                .ReverseMap();
            CreateMap<PlaceRequest, Place>().ReverseMap();
            #endregion

            #region Account
            CreateMap<Account, AccountRequest>().ReverseMap();

            #endregion
        }
    }
}
