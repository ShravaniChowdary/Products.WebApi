using AutoMapper;
using Products.Api.Models;
using Products.Api.Repositories;
using Products.WebAPI.Models.Domain;

namespace Products.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }
        public async Task<bool> CreateAsync(ProductDto model)
        {
            Product product = this.mapper.Map<Product>(model);
            return await this.productRepository.CreateAsync(product);
        }

        public async Task<string> DecrementStockAsync(int productId, int quantity)
        {
            var product = await this.productRepository.GetByIdAsync(productId);
            if(product != null)
            {
                if(quantity > product.Stock)
                {
                    return $"Insufficient Stock, Avaiable stock: {product.Stock}, Requested: {quantity}";
                }
                product.Stock -= quantity;
                return await this.productRepository.UpdateAsync(product) ? $"Successfully decremented product stock! Available stock = {product.Stock}" : "Failed to decrement the product stock";
            }
            return $"Product with ID '{productId}' does not exist, please check";
        }

        public async Task<bool> DeleteAsync(int productId) => await this.productRepository.DeleteAsync(productId);

        public async Task<IEnumerable<Product>> GetAllAsync() => await this.productRepository.GetAllAsync();

        public async Task<Product?> GetByIdAsync(int productId) => await this.productRepository.GetByIdAsync(productId);

        public async Task<string> IncrementStockAsync(int productId, int quantity)
        {
            var product = await this.productRepository.GetByIdAsync(productId);
            if (product != null)
            {
                product.Stock += quantity;
                return await this.productRepository.UpdateAsync(product) ? $"Successfully incremented product stock! Available stock = {product.Stock}" : "Failed to decrement the product stock";
            }
            return $"Product with ID '{productId}' does not exist, please check";
        }

        public async Task<bool> UpdateAsync(int productId, ProductDto model)
        {
            var product = await GetByIdAsync(productId);
            if(product != null)
            {
                this.mapper.Map(model, product);
                product.UpdatedAt = DateTime.UtcNow;
                return await this.productRepository.UpdateAsync(product);
            }
            return false;
        }
    }
}
