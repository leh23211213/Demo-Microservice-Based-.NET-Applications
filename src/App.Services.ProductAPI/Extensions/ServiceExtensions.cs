using AutoMapper;


namespace App.Services.ProductAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AppServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
            services.AddSingleton(mapper);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            // Add services to the container. 
            services.AddResponseCaching();
            // services.AddControllers(option =>
            //  {
            //      option.CacheProfiles.Add("Default10",
            //         new CacheProfile()
            //         {
            //             Duration = 10
            //         });
            //      //option.ReturnHttpNotAcceptable=true;
            //  }).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
            services.AddControllers();
            return services;
        }
    }
}