using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KayeeDictionaryServiceDemo.Services.Dictionary.Models
{
    public class DictionaryModel
    {
        private readonly Lazy<Task<Dictionary<string, string>>> _dictionaryDataTask;

        public DictionaryModel(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _dictionaryDataTask = new Lazy<Task<Dictionary<string, string>>>(FetchDictionaryDataAsync);
        }

        protected IServiceProvider ServiceProvider { get; }

        private Dictionary<string, string> DictionaryData => _dictionaryDataTask.Value.Result;

        private async Task<Dictionary<string, string>> FetchDictionaryDataAsync()
        {
            var dictionaryService = ServiceProvider.GetService(typeof(IDictionaryService)) as IDictionaryService;
            return await dictionaryService?.FetchDictionaryData() ?? new Dictionary<string, string>();
        }

        public string GetDictionaryPhrase(string dictionaryKey, string defaultValue)
        {
            return DictionaryData.GetValueOrDefault(dictionaryKey, defaultValue);
        }
    }
}