using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bookshop.Data;
using Bookshop.Models;
using Bookshop.Services.Interfaces;
using Bookshop.Helpers;
using Bookshop.Services;

namespace Bookshop.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUsersService _userService;

		public UsersController(IUsersService userService)
		{
			_userService = userService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<User>>> GetUsers()
		{
			var users = await _userService.GetUsersAsync();
			return users;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<User>> GetUser(int id)
		{
			var user = await _userService.GetUserAsync(id);
			if (user == null)
			{
				return NotFound(new ErrorMessage() { Message = string.Format(Constants.UserWithIdDoesntExist, id) });
			}

			return user;
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutUser(int id, User user)
		{
			if (user == null)
			{
				return BadRequest(new ErrorMessage { Message = Constants.InvalidModelMessage });
			}
			user.Id = id;
			try
			{
				await _userService.UpdateUserAsync(user);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new ErrorMessage { Message = ex.Message });
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<User>> PostUser(User user)
		{
			if (user == null)
			{
				return BadRequest(new ErrorMessage { Message = Constants.InvalidModelMessage });
			}

			var createdUser = await _userService.CreateUserAsync(user);
			return CreatedAtAction("GetUser", new { id = createdUser.Id }, createdUser);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUser(int id)
		{
			try
			{
				await _userService.DeleteUserAsync(id);
			}
			catch (InvalidOperationException ex)
			{
				return NotFound(new ErrorMessage { Message = ex.Message });
			}

			return NoContent();
		}
	}
}
