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
            #region Account
            CreateMap<Account, AccountRequest>().ReverseMap();
            CreateMap<AccountResponse, Account>().ReverseMap();
            #endregion
            
            #region Category
            CreateMap<CategoryRequest, Category>().ReverseMap();
            CreateMap<CategoryResponse, Category>().ReverseMap();
            #endregion

            #region Contract
            CreateMap<Contract, ContractRequest>().ReverseMap();
            CreateMap<Contract, ContractResponse>().ReverseMap();
            #endregion

            #region FavoritePost
            CreateMap<FavoritePostRequest, FavoritePost>().ReverseMap();
            CreateMap<FavoritePost, FavoritePostResponse>().ReverseMap();
            #endregion

            #region Notification
            CreateMap<Notification, NotificationRequest>().ReverseMap();
            CreateMap<Notification, NotificationResponse>().ReverseMap();
            #endregion

            #region Order
            CreateMap<Order, OrderRequest>().ReverseMap();
            CreateMap<Order, OrderResponse>().ReverseMap();
            #endregion

            #region Place
            CreateMap<Place, PlaceResponse>()
                //.ForMember(dest => dest.Direction, opt => opt.MapFrom(src => ((PlaceDirection)src.Direction).ToString()))
                .ReverseMap();
            CreateMap<PlaceRequest, Place>().ReverseMap();
            #endregion

            #region Post
            CreateMap<Post, PostRequest>().ReverseMap();
            CreateMap<PostResponse, Post>().ReverseMap();
            #endregion

            #region Subscription
            CreateMap<Subscription, SubscriptionRequest>().ReverseMap();
            CreateMap<Subscription, SubscriptionResponse>().ReverseMap();
            #endregion

            #region CategoryPlace
            CreateMap<CategoryPlace, CategoryPlaceResponse>().ReverseMap();
            #endregion
        }
    }
}
