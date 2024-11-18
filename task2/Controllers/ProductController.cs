using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using task2.Data;
using task2.Dto;
using task2.Model;

namespace task2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ProductController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet("GetAll")]
        public async Task< IActionResult >GetAll()
        {
            var products = await context.products.ToListAsync();
            if (products is null)
            {
                return NotFound("products not found");
            }
            var productsDto = products.Adapt<List<GetAllProductDto>>();

            return Ok(productsDto);
        }

        [HttpGet("Details")]
        public async Task< IActionResult> GetById(int id)
        {
            var product = await  context.products.FindAsync(id);
               
            if (product is null)
            {
                return NotFound("product not found");
            }

            var productsDto = product.Adapt<GetByIdProductDto>();
            return Ok(productsDto);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateProductDto productsDto, [FromServices] IValidator<CreateProductDto> validator)
        {
            var validationResult = await validator.ValidateAsync(productsDto);

            if (!validationResult.IsValid)
            {
                var modelstate = new ModelStateDictionary();
                validationResult.Errors.ForEach(error =>
                {
                    modelstate.AddModelError(error.PropertyName, error.ErrorMessage);
                });
                return ValidationProblem(modelstate);
            }

            var product = productsDto.Adapt<Product>();
          await  context.products.AddAsync(product);
            await context.SaveChangesAsync(); 

            var productDto = product.Adapt<GetByIdProductDto>();
            return Ok(productDto);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(int id, EditProductDto productDto, [FromServices] IValidator<EditProductDto> validator)
        {
            var validationResult = await validator.ValidateAsync(productDto);

            if (!validationResult.IsValid)
            {
                var modelstate = new ModelStateDictionary();
                validationResult.Errors.ForEach(error =>
                {
                    modelstate.AddModelError(error.PropertyName, error.ErrorMessage);
                });
                return ValidationProblem(modelstate);
            }
            var product = await context.products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            productDto.Adapt(product);
           await context.SaveChangesAsync();

            var updatedProductDto = product.Adapt<EditProductDto>();
            return Ok(updatedProductDto);
        }

        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove(int id)
        {
            var product =await context.products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            context.products.Remove(product);
            await context.SaveChangesAsync();
            return Ok("Product removed successfully.");
        }
    }
}
