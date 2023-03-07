using System.ComponentModel.DataAnnotations;

namespace Bookshop.Models
{
	public class Booking
	{
		public int Id { get; set; }
		public int ProductId { get; set; }
		public int UserId { get; set; }
		public string DeliveryAddress { get; set; }
		public DateTime DateTime { get; set; }
		public BookingStatus BookingStatus { get; set; }

		[Range(0, int.MaxValue)]
		public int Quantity { get; set; }
	}

	public enum BookingStatus
	{
		SUBMITTED,
		REJECTED,
		APPROVED,
		CANCELED,
		IN_DELIVERY,
		COMPLETED
	}
}
