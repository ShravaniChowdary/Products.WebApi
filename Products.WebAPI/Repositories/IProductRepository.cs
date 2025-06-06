using Products.WebAPI.Models.Domain;

namespace Products.Api.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int productId);
        Task<bool> CreateAsync(Product product);
        Task<bool> UpdateAsync(Product product); 
        Task<bool> DeleteAsync(int productId);
    }
}
