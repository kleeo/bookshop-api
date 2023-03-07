using Bookshop.Data;
using Bookshop.Helpers;
using Bookshop.Models;
using Bookshop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Services
{
	public class UsersService : IUsersService
	{
		private BookshopDbContext _dbContext;

		public UsersService(BookshopDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<User> GetUserAsync(int id)
		{
			if (_dbContext.Users == null)
			{
				return null;
			}

			var user = await _dbContext.Users.FindAsync(id);
			return user;
		}

		public async Task<List<User>> GetUsersAsync()
		{
			var users = await _dbContext.Users.ToListAsync();
			return users;
		}

		public async Task UpdateUserAsync(User user)
		{
			_dbContext.Entry(user).State = EntityState.Modified;
			try
			{
				await _dbContext.SaveChangesAsync();
			}
			catch(DbUpdateConcurrencyException)
			{
				if (!(_dbContext.Users?.Any(u=>u.Id==user.Id)).GetValueOrDefault())
				{
					throw new InvalidOperationException(string.Format(Constants.UserWithIdDoesntExist, user.Id));
				}
				else
				{
					throw;
				}
			}
		}

		public async Task<User> CreateUserAsync(User user)
		{
			await _dbContext.Users.AddAsync(user);
			await _dbContext.SaveChangesAsync();
			return user;
		}

		public async Task DeleteUserAsync(int id)
		{
			var existingUser = await _dbContext.Users.FindAsync(id);
			if (existingUser == null)
			{
				throw new InvalidOperationException(string.Format(Constants.UserWithIdDoesntExist, id));
			}

			_dbContext.Users.Remove(existingUser);
			await _dbContext.SaveChangesAsync();
		}
	}
}
