using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.UnitOfWork;
using MediatR;

namespace Application.Commands.Carts.ClearCart
{
    public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand, Result<CartDto>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ClearCartCommandHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork)
        {
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CartDto>> Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetByIdAsync(request.CartId);

            if (cart == null)
                return Result<CartDto>.Failure("Cart not found.");

            cart.ClearItems();

            await _cartRepository.UpdateAsync(cart);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<CartDto>.Success(cart.ToDto());
        }
    }

}
