using Bookshop.Models;

namespace Bookshop.Services.Interfaces
{
    public interface IProductsService
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product> GetProductAsync(int id);
		Task UpdateProductAsync(Product product);
		Task<Product> CreateProductAsync(Product product);
		Task DeleteProductAsync(int id);
    }
}
