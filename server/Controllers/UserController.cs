using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Models.UserModel;

namespace Controller.UserController;

[ApiController]
[Route("/api/auth")]
public class UserController : ControllerBase {
	private readonly UserManager<User> _userManager;
	private readonly SignInManager<User> _signInManager;
	private readonly ILogger<UserController> _logger;

	public UserController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<UserController> logger)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_logger = logger;
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login(UserDto dto)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		var result = await _signInManager.PasswordSignInAsync(
					dto.Username,
					dto.Password,
					isPersistent: false,
					lockoutOnFailure: false
		);

		if (result.Succeeded)
		{
			var user = await _userManager.FindByNameAsync(dto.Username);
			if (user != null)
			{
				var principal = await _signInManager.CreateUserPrincipalAsync(user);
				await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);
			}
			else
			{
				_logger.LogError("(UserController Login) Could not find name in _userManager.");
				return Unauthorized("Could not find user.");
			}
		}
		else
		{
			return Unauthorized("User does not exist.");
		}

		return Ok("User logged in successfully.");
	}

	[HttpPost("register")]
	public async Task<IActionResult> Register(UserDto dto)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		var userExists = await _userManager.FindByNameAsync(dto.Username);
		if (userExists != null)
		{
			return BadRequest("Username already exists.");
		}
		else
		{
			var user = new User { UserName = dto.Username };
			var result = await _userManager.CreateAsync(user, dto.Password);
			if (!result.Succeeded)
			{
				return BadRequest(result.Errors);
			}
			return CreatedAtRoute("PostMessage", new { id = user.Id }, user);
		}
	}

	[HttpGet("status")]
	public async Task<IActionResult> Status() {
		if (HttpContext.User.Identity.IsAuthenticated == false)
		{
			return Unauthorized();
		}
		else
		{
			return Ok("Authorized");
		}
	}
}