using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using Markdig;
using System.Reflection;

namespace FileConverter.Services.Converters {
    public class MarkdownPDFConverter {
        public byte[] Convert(string markdownText) {
            var pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UsePipeTables()
                .UseGridTables()
                .UseListExtras()
                .UseTaskLists()
                .UseAutoLinks()
                .UseEmojiAndSmiley()
                .Build();

            string html = Markdown.ToHtml(markdownText, pipeline);

            html = $@"
                <html>
                    <head>
                        <style>
                            {GetStyles()}
                        </style>
                    </head>
                    <body>
                        {html}
                    </body>
                </html>
            ";

            using var memoryStream = new MemoryStream();
            var converterProperties = new ConverterProperties();
            converterProperties.SetCreateAcroForm(true);
            converterProperties.SetFontProvider(new DefaultFontProvider(true, true, true));

            HtmlConverter.ConvertToPdf(html, memoryStream, converterProperties);

            return memoryStream.ToArray();
        }

        private string? GetStyles() {
            string stylesPatch = "FileConverter.Services.Converters.Resources.styles.css";

            if (!File.Exists(stylesPatch)) throw new KeyNotFoundException("Styles not found");

            var assembly = Assembly.GetExecutingAssembly();

            var manifestResourceStream = assembly.GetManifestResourceStream(stylesPatch) ?? throw new KeyNotFoundException("Manifest not found");

            using var stream = manifestResourceStream;
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
            
        }
    }
}
