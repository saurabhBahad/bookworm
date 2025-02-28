using Bookworm.Dto;
using Bookworm.Models;
using Bookworm.Repository;
using Bookworm.Service;
using Bookworm.Service.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace Bookworm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductMasterController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductMasterController> _logger;
        private readonly ProductMasterServiceImpl _service;

        public ProductMasterController(AppDbContext context, ILogger<ProductMasterController> logger)
        {
            _context = context;
            _logger = logger;
            _service = new ProductMasterServiceImpl(context);
        }

        // GET: api/ProductMaster
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductMaster>>> GetProducts()
        {
            try
            {
                var products = _service.GetAll();
                return Ok(products);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting products.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/ProductMaster/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductMaster>> GetProductMaster(int id)
        {
            try
            {
                var productMaster = _service.GetProduct(id);

                if (productMaster == null)
                {
                    return NotFound();
                }

                return Ok(productMaster);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the product.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // PUT: api/ProductMaster/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductMaster(int id, ProductMaster productMaster)
        {
            if (id != productMaster.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(productMaster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ProductMasterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "A concurrency error occurred while updating the product.");
                    return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the product.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }

            return NoContent();
        }

        // POST: api/ProductMaster
        [HttpPost]
        public async Task<ActionResult<ProductMaster>> PostProductMaster(ProductMaster productMaster)
        {
            try
            {
                _context.Set<ProductMaster>().Add(productMaster);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetProductMaster", new { id = productMaster.ProductId }, productMaster);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the product.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE: api/ProductMaster/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductMaster(int id)
        {
            try
            {
                var productMaster = await _context.Set<ProductMaster>().FindAsync(id);
                if (productMaster == null)
                {
                    return NotFound();
                }

                _context.Set<ProductMaster>().Remove(productMaster);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the product.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        private bool ProductMasterExists(int id)
        {
            return _context.Set<ProductMaster>().Any(e => e.ProductId == id);
        }

        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<ProductMaster>>> FindProducts(ProductSearch ps)
        {
            try
            {
                var products = _service.GetProducts(ps);
                return Ok(products);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for products.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}