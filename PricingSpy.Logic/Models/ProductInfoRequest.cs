namespace PricingSpy.Logic.Models
{
    public class ProductInfoRequest
    {
        public string ProductName { get; set; }
        public StoreMetadata[] StoresMetadata { get; set; }
    }

    public class StoreMetadata
    {
        public string StoreName { get; set; }
        public string ProductUrl { get; set; }
        public string ExtractorType { get; set; }
    }
}
