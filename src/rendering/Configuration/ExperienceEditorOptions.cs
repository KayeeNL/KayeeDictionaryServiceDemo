namespace KayeeDictionaryServiceDemo.Configuration
{
    public class ExperienceEditorConfiguration
    {
        public static readonly string Key = "RenderingEngine:ExperienceEditor";
        public static readonly string JssEditingSecretKey = "JSS_EDITING_SECRET";

        public string Endpoint { get; set; } = default!;

        public string? ApplicationUrl { get; set; }
    }
}
