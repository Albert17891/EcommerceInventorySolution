using EcommerceInventory.Application.DTO.ProductDTO;
using FluentValidation;

namespace EcommerceInventory.Application.Validators;
public class CreateProductRequestDtoValidator : AbstractValidator<CreateProductRequestDto>
{
    public CreateProductRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required")
            .MaximumLength(200).WithMessage("Name must be at most 200 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative");
    }
}
