using Bookshop.Models;

namespace Bookshop.UnitTests.Helpers
{
	public static class ModelHelper
	{
		public static Product GetRandomProduct()
		{
			var id = RandomDataGenerator.GetRandomId();
			var product = new Product
			{
				Name = $"Name_{id}",
				Author = $"Author_{id}",
				Description = $"Description_{id}",
				ImagePath = $"ImagePath_{id}",
				Price = id,
			};

			return product;
		}

		public static User GetRandomUser()
		{
			var id = RandomDataGenerator.GetRandomId();
			var user = new User
			{
				Address = $"Address {id}",
				Email = "email@example.org",
				Login = "email@example.org",
				Name = $"Name_{id}",
				Password = $"Password_{id}",
				Phone = $"{id}{id}",
				Role = Role.CUSTOMER
			};

			return user;
		}

		public static Booking GetRandomBooking()
		{
			var id = RandomDataGenerator.GetRandomId();
			var booking = new Booking
			{
				UserId = 1,
				ProductId = 1,
				BookingStatus = BookingStatus.SUBMITTED,
				DateTime = DateTime.Now,
				DeliveryAddress = $"Address_{id}",
				Quantity = id
			};

			return booking;
		}

		public static StoreItem GetRandomStoreItem()
		{
			var storeItem = new StoreItem
			{
				ProductId = 1,
				AvailableQty = 2,
				BookedQty = 3,
				SoldQty = 4,
			};

			return storeItem;
		}
	}
}
