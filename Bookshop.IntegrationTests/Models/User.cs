using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace Bookshop.Models
{
	public class User
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public Role Role { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj))
			{
				return true;
			}
			if (obj == null || this.GetType() != obj.GetType())
			{
				return false;
			}
			var other = (User)obj;
			return Id == other.Id
				&& Name == other.Name
				&& Role == other.Role
				&& Email == other.Email
				&& Phone == other.Phone
				&& Address == other.Address
				&& Login == other.Login
				&& Password == other.Password;
		}
	}

	public enum Role
	{
		ADMIN,
		MANAGER,
		CUSTOMER
	}
}
