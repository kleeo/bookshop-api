using Bookshop.Models;

namespace Bookshop.Services.Interfaces
{
	public interface IUsersService
	{
		Task<List<User>> GetUsersAsync();
		Task<User> GetUserAsync(int id);
		Task UpdateUserAsync(User user);
		Task<User> CreateUserAsync(User user);
		Task DeleteUserAsync(int id);
	}
}
