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
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient(typeof(HomeedbContext), typeof(HomeedbContext));

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPlaceService, PlaceService>();
            services.AddScoped<IContractService, ContractService>();
            services.AddScoped<IFavoritePostService, FavoritePostService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<IMailService, MailService>();

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IPlaceRepository, PlaceRepository>();
            services.AddScoped<IContractRepository, ContractRepository>();
            services.AddScoped<IFavoritePostRepository, FavoritePostRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        }
    }
}
