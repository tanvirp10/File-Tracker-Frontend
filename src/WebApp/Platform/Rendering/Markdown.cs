using Ganss.Xss;
using Markdig;

namespace MyApp.WebApp.Platform.Rendering
{
    public static class MarkdownHelper
    {
        public static string MarkdownToHtml(string markdown)
        {
            var pipeline = new MarkdownPipelineBuilder().UseBootstrap().UseAdvancedExtensions().Build();
            var sanitizer = new HtmlSanitizer();
            sanitizer.AllowedAttributes.Add("class");
            return sanitizer.Sanitize(Markdown.ToHtml(markdown, pipeline));
        }
    }
}
