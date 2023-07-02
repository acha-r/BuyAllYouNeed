using AllYouNeed_Models.Implementation;
using AllYouNeed_Models.Interface;
using AllYouNeed_Services.Implementation;
using AllYouNeed_Services.Interface;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AllYouNeed_API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
            => services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
            builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
            });

        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAllYouNeedRepo>(x =>
            x.GetRequiredService<IOptions<AllYouNeedRepo>>().Value);

            services.AddSingleton<IMongoClient>(x =>
            new MongoClient(configuration.GetValue<string>("AllYouNeedRepo:ConnectionString")));

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IMerchantServices, MerchantService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICartServices, CartServices>();
            services.AddTransient<IPaystackPaymentService, PaymentService>();
        }
    }
}
