using Bookshop.Models;

namespace Bookshop.Services.Interfaces
{
	public interface IStoreService
	{
		Task<StoreItem> GetStoreItemAsync(int id);
		Task<List<StoreItem>> GetStoreItemsAsync();
		Task UpdateStoreItemAsync(StoreItem storeItem);
		Task<StoreItem> CreateStoreItemAsync(StoreItem user);
		Task DeleteStoreItemAsync(int id);
	}
}
