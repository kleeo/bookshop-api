using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bookshop.Data;
using Bookshop.Models;
using Bookshop.Services.Interfaces;
using Bookshop.Services;
using Bookshop.Helpers;

namespace Bookshop.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StoreController : ControllerBase
	{
		private readonly IStoreService _storeService;
		private readonly IProductsService _productService;

		public StoreController(IStoreService storeService, IProductsService productService)
		{
			_storeService = storeService;
			_productService = productService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<StoreItem>>> GetStoreItems()
		{
			var storeItems = await _storeService.GetStoreItemsAsync();
			return storeItems;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<StoreItem>> GetStoreItem(int id)
		{
			var storeItem = await _storeService.GetStoreItemAsync(id);
			if (storeItem == null)
			{
				return NotFound(new ErrorMessage() { Message = string.Format(Constants.StoreItemWithIdDoesntExist, id) });
			}

			return storeItem;
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutStoreItem(int id, StoreItem storeItem)
		{
			if (storeItem == null)
			{
				return BadRequest(new ErrorMessage { Message = Constants.InvalidModelMessage });
			}

			var product = await _productService.GetProductAsync(storeItem.ProductId);
			if (product == null)
			{
				return NotFound(new ErrorMessage { Message = string.Format(Constants.ProductWithIdDoesntExist, storeItem.ProductId) });
			}

			storeItem.Id = id;
			try
			{
				await _storeService.UpdateStoreItemAsync(storeItem);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new ErrorMessage { Message = ex.Message });
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<StoreItem>> PostStoreItem(StoreItem storeItem)
		{
			if (storeItem == null)
			{
				return BadRequest(new ErrorMessage { Message = Constants.InvalidModelMessage });
			}

			var product = await _productService.GetProductAsync(storeItem.ProductId);
			if (product == null)
			{
				return NotFound(new ErrorMessage { Message = string.Format(Constants.ProductWithIdDoesntExist, storeItem.ProductId) });
			}

			var createdBooking = await _storeService.CreateStoreItemAsync(storeItem);
			return CreatedAtAction("GetStoreItem", new { id = createdBooking.Id }, createdBooking);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteStoreItem(int id)
		{
			try
			{
				await _storeService.DeleteStoreItemAsync(id);
			}
			catch (InvalidOperationException ex)
			{
				return NotFound(new ErrorMessage { Message = ex.Message });
			}

			return NoContent();
		}
	}
}
