using System.Collections.Generic;
using System.Threading.Tasks;

namespace KayeeDictionaryServiceDemo.Services.Dictionary.Models
{
    public interface IDictionaryService
    {
        Task<Dictionary<string, string>> FetchDictionaryData();
    }
}