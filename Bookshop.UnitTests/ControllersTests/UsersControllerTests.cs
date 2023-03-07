using Bookshop.Controllers;
using NUnit.Framework;
using NSubstitute;
using Bookshop.Services.Interfaces;
using Bookshop.Models;
using Bookshop.UnitTests.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.UnitTests.ControllersTests
{
	public class UsersControllerTests
	{
		private readonly UsersController _sut;
		private readonly IUsersService _usersService;

		public UsersControllerTests()
		{
			_usersService = Substitute.For<IUsersService>();
			_sut = new UsersController(_usersService);
		}

		[Test]
		public async Task GetUsers_ReturnsUsers_WhenRequestSubmitted()
		{
			var users = new List<User>()
			{
				ModelHelper.GetRandomUser(),
				ModelHelper.GetRandomUser()
			};
			_usersService.GetUsersAsync().Returns(users);

			var response = await _sut.GetUsers();

			var actualUsers = response.Value;
			Assert.That(actualUsers.Count, Is.EqualTo(users.Count));
		}

		[Test]
		public async Task GetUser_ReturnsUser_WhenIdIsProvided()
		{
			var user = ModelHelper.GetRandomUser();
			_usersService.GetUserAsync(user.Id).Returns(user);

			var response = await _sut.GetUser(user.Id);

			var actualUser = response.Value;
			Assert.That(actualUser, Is.EqualTo(user));
		}

		[Test]
		public async Task PutUser_UpdatesUser_WhenCorrectModelIsPassed()
		{
			var user = ModelHelper.GetRandomUser();
			_usersService.UpdateUserAsync(user).Returns(Task.CompletedTask);

			var response = (NoContentResult)await _sut.PutUser(user.Id, user);

			Assert.That(response.StatusCode, Is.EqualTo(204));
		}

		[Test]
		public async Task PostUser_CreatesUser_WhenCorrectModelIsPassed()
		{
			var user = ModelHelper.GetRandomUser();
			_usersService.CreateUserAsync(user).Returns(user);

			var response = await _sut.PostUser(user);

			var actualUser = (response.Result as CreatedAtActionResult).Value;
			Assert.That(actualUser, Is.EqualTo(user));
		}

		[Test]
		public async Task DeleteUser_DeletesUser_WhenIdPassed()
		{
			var user = ModelHelper.GetRandomUser();
			_usersService.DeleteUserAsync(user.Id).Returns(Task.CompletedTask);

			var response = (NoContentResult)await _sut.DeleteUser(user.Id);

			Assert.That(response.StatusCode, Is.EqualTo(204));
		}
	}
}
