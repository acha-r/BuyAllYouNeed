using AllYouNeed_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllYouNeed_Services.Interface
{
    public interface IProductService
    {
        Task<Product> GetProductById(string id);
        Task<List<Product>> GetProductBySearch(string text); //text search and pagination
        Task<Product> RegisterProduct(Product product);
        Task UpdateProductInfo(string id, Product merchant);
        Task CheckInStockStatus(string id);
        Task DeleteProduct(string id);
    }
}
