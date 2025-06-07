using Microsoft.Extensions.DependencyInjection;
using Organetto.UseCases.Boards.Services;
using System.Reflection;

namespace Organetto.UseCases.Configuration.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(typeof(BoardMappingProfile).Assembly);
            //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipeline<,>));

            return services;
        }
    }

}
