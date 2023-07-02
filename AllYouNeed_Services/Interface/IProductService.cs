using AllYouNeed_Models.DTOS.Requests;
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
        Task PreSeedProducts();
        Task<ProductRegistration> GetProductById(string id);
        Task<List<ProductRegistration>> GetProductBySearch(string keyword); //text search and pagination
        Task<ProductRegistration> RegisterProduct(ProductRegistration product);
        Task UpdateProductInfo(string id, ProductRegistration product);
        Task<bool> UpdateInStockStatus(string id);
        Task DeleteProduct(string id);
    }
}
