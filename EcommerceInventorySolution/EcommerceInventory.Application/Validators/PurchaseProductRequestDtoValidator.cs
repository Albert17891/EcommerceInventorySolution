using EcommerceInventory.Application.DTO.ProductDTO;
using FluentValidation;

namespace EcommerceInventory.Application.Validators;
public class PurchaseProductRequestDtoValidator : AbstractValidator<PurchaseProductRequestDto>
{
    public PurchaseProductRequestDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}
