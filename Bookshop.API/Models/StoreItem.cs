using System.ComponentModel.DataAnnotations;

namespace Bookshop.Models
{
	public class StoreItem
	{
		public int Id { get; set; }
		public int ProductId { get; set; }

		[Range(0, int.MaxValue)]
		public int AvailableQty { get; set; }

		[Range(0, int.MaxValue)]
		public int BookedQty { get; set; }

		[Range(0, int.MaxValue)]
		public int SoldQty { get; set; }
	}
}
