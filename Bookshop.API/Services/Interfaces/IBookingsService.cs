using Bookshop.Models;

namespace Bookshop.Services.Interfaces
{
	public interface IBookingsService
	{
		Task<List<Booking>> GetBookingsAsync();
		Task<Booking> GetBookingAsync(int id);
		Task UpdateBookingAsync(Booking booking);
		// will call store service to update booking for product
		Task<Booking> CreateBookingAsync(Booking booking);
		Task DeleteBookingAsync(int id);
	}
}
