using Bookshop.IntegrationTests.Helpers;
using Bookshop.Models;
using Bookshop.UnitTests.Helpers;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Text;

namespace Bookshop.IntegrationTests.Tests
{
	public class UsersTests : BaseTest
	{
		[Test]
		public async Task PostUser_CreatesUser_WhenValidModelSubmitted()
		{
			// Arrange
			var userModel = ModelHelper.GetRandomUser();
			var seriaLizedUser = JsonConvert.SerializeObject(userModel);
			var content = new StringContent(seriaLizedUser, Encoding.UTF8, "application/json");

			// Act
			var postUserResponse = await client.PostAsync(Constants.UsersAction, content);
			var postUserResponseModel = JsonConvert.DeserializeObject<User>(await postUserResponse.Content.ReadAsStringAsync());

			// Assert
			Assert.That(postUserResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));

			userModel.Id = postUserResponseModel.Id;
			Assert.That(postUserResponseModel, Is.EqualTo(userModel));

			// Act
			var getUserResponse = await client.GetAsync($"{Constants.UsersAction}/{postUserResponseModel.Id}");
			var getUserResponseModel = JsonConvert.DeserializeObject<User>(await getUserResponse.Content.ReadAsStringAsync());

			// Assert
			Assert.That(postUserResponseModel, Is.EqualTo(getUserResponseModel));
		}

		[Test]
		public async Task GetUsers_ReturnsUsers_WhenTheyExist()
		{
			// Arrange
			var expectedUsers = new List<User>()
			{
				ModelHelper.GetRandomUser(),
				ModelHelper.GetRandomUser()
			};

			foreach (var user in expectedUsers)
			{
				var serializedUser = JsonConvert.SerializeObject(user);
				var content = new StringContent(serializedUser, Encoding.UTF8, "application/json");
				await client.PostAsync(Constants.UsersAction, content);
			}

			// Act
			var response = await client.GetAsync(Constants.UsersAction);
			var actualUsers = JsonConvert.DeserializeObject<List<User>>(await response.Content.ReadAsStringAsync());

			// Assert
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
			Assert.That(actualUsers.Any(u => u.Name == expectedUsers[0].Name), Is.True);
			Assert.That(actualUsers.Any(u => u.Name == expectedUsers[1].Name), Is.True);
		}

		[Test]
		public async Task GetUser_ReturnsUser_WhenItExists()
		{
			// Arrange
			var userModel = ModelHelper.GetRandomUser();
			var seriaLizedUser = JsonConvert.SerializeObject(userModel);
			var content = new StringContent(seriaLizedUser, Encoding.UTF8, "application/json");
			var userResponse = await client.PostAsync(Constants.UsersAction, content);
			var expectedUser = JsonConvert.DeserializeObject<User>(await userResponse.Content.ReadAsStringAsync());

			// Act
			var response = await client.GetAsync($"{Constants.UsersAction}/{expectedUser.Id}");
			var actualUser = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());

			// Assert
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
			Assert.That(actualUser.Name, Is.EqualTo(expectedUser.Name));
		}

		[Test]
		public async Task PutUser_UpdatesUser_WhenValidModelPassed()
		{
			// Arrange
			var userModel = ModelHelper.GetRandomUser();
			var seriaLizedUser = JsonConvert.SerializeObject(userModel);
			var postContent = new StringContent(seriaLizedUser, Encoding.UTF8, "application/json");
			var postUserResponse = await client.PostAsync(Constants.UsersAction, postContent);
			var postUserResponseModel = JsonConvert.DeserializeObject<User>(await postUserResponse.Content.ReadAsStringAsync());
			postUserResponseModel.Name = "UpdatedName";

			// Act
			seriaLizedUser = JsonConvert.SerializeObject(postUserResponseModel);
			var putUserContent = new StringContent(seriaLizedUser, Encoding.UTF8, "application/json");
			var putUserResponse = await client.PutAsync($"{Constants.UsersAction}/{postUserResponseModel.Id}", putUserContent);

			// Assert
			var response = await client.GetAsync($"{Constants.UsersAction}/{postUserResponseModel.Id}");
			var actualUser = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
			Assert.That(putUserResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
			Assert.That(actualUser.Name, Is.EqualTo(postUserResponseModel.Name));
		}

		[Test]
		public async Task DeleteUser_DeletesUser_WhenExistingIdPassed()
		{
			// Arrange
			var userModel = ModelHelper.GetRandomUser();
			var seriaLizedUser = JsonConvert.SerializeObject(userModel);
			var postContent = new StringContent(seriaLizedUser, Encoding.UTF8, "application/json");
			var postUserResponse = await client.PostAsync(Constants.UsersAction, postContent);
			var postUserResponseModel = JsonConvert.DeserializeObject<User>(await postUserResponse.Content.ReadAsStringAsync());

			// Act
			var deleteResponse = await client.DeleteAsync($"{Constants.UsersAction}/{postUserResponseModel.Id}");

			// Assert
			var getResponse = await client.GetAsync($"{Constants.UsersAction}/{postUserResponseModel.Id}");
			Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
			Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
		}
	}
}
