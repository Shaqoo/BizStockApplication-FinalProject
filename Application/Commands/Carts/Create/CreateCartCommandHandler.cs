using Application.Commands.Carts.Create;
using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

public class CreateCartCommandHandler
    : IRequestHandler<CreateCartCommand, Result<CartDto>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateCartCommandHandler> _logger;

    public CreateCartCommandHandler(
        ICartRepository cartRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ILogger<CreateCartCommandHandler> logger)
    {
        _cartRepository = cartRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<CartDto>> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        var dto = request.CreateCartRequest;

        if (dto.UserId == null && string.IsNullOrWhiteSpace(dto.SessionId))
        {
            _logger.LogWarning("Cart creation failed. Both UserId and SessionId are missing.");
            return Result<CartDto>.Failure("Either UserId or SessionId must be provided.");
        }

        // Validate UserId if provided
        if (dto.UserId.HasValue)
        {
            var userExists = await _userRepository.GetByIdAsync(dto.UserId.Value);
            if (userExists is null)
            {
                _logger.LogWarning("Cart creation failed. UserId {UserId} does not exist.", dto.UserId.Value);
                return Result<CartDto>.Failure("Invalid UserId.");
            }
        }

        Cart? existingCart = dto.UserId.HasValue
            ? await _cartRepository.GetByUserIdAsync(dto.UserId.Value)
            : await _cartRepository.GetBySessionIdAsync(dto.SessionId!);

        if (existingCart != null)
        {
            _logger.LogInformation("Cart already exists for {KeyType}: {Key}",
                dto.UserId.HasValue ? "UserId" : "SessionId",
                dto.UserId.ToString() ?? dto.SessionId);

            return Result<CartDto>.Success(existingCart.ToDto());
        }


        var cart = dto.UserId.HasValue ? new Cart(dto.UserId.Value) : new Cart(dto.SessionId!);

        await _cartRepository.AddAsync(cart);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("New cart created with Id {CartId}", cart.Id);

        return Result<CartDto>.Success(cart.ToDto());
    }
}

