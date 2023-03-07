using System.ComponentModel.DataAnnotations;

namespace Bookshop.Models
{
	public class Product
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Author { get; set; }
		
		[Range(0, float.MaxValue)]
		public float Price { get; set; }
		public string? ImagePath { get; set;}
	}
}
