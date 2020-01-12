using PricingSpy.Logic.DataExtractors;
using PricingSpy.Logic.Models;
using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PricingSpy.Logic
{
    public class AppController
    {
        public async Task<string> GetProductsInfoAsync(ProductInfoRequest[] productsInfoRequest)
        {
            StringBuilder sb = new StringBuilder();

            if (productsInfoRequest.Length > 0)
                sb.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            foreach(var productInfoRequest in productsInfoRequest)
            {
                sb.AppendLine($"-----------------------------");
                sb.AppendLine($"Product - {productInfoRequest.ProductName}");
                sb.AppendLine($"-----------------------------");
                foreach (var store in productInfoRequest.StoresMetadata)
                {
                    var dataExtractor = CreateDataExtractorInstance(store.ExtractorType);
                    var productInfo = await dataExtractor.GetProductInfoAsync(store.ProductUrl);

                    var productInfoPrice = productInfo != null && productInfo.Available ? productInfo.Price.ToString("C", new CultureInfo("pt-PT")) : "not available";
                    sb.AppendLine($"{store.StoreName}: {productInfoPrice}");
                }
            }

            return sb.ToString();
        }

        private IDataExtractor CreateDataExtractorInstance(string extractorType)
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            var objectTypeNameToInstantiate = $"{assemblyName}.DataExtractors.{extractorType}, {assemblyName}";
            var objectType = Type.GetType(objectTypeNameToInstantiate);

            return Activator.CreateInstance(objectType) as IDataExtractor;
        }
    }
}
