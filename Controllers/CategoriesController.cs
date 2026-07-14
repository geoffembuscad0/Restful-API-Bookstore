using Microsoft.AspNetCore.Mvc;
using RestfulApiDemo.Data;
using RestfulApiDemo.DTOs;
using RestfulApiDemo.Models;

namespace RestfulApiDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetAll()
        {
            try
            {
                var categories = _context.Categories
                    .Where(c => c.DeletedAt == null)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching categories", error = ex.Message });
            }
        }

        /// <summary>
        /// Get category by ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<Category> GetById(long id)
        {
            try
            {
                var category = _context.Categories
                    .FirstOrDefault(c => c.Id == id && c.DeletedAt == null);

                if (category == null)
                {
                    return NotFound(new { message = "Category not found" });
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the category", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Category>> Create([FromBody] CategoryRequest request)
        {
            try
            {
                var category = new Category
                {
                    Title = request.Title,
                    CreatedAt = DateTime.Now
                };

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the category", error = ex.Message });
            }
        }

        /// <summary>
        /// Update a category
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<Category>> Update(long id, [FromBody] CategoryRequest request)
        {
            try
            {
                var category = _context.Categories
                    .FirstOrDefault(c => c.Id == id && c.DeletedAt == null);

                if (category == null)
                {
                    return NotFound(new { message = "Category not found" });
                }

                category.Title = request.Title;

                _context.Categories.Update(category);
                await _context.SaveChangesAsync();

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the category", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a category (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                var category = _context.Categories
                    .FirstOrDefault(c => c.Id == id && c.DeletedAt == null);

                if (category == null)
                {
                    return NotFound(new { message = "Category not found" });
                }

                category.DeletedAt = DateTime.Now;

                _context.Categories.Update(category);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Category deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the category", error = ex.Message });
            }
        }
    }
}