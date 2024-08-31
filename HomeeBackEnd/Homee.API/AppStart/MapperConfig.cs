using AutoMapper;
using Homee.BusinessLayer.RequestModels;
using Homee.DataLayer;

namespace Homee.API.AppStart
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            #region Category
            CreateMap<CategoryRequest, Category>().ReverseMap();
            #endregion
        }
    }
}
