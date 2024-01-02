using KayeeDictionaryServiceDemo.Services.Dictionary.Models;
using Microsoft.Extensions.DependencyInjection;

namespace KayeeDictionaryServiceDemo.Services.Dictionary.Extensions
{
    public static class DictionaryServiceExtensions
    {
        public static IServiceCollection AddDictionaryService(this IServiceCollection services)
        {
            services.AddScoped<IDictionaryService, RestDictionaryService>();
            return services;
        }
    }
}