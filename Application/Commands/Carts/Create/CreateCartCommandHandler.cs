using Application.Commands.Carts.Create;
using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

public class CreateCartCommandHandler
    : IRequestHandler<CreateCartCommand, Result<CartDto>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateCartCommandHandler> _logger;
    private readonly IUserRepository _userRepository;

    public CreateCartCommandHandler(
        ICartRepository cartRepository,
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        ILogger<CreateCartCommandHandler> logger)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task<Result<CartDto>> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        var cartRequest = request.CreateCartRequest;

        _logger.LogInformation("Creating a new cart for SessionId: {SessionId}, UserId: {UserId}",
            cartRequest.SessionId, cartRequest.UserId);

        try
        {
            Cart cart;
            if (!cartRequest.UserId.HasValue || cartRequest.UserId.Value == Guid.Empty)
            {
                _logger.LogWarning("No valid UserId. Creating anonymous cart for SessionId: {SessionId}", cartRequest.SessionId);
                cart = new Cart(cartRequest.SessionId);
            }
            else
            {
                var user = await _userRepository.GetByIdAsync(cartRequest.UserId.Value);
                if (user == null)
                { 
                   _logger.LogError("User not found for UserId: {UserId}", cartRequest.UserId);
                   return Result<CartDto>.Failure("User not found.");
                }
            _logger.LogInformation("Creating linked cart for UserId: {UserId}, SessionId: {SessionId}",
                    cartRequest.UserId, cartRequest.SessionId);
                cart = new Cart(cartRequest.UserId.Value, cartRequest.SessionId);
            }

            await _cartRepository.AddAsync(cart);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var dto = new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                SessionId = cart.SessionId,
                IsLinked = cart.IsLinked,
                Items = cart.Items.Select(i => new CartItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList()
            };

            _logger.LogInformation("Cart created successfully with Id: {CartId}", cart.Id);

            return Result<CartDto>.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating cart for SessionId: {SessionId}", cartRequest.SessionId);
            return Result<CartDto>.Failure("An error occurred while creating the cart.");
        }
    }
}
