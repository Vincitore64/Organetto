using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Organetto.Web.Configuration.Swagger.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        /// </summary>
        /// <param name="services"></param>
        public static void AddAppSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            //services.AddSwaggerGen(c =>
            //{
            //    c.AddAuthorization();
            //});
            services.AddSwaggerGenNewtonsoftSupport();
        }

        public static void AddAuthorization(this SwaggerGenOptions c)
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });
        }

        public static IApplicationBuilder UseAppSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }

        public static IApplicationBuilder UseAppSwagger(this IApplicationBuilder app, string prefix, string name, IWebHostEnvironment webHostEnvironment)
        {
            var prefixWithEnvironment = prefix;
            app.UseSwagger(options =>
            {
                //options.RouteTemplate = Path.Combine(prefixWithEnvironment, "swagger", "{documentname}", "swagger.json"); // $"{prefixWithEnvironment}/swagger/{{documentname}}/swagger.json";
                options.RouteTemplate = $"{prefixWithEnvironment}/swagger/{{documentname}}/swagger.json";
                //options.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                //{
                //    swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = new SwaggerUri(httpReq, webHostEnvironment).AbsoluteUri } };
                //});
            });

            app.UseSwaggerUI(options =>
            {
                //var endpoint = Path.Combine(prefixWithEnvironment, "swagger", "v1", "swagger.json");
                options.SwaggerEndpoint($"/{prefixWithEnvironment}/swagger/v1/swagger.json", name); // $"/{prefixWithEnvironment}/swagger/v1/swagger.json"
                options.RoutePrefix = $"{prefixWithEnvironment}/swagger"; // $"{prefixWithEnvironment}/swagger"
            });
            return app;
        }
    }
}
