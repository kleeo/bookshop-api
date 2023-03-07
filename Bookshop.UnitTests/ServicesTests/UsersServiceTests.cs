using Bookshop.Data;
using Bookshop.Models;
using Bookshop.Services;
using Bookshop.UnitTests.Helpers;
using EntityFrameworkCore.Testing.NSubstitute;
using NUnit.Framework;

namespace Bookshop.UnitTests.ServicesTests
{
	public class UsersServiceTests
	{
		private UsersService _sut;
		private BookshopDbContext _bookshopDbContext;

		[SetUp]
		public void BeforeTest()
		{
			foreach (var entity in _bookshopDbContext.Users)
				_bookshopDbContext.Users.Remove(entity);
			_bookshopDbContext.SaveChanges();
		}

		public UsersServiceTests()
		{
			_bookshopDbContext = Create.MockedDbContextFor<BookshopDbContext>();
			_sut = new UsersService(_bookshopDbContext);
		}

		[Test]
		public async Task GetAllUsers_ReturnsAllUsers_WhenItsCalled()
		{
			var expectedUsers = new List<User>()
			{
				ModelHelper.GetRandomUser(),
				ModelHelper.GetRandomUser()
			};
			_bookshopDbContext.Set<User>().AddRange(expectedUsers);
			_bookshopDbContext.SaveChanges();

			var users = await _sut.GetUsersAsync();

			Assert.That(users.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task GetUserById_ReturnsSingleUser_WhenValidRequestMade()
		{
			var expectedUser = ModelHelper.GetRandomUser();
			_bookshopDbContext.Set<User>().Add(expectedUser);
			_bookshopDbContext.SaveChanges();

			var actualUser = await _sut.GetUserAsync(expectedUser.Id);

			Assert.That(actualUser.Id, Is.EqualTo(expectedUser.Id));
		}

		[Test]
		public async Task UpdateUser_UpdatesTheUser_WhenCorrectModelIsPassed()
		{
			var expectedUser = ModelHelper.GetRandomUser();
			_bookshopDbContext.Set<User>().Add(expectedUser);
			_bookshopDbContext.SaveChanges();

			expectedUser.Address = "Updated address";
			await _sut.UpdateUserAsync(expectedUser);

			var actualUser = await _sut.GetUserAsync(expectedUser.Id);
			Assert.That(actualUser.Address, Is.EqualTo(expectedUser.Address));
		}

		[Test]
		public async Task CreateUser_CreatesTheUser_WhenCorrectModelIsPassed()
		{
			var expectedUser = ModelHelper.GetRandomUser();

			var responseUser = await _sut.CreateUserAsync(expectedUser);

			var dbUser = await _sut.GetUserAsync(expectedUser.Id);
			Assert.That(responseUser, Is.EqualTo(expectedUser));
			Assert.That(dbUser, Is.EqualTo(expectedUser));
		}

		[Test]
		public async Task DeleteUser_DeleteTheUser_WhenExistingIdIsProvided()
		{
			var expectedUser = ModelHelper.GetRandomUser();
			_bookshopDbContext.Set<User>().Add(expectedUser);
			_bookshopDbContext.SaveChanges();

			await _sut.DeleteUserAsync(expectedUser.Id);

			var actualUser = await _sut.GetUserAsync(expectedUser.Id);
			Assert.That(actualUser, Is.EqualTo(null));
		}


	}
}
