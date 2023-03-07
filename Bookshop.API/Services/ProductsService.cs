using Bookshop.Data;
using Bookshop.Helpers;
using Bookshop.Models;
using Bookshop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Bookshop.Services
{
	public class ProductsService : IProductsService
	{
		private BookshopDbContext _dbContext;

		public ProductsService(BookshopDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Product> GetProductAsync(int id)
		{
			if (_dbContext.Products == null)
			{
				return null;
			}

			var product = await _dbContext.Products.FindAsync(id);
			return product;
		}

		public async Task<List<Product>> GetProductsAsync()
		{
			var products = await _dbContext.Products.ToListAsync();
			return products;
		}

		public async Task UpdateProductAsync(Product product)
		{
			var existingProduct = _dbContext.Products.FirstOrDefault(p => p.Id == product.Id);
			if (existingProduct == null)
			{
				throw new InvalidOperationException(string.Format(Constants.ProductWithIdDoesntExist, product.Id));
			}
			existingProduct.Author = product.Author;
			existingProduct.Name = product.Name;
			existingProduct.Description = product.Description;
			existingProduct.Price = product.Price;
			existingProduct.ImagePath = product.ImagePath ?? existingProduct.ImagePath;
			await _dbContext.SaveChangesAsync();
		}

		public async Task<Product> CreateProductAsync(Product product)
		{
			await _dbContext.Products.AddAsync(product);
			await _dbContext.SaveChangesAsync();
			return product;
		}

		public async Task DeleteProductAsync(int id)
		{
			var existingProduct = await _dbContext.Products.FindAsync(id);
			if (existingProduct == null)
			{
				throw new InvalidOperationException(string.Format(Constants.ProductWithIdDoesntExist, id));
			}

			_dbContext.Products.Remove(existingProduct);
			await _dbContext.SaveChangesAsync();
		}
	}
}
