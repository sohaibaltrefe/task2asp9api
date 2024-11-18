using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using task2.Data;

namespace task2.Dto
{
    public class CreateProductDtoValidation:AbstractValidator<CreateProductDto>
    {
        private readonly ApplicationDbContext context;

        public CreateProductDtoValidation( ApplicationDbContext context) {
 this.context = context;
            RuleFor(p => p.Name)
                 .NotEmpty().WithMessage("The product name is required !!!!!!")
                 .MinimumLength(5).WithMessage("The product name must be at least 5 characters.")
                 .MaximumLength(30).WithMessage("The product name cannot exceed 30 characters.")
                 .MustAsync(BeUnique).WithMessage("The product name must be unique.");

            RuleFor(p => p.price)
                .NotEmpty().WithMessage("The price is required !!!!!!")
                .InclusiveBetween(20, 3000).WithMessage("The price must be between 20 and 3000.");

            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("The description is required !!!!!!")
                .MinimumLength(10).WithMessage("The description must be at least 10 characters.");
           
        }
        private async Task<bool> BeUnique(string name, CancellationToken cancellationToken)
        {
            return !await context.products.AnyAsync(p => p.Name == name, cancellationToken);
        }
    }
}
