using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using KayeeDictionaryServiceDemo.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KayeeDictionaryServiceDemo.Services.Dictionary.Models
{
    public class RestDictionaryService : IDictionaryService
    {
        private readonly IMemoryCache _cache;
        private readonly DictionaryServiceOptionsConfiguration _dictionaryOptions;
        private readonly HttpClient _httpClient;

        public RestDictionaryService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _dictionaryOptions = configuration.GetSection(DictionaryServiceOptionsConfiguration.Key)
                .Get<DictionaryServiceOptionsConfiguration>();
            _cache = serviceProvider.GetRequiredService<IMemoryCache>();
            _httpClient = new HttpClient();
        }

        public async Task<Dictionary<string, string>> FetchDictionaryData()
        {
            const string language = "en";
            var cacheKey = ConstructCacheKey(language);

            if (_cache.TryGetValue(cacheKey, out Dictionary<string, string> cachedValue))
            {
                Console.WriteLine($"Using cached dictionary data for {language} {_dictionaryOptions.SiteName}");
                return cachedValue;
            }

            Console.WriteLine($"Fetching dictionary data for {language} {_dictionaryOptions.SiteName}");
            var data = await FetchData<DictionaryEndpointResponse>(GetUrl(language));
            SetCacheValue(cacheKey, data.Phrases);
            return data.Phrases;
        }

        private void SetCacheValue(string key, Dictionary<string, string> value)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(_dictionaryOptions.CacheDuration));

            _cache.Set(key, value, cacheEntryOptions);
        }

        private string ConstructCacheKey(string language)
        {
            return $"dictionary-{_dictionaryOptions.SiteName.ToLowerInvariant()}-{language}";
        }

        private async Task<T> FetchData<T>(string url)
        {
            Console.WriteLine($"Fetch data from url {url}");
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                return await JsonSerializer.DeserializeAsync<T>(responseStream);
            }
        }

        private string GetUrl(string language)
        {
            return
                $"{_dictionaryOptions.SitecoreInstanceUri}sitecore/api/jss/dictionary/{_dictionaryOptions.SiteName}/{language}?sc_apikey={_dictionaryOptions.ApiKey}";
        }
    }
}