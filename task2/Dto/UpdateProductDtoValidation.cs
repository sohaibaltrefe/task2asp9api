using FluentValidation;
using Microsoft.EntityFrameworkCore;
using task2.Data;

namespace task2.Dto
{
    public class UpdateProductDtoValidation : AbstractValidator<EditProductDto>
    {
        private readonly ApplicationDbContext context;

        public UpdateProductDtoValidation(ApplicationDbContext context)
        {
            this.context = context;
            RuleFor(p => p.Name).MinimumLength(5).WithMessage("The product name must be at least 5 characters.")
                   .MaximumLength(30).WithMessage("The product name cannot exceed 30 characters.")
                   .MustAsync(BeUnique).WithMessage("The product name must be unique.");

            RuleFor(p => p.price).InclusiveBetween(20, 3000).WithMessage("The price must be between 20 and 3000.");

            RuleFor(p => p.Description)
                .MinimumLength(10).WithMessage("The description must be at least 10 characters.");

        }
        private async Task<bool> BeUnique(string name, CancellationToken cancellationToken)
        {
            return !await context.products.AnyAsync(p => p.Name == name, cancellationToken);
        }
    }
}
