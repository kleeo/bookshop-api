using Microsoft.AspNetCore.Mvc;
using Bookshop.Models;
using Bookshop.Services.Interfaces;
using Bookshop.Helpers;

namespace Bookshop.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BookingsController : ControllerBase
	{
		private readonly IBookingsService _bookingsService;
		private readonly IProductsService _productsService;
		private readonly IUsersService _usersService;

		public BookingsController(
			IBookingsService bookingsService,
			IUsersService usersService,
			IProductsService productsService
			)
		{
			_bookingsService = bookingsService;
			_usersService = usersService;
			_productsService = productsService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
		{
			var bookings = await _bookingsService.GetBookingsAsync();
			return bookings;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Booking>> GetBooking(int id)
		{
			var booking = await _bookingsService.GetBookingAsync(id);
			if (booking == null)
			{
				return NotFound(new ErrorMessage() { Message = string.Format(Constants.BookingWithIdDoesntExist, id) });
			}

			return booking;
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutBooking(int id, Booking booking)
		{
			if (booking == null)
			{
				return BadRequest(new ErrorMessage { Message = Constants.InvalidModelMessage });
			}

			var user = await _usersService.GetUserAsync(booking.UserId);
			if (user == null)
			{
				return NotFound(new ErrorMessage { Message = string.Format(Constants.UserWithIdDoesntExist, booking.UserId) });
			}

			var product = await _productsService.GetProductAsync(booking.ProductId);
			if (product == null)
			{
				return NotFound(new ErrorMessage { Message = string.Format(Constants.ProductWithIdDoesntExist, booking.ProductId) });
			}

			booking.Id = id;
			try
			{
				await _bookingsService.UpdateBookingAsync(booking);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new ErrorMessage { Message = ex.Message });
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<Booking>> PostBooking(Booking booking)
		{
			if (booking == null)
			{
				return BadRequest(new ErrorMessage { Message = Constants.InvalidModelMessage });
			}

			var user = await _usersService.GetUserAsync(booking.UserId);
			if (user == null)
			{
				return NotFound(new ErrorMessage { Message = string.Format(Constants.UserWithIdDoesntExist, booking.UserId) });
			}

			var product = await _productsService.GetProductAsync(booking.ProductId);
			if (product == null)
			{
				return NotFound(new ErrorMessage { Message = string.Format(Constants.ProductWithIdDoesntExist, booking.ProductId) });
			}

			var createdBooking = await _bookingsService.CreateBookingAsync(booking);
			return CreatedAtAction("GetBooking", new { id = createdBooking.Id }, createdBooking);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteBooking(int id)
		{
			try
			{
				await _bookingsService.DeleteBookingAsync(id);
			}
			catch (InvalidOperationException ex)
			{
				return NotFound(new ErrorMessage { Message = ex.Message });
			}

			return NoContent();
		}
	}
}
