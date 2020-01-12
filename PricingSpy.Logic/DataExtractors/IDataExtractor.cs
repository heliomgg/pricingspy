using PricingSpy.Logic.Models;
using System.Threading.Tasks;

namespace PricingSpy.Logic.DataExtractors
{
    interface IDataExtractor
    {
        Task<ProductInfo> GetProductInfoAsync(string pageUrl);
    }
}
