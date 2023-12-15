using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace KayeeDictionaryServiceDemo.Models
{
    public class FieldUsageFile : HeadingAndDescription
    {
        public FileField File { get; set; } = default!;
    }
}
