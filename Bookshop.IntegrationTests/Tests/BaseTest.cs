

namespace Bookshop.IntegrationTests.Tests
{
	public class BaseTest
	{
		protected string BaseUrl;
		protected HttpClient client;

		public BaseTest()
		{
			var baseUrl = System.Configuration.ConfigurationManager.AppSettings["baseUrl"];
			client = new HttpClient();
			client.BaseAddress = new Uri(baseUrl);
		}
	}
}
