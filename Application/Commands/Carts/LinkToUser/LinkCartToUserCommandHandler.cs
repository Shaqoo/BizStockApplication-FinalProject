using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Carts.LinkToUser
{
    public class LinkCartToUserCommandHandler : IRequestHandler<LinkCartToUserCommand, Result<string>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ILogger<LinkCartToUserCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public LinkCartToUserCommandHandler(
            ICartRepository cartRepository,
            ILogger<LinkCartToUserCommandHandler> logger,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository)
        {
            _cartRepository = cartRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<Result<string>> Handle(LinkCartToUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Starting to link cart. UserId: {UserId}, SessionId: {SessionId}", request.UserId, request.SessionId);

                var user = await _userRepository.GetByIdAsync(request.UserId);
                if (user == null)
                {
                    _logger.LogWarning("User not found. UserId: {UserId}", request.UserId);
                    return Result<string>.Failure("User not found.");
                }

                var sessionCart = await _cartRepository.GetBySessionIdAsync(request.SessionId);
                var userCart = await _cartRepository.GetByUserIdAsync(request.UserId);

                if (sessionCart is null)
                {
                    if (userCart is null)
                    {
                        _logger.LogInformation("No carts found. Creating new cart for user: {UserId}", request.UserId);
                        userCart = new Cart(request.UserId);
                        await _cartRepository.AddAsync(userCart);
                        await _unitOfWork.SaveChangesAsync(cancellationToken);
                        return Result<string>.Success("Cart Created Successfully");
                    }

                    _logger.LogInformation("Session cart not found. User cart exists. Returning existing cart.");
                    return Result<string>.Success("Cart Retrieved Successfully");
                }

                if (userCart is null)
                {
                    _logger.LogInformation("User has no cart. Linking session cart to user.");
                    sessionCart.LinkToUser(request.UserId);
                    await _cartRepository.UpdateAsync(sessionCart);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    return Result<string>.Success("Session Cart Linked To User Successfully");
                }

                _logger.LogInformation("Merging session cart into user cart. UserId: {UserId}", request.UserId);

                if (sessionCart.Items != null && sessionCart.Items.Any())
                {
                    foreach (var item in sessionCart.Items)
                    {
                        userCart.AddOrUpdateItem(item.ProductId, item.Quantity);
                    }
                }
                else
                {
                    _logger.LogInformation("Session cart is empty. Nothing to merge.");
                }

                userCart.MarkAsLinked();

                await _cartRepository.UpdateAsync(userCart);
                await _cartRepository.DeleteAsync(sessionCart);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Cart successfully linked and merged for UserId: {UserId}", request.UserId);
                return Result<string>.Success("Cart successfully linked to user.");
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Domain exception while linking cart. UserId: {UserId}, SessionId: {SessionId}", request.UserId, request.SessionId);
                return Result<string>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while linking cart. UserId: {UserId}, SessionId: {SessionId}", request.UserId, request.SessionId);
                return Result<string>.Failure("An error occurred while linking the cart to user.");
            }
        }
    }
}
