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
using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Bookshop.Helpers;
using Bookshop.Services;

namespace Bookshop.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly IProductsService _productService;

		public ProductsController(IProductsService productService)
		{
			_productService = productService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
		{
			var products = await _productService.GetProductsAsync();
			return products;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> GetProduct(int id)
		{
			var product = await _productService.GetProductAsync(id);
			if (product == null)
			{
				return NotFound(new ErrorMessage() { Message = string.Format(Constants.ProductWithIdDoesntExist, id) });
			}

			return product;
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutProduct(int id, Product product)
		{
			if (product == null)
			{
				return BadRequest(new ErrorMessage { Message = Constants.InvalidModelMessage });
			}
			product.Id = id;
			try
			{
				await _productService.UpdateProductAsync(product);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new ErrorMessage { Message = ex.Message });
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<Product>> PostProduct(Product product)
		{
			if (product == null)
			{
				return BadRequest(new ErrorMessage { Message = Constants.InvalidModelMessage });
			}

			var createdProduct = await _productService.CreateProductAsync(product);
			return CreatedAtAction("GetProduct", new { id = createdProduct.Id }, createdProduct);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			try
			{
				await _productService.DeleteProductAsync(id);
			}
			catch (InvalidOperationException ex)
			{
				return NotFound(new ErrorMessage { Message = ex.Message });
			}

			return NoContent();
		}
	}
}
