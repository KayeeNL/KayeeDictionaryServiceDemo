namespace KayeeDictionaryServiceDemo.Configuration
{
    public class DictionaryServiceOptionsConfiguration
    {
        public static readonly string Key = "DictionaryService";
        public string SitecoreInstanceUri { get; set; } = default!;
        public string ApiKey { get; set; } = default!;
        public string SiteName { get; set; } = default!;
        public int CacheDuration { get; set; } = default!;
    }
}
