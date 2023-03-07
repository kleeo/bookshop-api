using Bookshop.Data;
using Bookshop.Helpers;
using Bookshop.Models;
using Bookshop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Services
{
	public class BookingsService : IBookingsService
	{
		private BookshopDbContext _dbContext;

		public BookingsService(BookshopDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Booking> GetBookingAsync(int id)
		{
			if (_dbContext.Bookings == null)
			{
				return null;
			}

			var booking = await _dbContext.Bookings.FindAsync(id);
			return booking;
		}

		public async Task<List<Booking>> GetBookingsAsync()
		{
			var bookings = await _dbContext.Bookings.ToListAsync();
			return bookings;
		}

		public async Task UpdateBookingAsync(Booking booking)
		{
			_dbContext.Entry(booking).State = EntityState.Modified;
			try
			{
				await _dbContext.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException ex)
			{
				if (!(_dbContext?.Bookings.Any(b => b.Id == booking.Id)).GetValueOrDefault())
				{
					throw new InvalidOperationException(string.Format(Constants.BookingWithIdDoesntExist, booking.Id));
				}
				else
				{
					throw;
				}
			}
		}

		public async Task<Booking> CreateBookingAsync(Booking booking)
		{
			await _dbContext.Bookings.AddAsync(booking);
			await _dbContext.SaveChangesAsync();
			return booking;
		}

		public async Task DeleteBookingAsync(int id)
		{
			var existingBooking = await _dbContext.Bookings.FindAsync(id);
			if (existingBooking == null)
			{
				throw new InvalidOperationException(string.Format(Constants.BookingWithIdDoesntExist, id));
			}

			_dbContext.Bookings.Remove(existingBooking);
			await _dbContext.SaveChangesAsync();
		}

	}
}
