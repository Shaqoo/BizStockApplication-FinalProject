namespace Application.Validators.Cart
{
    using Application.Dto.RequestModels;
    using FluentValidation;

    public class CreateCartValidator : AbstractValidator<CreateCartRequest>
    {
        public CreateCartValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().When(x => string.IsNullOrWhiteSpace(x.SessionId));
            RuleFor(x => x.SessionId).NotEmpty().When(x => !x.UserId.HasValue);
        }
    }

    public class AddCartItemValidator : AbstractValidator<AddCartItemRequest>
    {
        public AddCartItemValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Quantity).GreaterThan(0);
        }
    }

    public class UpdateCartItemQuantityValidator : AbstractValidator<UpdateCartItemQuantityRequest>
    {
        public UpdateCartItemQuantityValidator()
        {
            RuleFor(x => x.CartId).NotEmpty();
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Quantity).GreaterThan(0);
        }
    }

    public class RemoveCartItemValidator : AbstractValidator<RemoveCartItemRequest>
    {
        public RemoveCartItemValidator()
        {
            RuleFor(x => x.CartId).NotEmpty();
            RuleFor(x => x.ProductId).NotEmpty();
        }
    }

}
