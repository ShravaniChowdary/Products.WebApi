using Microsoft.EntityFrameworkCore;
using Products.Api.Data;
using Products.WebAPI.Models.Domain;

namespace Products.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext productDbContext;

        public ProductRepository(ProductDbContext productDbContext)
        {
            this.productDbContext = productDbContext;
        }
        public async Task<bool> CreateAsync(Product product)
        {
            await this.productDbContext.Products.AddAsync(product);
            return await this.productDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int productId)
        {
            var existingProduct = await GetByIdAsync(productId);
            if (existingProduct != null)
            {
                this.productDbContext.Products.Remove(existingProduct);
                return await this.productDbContext.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await this.productDbContext.Products.ToListAsync(); 
        }

        public async Task<Product?> GetByIdAsync(int productId)
        {
            return await this.productDbContext.Products.FirstOrDefaultAsync(x => x.Id == productId);
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            this.productDbContext.Products.Update(product);
            return await this.productDbContext.SaveChangesAsync() > 0;
        }
    }
}
