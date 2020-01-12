using AngleSharp;
using AngleSharp.Dom;
using System.Net.Http;
using System.Threading.Tasks;

namespace PricingSpy.Logic
{
    public class ContentReader
    {
        private static readonly HttpClient _client = new HttpClient();

        public static async Task<string> ReadPageContentAsync(string url)
        {
            //var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36";
            var userAgent = "C# Console App";

            _client.DefaultRequestHeaders.Add("User-Agent", userAgent);
            return await _client.GetStringAsync(url);
        }

        public static async Task<IDocument> ReadDocumentAsync(string pageUrl)
        {
            var pageContent = await ReadPageContentAsync(pageUrl);

            var config = Configuration.Default;

            //Create a new context for evaluating webpages with the given config
            var context = BrowsingContext.New(config);

            //Create a virtual request to specify the document to load (here from our fixed string)
            return await context.OpenAsync(req => req.Content(pageContent));
        }
    }
}
