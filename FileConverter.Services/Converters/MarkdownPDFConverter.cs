using FileConverter.Services.Converters.Base;
using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using Markdig;
using System.Reflection;

namespace FileConverter.Services.Converters {
    public class MarkdownPDFConverter : IConverter {
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

        private string GetStyles() {
            
            string resourcePath = "FileConverter.Services.Converters.Resources.styles.css";

            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(resourcePath)
                                ?? throw new KeyNotFoundException("Styles resource not found");
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

    }
}
