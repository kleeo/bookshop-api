using Bookshop.Data;
using Bookshop.Models;
using Bookshop.Services;
using Bookshop.UnitTests.Helpers;
using EntityFrameworkCore.Testing.NSubstitute;
using NUnit.Framework;

namespace Bookshop.UnitTests.ServicesTests
{
	public class ProductsServiceTests
	{
		private ProductsService _sut;
		private BookshopDbContext _bookshopDbContext;

		[SetUp]
		public void BeforeTest()
		{
			foreach (var entity in _bookshopDbContext.Products)
				_bookshopDbContext.Products.Remove(entity);
			_bookshopDbContext.SaveChanges();
		}

		public ProductsServiceTests()
		{
			_bookshopDbContext = Create.MockedDbContextFor<BookshopDbContext>();
			_sut = new ProductsService(_bookshopDbContext);
		}

		[Test]
		public async Task GetAllProducts_ReturnsAllProducts_WhenItsCalled()
		{
			var expectedProducts = new List<Product>()
			{
				ModelHelper.GetRandomProduct(),
				ModelHelper.GetRandomProduct()
			};
			_bookshopDbContext.Set<Product>().AddRange(expectedProducts);
			_bookshopDbContext.SaveChanges();

			var products = await _sut.GetProductsAsync();

			Assert.That(products.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task GetProductById_ReturnsSingleProduct_WhenValidRequestMade()
		{
			var expectedProduct = ModelHelper.GetRandomProduct();
			_bookshopDbContext.Set<Product>().Add(expectedProduct);
			_bookshopDbContext.SaveChanges();

			var actualProduct = await _sut.GetProductAsync(expectedProduct.Id);

			Assert.That(actualProduct.Id, Is.EqualTo(expectedProduct.Id));
		}

		[Test]
		public async Task UpdateProduct_UpdatesTheProduct_WhenCorrectModelIsPassed()
		{
			var expectedProduct = ModelHelper.GetRandomProduct();
			_bookshopDbContext.Set<Product>().Add(expectedProduct);
			_bookshopDbContext.SaveChanges();

			expectedProduct.Author = "Updated author";
			await _sut.UpdateProductAsync(expectedProduct);

			var actualProduct = await _sut.GetProductAsync(expectedProduct.Id);
			Assert.That(actualProduct.Author, Is.EqualTo(expectedProduct.Author));
		}

		[Test]
		public async Task CreateProduct_CreatesTheProduct_WhenCorrectModelIsPassed()
		{
			var expectedProduct = ModelHelper.GetRandomProduct();

			var responseProduct = await _sut.CreateProductAsync(expectedProduct);

			var dbProduct = await _sut.GetProductAsync(expectedProduct.Id);
			Assert.That(responseProduct, Is.EqualTo(expectedProduct));
			Assert.That(dbProduct, Is.EqualTo(expectedProduct));
		}

		[Test]
		public async Task DeleteProduct_DeleteTheProduct_WhenExistingIdIsProvided()
		{
			var expectedProduct = ModelHelper.GetRandomProduct();
			_bookshopDbContext.Set<Product>().Add(expectedProduct);
			_bookshopDbContext.SaveChanges();

			await _sut.DeleteProductAsync(expectedProduct.Id);

			var actualProduct = await _sut.GetProductAsync(expectedProduct.Id);
			Assert.That(actualProduct, Is.EqualTo(null));
		}
	}
}
