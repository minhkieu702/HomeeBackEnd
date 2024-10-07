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

            #region Room
            CreateMap<RoomRequest, Room>()
                .ForMember(dest => dest.RoomId, opt => opt.Ignore())
                .ForMember(dest => dest.Place, opt => opt.Ignore())
                .ForMember(dest => dest.Posts, opt => opt.Ignore());
            CreateMap<Room, RoomResponse>()
                .AfterMap((src, dest) =>
                {
                    dest.Direction = src.Direction switch
                    {
                        1 => "Bắc",
                        2 => "Đông Bắc",
                        3 => "Đông",
                        4 => "Đông Nam",
                        5 => "Nam",
                        6 => "Tây Nam",
                        7 => "Tây",
                        8 => "Tây Bắc",
                        _=> "Chưa có thông tin"
                    };
                })
                .AfterMap((src, dest) =>
                {
                    dest.InteriorStatus = src.InteriorStatus switch
                    {
                        1 => "Trống",
                        2 => "Cơ bản",
                        3 => "Đầy đủ",
                        4 => "Nội thất cao cấp",
                        _ => "Chưa có thông tin"
                    };
                })
                .AfterMap((src, dest) =>
                {
                    dest.Type = src.Type switch
                    {
                        1 => "Nhà trọ",
                        2 => "Chung cư",
                        3 => "Nhà nguyên căn",
                        _ => "Chưa có thông tin"
                    };
                });
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

            #region Room Post
            // Map RoomPostRequest to Room
            CreateMap<RoomPostRequest, Room>()
                //.ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.RoomName))
                //.ForMember(dest => dest.Direction, opt => opt.MapFrom(src => src.Direction))
                //.ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area))
                //.ForMember(dest => dest.InteriorStatus, opt => opt.MapFrom(src => src.InteriorStatus))
                //.ForMember(dest => dest.IsRented, opt => opt.MapFrom(src => src.IsRented))
                //.ForMember(dest => dest.RentAmount, opt => opt.MapFrom(src => src.RentAmount))
                //.ForMember(dest => dest.WaterAmount, opt => opt.MapFrom(src => src.WaterAmount))
                //.ForMember(dest => dest.ElectricAmount, opt => opt.MapFrom(src => src.ElectricAmount))
                //.ForMember(dest => dest.ServiceAmount, opt => opt.MapFrom(src => src.ServiceAmount))
                .ForMember(dest => dest.IsBlock, opt => opt.Ignore())
                .ForMember(dest => dest.Place, opt => opt.Ignore())  
                .ForMember(dest => dest.Posts, opt => opt.Ignore()); 

            // Map RoomPostRequest to Post
            CreateMap<RoomPostRequest, Post>()
                //.ForMember(dest => dest.PostedDate, opt => opt.MapFrom(src => src.PostedDate))
                //.ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note))
                //.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                //.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Images, opt => opt.Ignore()) 
                .ForMember(dest => dest.IsBlock, opt => opt.Ignore())
                .ForMember(dest => dest.FavoritePosts, opt => opt.Ignore()) 
                .ForMember(dest => dest.Room, opt => opt.Ignore());

            #endregion

            #region Image
            CreateMap<ImageRequest, Image>();
            #endregion
        }
    }
}
