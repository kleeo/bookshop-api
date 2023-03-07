using Bookshop.Data;
using Bookshop.Helpers;
using Bookshop.Models;
using Bookshop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Services
{
	public class StoreService : IStoreService
	{
		private BookshopDbContext _dbContext;

		public StoreService(BookshopDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<StoreItem> GetStoreItemAsync(int id)
		{
			if (_dbContext.StoreItems == null)
			{
				return null;
			}

			var storeItem = await _dbContext.StoreItems.FindAsync(id);
			return storeItem;
		}

		public async Task<List<StoreItem>> GetStoreItemsAsync()
		{
			var storeItems = await _dbContext.StoreItems.ToListAsync();
			return storeItems;
		}

		public async Task UpdateStoreItemAsync(StoreItem storeItem)
		{
			_dbContext.Entry(storeItem).State = EntityState.Modified;
			try
			{
				await _dbContext.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!(_dbContext.StoreItems?.Any(u => u.Id == storeItem.Id)).GetValueOrDefault())
				{
					throw new InvalidOperationException(string.Format(Constants.StoreItemWithIdDoesntExist, storeItem.Id));
				}
				else
				{
					throw;
				}
			}
		}

		public async Task<StoreItem> CreateStoreItemAsync(StoreItem storeItem)
		{
			await _dbContext.StoreItems.AddAsync(storeItem);
			await _dbContext.SaveChangesAsync();
			return storeItem;
		}

		public async Task DeleteStoreItemAsync(int id)
		{
			var existingStoreItem = await _dbContext.StoreItems.FindAsync(id);
			if (existingStoreItem == null)
			{
				throw new InvalidOperationException(string.Format(Constants.StoreItemWithIdDoesntExist, id));
			}

			_dbContext.StoreItems.Remove(existingStoreItem);
			await _dbContext.SaveChangesAsync();
		}
	}
}
