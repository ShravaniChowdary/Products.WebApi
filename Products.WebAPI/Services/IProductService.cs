using Products.Api.Models;
using Products.WebAPI.Models.Domain;

namespace Products.Api.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int productId);
        Task<bool> CreateAsync(ProductDto model);
        Task<bool> UpdateAsync(int productId, ProductDto model);
        Task<bool> DeleteAsync(int productId);
        Task<string> IncrementStockAsync(int productId, int quantity);
        Task<string> DecrementStockAsync(int productId, int quantity);

    }
}
