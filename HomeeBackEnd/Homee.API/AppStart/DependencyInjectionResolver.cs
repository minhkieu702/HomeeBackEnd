using Homee.BusinessLayer.IServices;
using Homee.BusinessLayer.Services;
using Homee.DataLayer.Models;
using Homee.Repositories.IRepositories;
using Homee.Repositories.Repositories;

namespace Homee.API.AppStart
{
    public static class DependencyInjectionResolver
    {
        public static void ConfigDI(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddSingleton(typeof(HomeeDbContext), typeof(HomeeDbContext));

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPlaceService, PlaceService>();
            services.AddScoped<IContractService, ContractService>();
            services.AddScoped<IInteriorService, InteriorService>();

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IPlaceRepository, PlaceRepository>();
            services.AddScoped<IContractRepository, ContractRepository>();
            services.AddScoped<IConversationRepository, ConversationRepository>();
            services.AddScoped<IFavoritePostRepository, FavoritePostRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IInteriorRepository, InteriorRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        }
    }
}
