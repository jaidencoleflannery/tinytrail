using Microsoft.AspNetCore.Identity;
using Models.LinkModel;

namespace Models.UserModel;

public class User : IdentityUser
{
	public ICollection<Link> Links { get; set; } = new List<Link>();
}