using Bookshop.Data;
using Bookshop.Models;
using Bookshop.Services;
using Bookshop.UnitTests.Helpers;
using EntityFrameworkCore.Testing.NSubstitute;
using NUnit.Framework;

namespace Bookshop.UnitTests.ServicesTests
{
	public class StoreServiceTests
	{
		private StoreService _sut;
		private BookshopDbContext _bookshopDbContext;

		[SetUp]
		public void BeforeTest()
		{
			foreach (var entity in _bookshopDbContext.StoreItems)
				_bookshopDbContext.StoreItems.Remove(entity);
			_bookshopDbContext.SaveChanges();
		}

		public StoreServiceTests()
		{
			_bookshopDbContext = Create.MockedDbContextFor<BookshopDbContext>();
			_sut = new StoreService(_bookshopDbContext);
		}

		[Test]
		public async Task GetAllStoreItems_ReturnsAllStoreItems_WhenItsCalled()
		{
			var expectedStoreItems = new List<StoreItem>()
			{
				ModelHelper.GetRandomStoreItem(),
				ModelHelper.GetRandomStoreItem()
			};
			_bookshopDbContext.Set<StoreItem>().AddRange(expectedStoreItems);
			_bookshopDbContext.SaveChanges();

			var storeItems = await _sut.GetStoreItemsAsync();

			Assert.That(storeItems.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task GetStoreItemById_ReturnsSingleStoreItem_WhenValidRequestMade()
		{
			var expectedStoreItem = ModelHelper.GetRandomStoreItem();
			_bookshopDbContext.Set<StoreItem>().Add(expectedStoreItem);
			_bookshopDbContext.SaveChanges();

			var actualStoreItem = await _sut.GetStoreItemAsync(expectedStoreItem.Id);

			Assert.That(actualStoreItem.Id, Is.EqualTo(expectedStoreItem.Id));
		}

		[Test]
		public async Task UpdateStoreItem_UpdatesTheStoreItem_WhenCorrectModelIsPassed()
		{
			var expectedStoreItem = ModelHelper.GetRandomStoreItem();
			_bookshopDbContext.Set<StoreItem>().Add(expectedStoreItem);
			_bookshopDbContext.SaveChanges();

			expectedStoreItem.BookedQty = 123;
			await _sut.UpdateStoreItemAsync(expectedStoreItem);

			var actualStoreItem = await _sut.GetStoreItemAsync(expectedStoreItem.Id);
			Assert.That(actualStoreItem.BookedQty, Is.EqualTo(expectedStoreItem.BookedQty));
		}

		[Test]
		public async Task CreateStoreItem_CreatesTheStoreItem_WhenCorrectModelIsPassed()
		{
			var expectedStoreItem = ModelHelper.GetRandomStoreItem();

			var responseStoreItem = await _sut.CreateStoreItemAsync(expectedStoreItem);

			var dbStoreItem = await _sut.GetStoreItemAsync(expectedStoreItem.Id);
			Assert.That(responseStoreItem, Is.EqualTo(expectedStoreItem));
			Assert.That(dbStoreItem, Is.EqualTo(expectedStoreItem));
		}

		[Test]
		public async Task DeleteStoreItem_DeleteTheStoreItem_WhenExistingIdIsProvided()
		{
			var expectedStoreItem = ModelHelper.GetRandomStoreItem();
			_bookshopDbContext.Set<StoreItem>().Add(expectedStoreItem);
			_bookshopDbContext.SaveChanges();

			await _sut.DeleteStoreItemAsync(expectedStoreItem.Id);

			var actualStoreItem = await _sut.GetStoreItemAsync(expectedStoreItem.Id);
			Assert.That(actualStoreItem, Is.EqualTo(null));
		}
	}
}
