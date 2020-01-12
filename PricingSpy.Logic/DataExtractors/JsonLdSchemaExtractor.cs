using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Globalization;
using PricingSpy.Logic.Models;

namespace PricingSpy.Logic.DataExtractors
{
    public class JsonLdSchemaExtractor : IDataExtractor
    {
        private bool IsProductAvailable(string availability)
        {
            List<string> availableStates = new List<string>
            {
                "http://schema.org/InStock",
                "http://schema.org/LimitedAvailability"
            };
            return availableStates.Contains(availability);
        }

        public async Task<ProductInfo> GetProductInfoAsync(string pageUrl)
        {
            var document = await ContentReader.ReadDocumentAsync(pageUrl);

            var dataScripts = document.Scripts.Where(s => s.Type == "application/ld+json").ToList();

            foreach (var script in dataScripts)
            {
                var dictProduct = JsonSerializer.Deserialize<Dictionary<string, object>>(script.Text);
                var type = dictProduct["@type"].ToString();

                if (type.ToLower() == "product")
                {
                    var dictOffers = JsonSerializer.Deserialize<Dictionary<string, object>>(dictProduct["offers"].ToString());

                    var name = dictProduct["name"].ToString();
                    var price = dictOffers["price"].ToString();
                    var availability = dictOffers["availability"].ToString();

                    return new ProductInfo
                    {
                        Name = name,
                        Price = decimal.Parse(price, new CultureInfo("en-us")),
                        Available = IsProductAvailable(availability)
                    };
                }
            }

            return null;
        }
    }
}
