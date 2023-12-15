using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace KayeeDictionaryServiceDemo.Models
{
    public class FieldUsageCheckbox : HeadingAndDescription
    {
        public CheckboxField Checkbox { get; set; } = default!;

        public CheckboxField Checkbox2 { get; set; } = default!;
    }
}
