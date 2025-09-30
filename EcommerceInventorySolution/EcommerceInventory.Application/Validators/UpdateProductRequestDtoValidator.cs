using EcommerceInventory.Application.DTO.ProductDTO;
using FluentValidation;

namespace EcommerceInventory.Application.Validators;
public class UpdateProductRequestDtoValidator : AbstractValidator<UpdateProductRequestDto>
{
    public UpdateProductRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(200).When(x => x.Name != null)
            .WithMessage("Name must be at most 200 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).When(x => x.Price.HasValue)
            .WithMessage("Price must be greater than 0");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).When(x => x.Stock.HasValue)
            .WithMessage("Stock cannot be negative");
    }
}
