using Homee.BusinessLayer.IServices;
using Homee.BusinessLayer.Services;
using Homee.Repositories.IRepositories;
using Homee.Repositories.Repositories;

namespace Homee.API.AppStart
{
    public static class DependencyInjectionResolver
    {
        public static void ConfigDI(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
        }
    }
}
