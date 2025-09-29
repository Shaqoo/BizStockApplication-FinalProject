using Application.Dto;
using Application.Dto.RequestModels;
using Domain.Entities;

namespace Application.Extensions
{
    public static class Extension
    {
        public static UserDto UserAsDto(this User user)
        {
            if (user is null)
                throw new Exception("User is Null");
            Console.WriteLine(user.FullName);
            return new UserDto(user.Id,(string)user.Email,user.FullName, user.DateOfBirth.Age, user.PhoneNumber.ToString()
                , user.DateOfBirth.Value, user.LastLoggedIn, user.UserRoles.FirstOrDefault()!.Role.ToString(),
                user.Gender.ToString(),user.IsEmailVerified,
                user.IsTwoFactorEnabled,user.ProfilePictureUrl);
        }

        public static SupplierDto SupplierAsDto(this Supplier supplier)
        {
            return new SupplierDto(supplier.Id,supplier.CompanyName,supplier.Address,supplier.PhoneNumber.ToString(),
                supplier.TaxId,supplier.CompanyName,(string)supplier.Email);
        }

        public static DeliveryAgentDto DeliveryAgentAsDto(this DeliveryAgent deliveryAgent)
        {
            return new DeliveryAgentDto(deliveryAgent.Id, deliveryAgent.FullName, deliveryAgent.Email.ToString(), deliveryAgent.VehicleNumber,
                deliveryAgent.ContactNumber,deliveryAgent.AvailabilityStatus);
        }

        public static CustomerDto CustomerAsDto(this Customer customer)
        {
            return new CustomerDto(customer.Id, customer.FullName,customer.CustomerType.TypeName.ToString(),customer.TaxId,
                customer.State,customer.Address,customer.BusinessName);
        }

        public static ChatThreadDto ChatThreadAsDto(this ChatThread chatThread)
        {
            return new ChatThreadDto(chatThread.Id, chatThread.Status,chatThread.CreatedBy, chatThread.AssignedAgentId,
                chatThread.DateCreated,chatThread.LastModified);
        }

        public static MessageDto AsDto(this ChatMessage message)
        {
            return new MessageDto(
                message.Id,
                message.ChatThreadId,
                message.SenderId,
                message.Sender.FullName,  
                message.Message,
                message.AudioUrl,
                message.PictureUrl,
                message.RepliedToMessageId,
                message.RepliedToMessage?.Message,
                message.IsRead,
                message.SentAt,
                message.Reactions.Select(r => new ReactionDto(r.ReactedByUserId, r.Emoji)).ToList()
            );
        }

        public static CategoryDto CategoryAsDto(this Category category)
        {
            return new CategoryDto(category.Id, category.Name, category.Description,category.Depth ,category.ParentCategoryId,category.Products.Count);
        }

        public static WarehouseProductDto WarehouseProductDto(this WarehouseItem warehouseItem)
        {
            return new WarehouseProductDto(warehouseItem.Product.Id,warehouseItem.WarehouseId,warehouseItem.Quantity,
                warehouseItem.ReorderLevel,warehouseItem.Product.Name,warehouseItem.Warehouse.Name,warehouseItem.Product.ImageUrl,
                warehouseItem.Warehouse.Location,warehouseItem.Product.SKU,warehouseItem.Product.UnitOfMeasure);
        }

        public static ProductDto ToDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                SKU = product.SKU,
                Barcode = product.Barcode,
                QrCodeValue = product.QrCodeValue,
                Description = product.Description!,
                ImageUrl = product.ImageUrl,
                CostPrice = product.CostPrice,
                SellingPrice = product.SellingPrice,
                UnitOfMeasure = product.UnitOfMeasure.ToString(),

                 
                CategoryId = product.CategoryId,
                //CategoryName = product.Category.Name ?? string.Empty,

                BrandId = product.BrandId,
               // BrandName = product.Brand.Name ?? string.Empty,

                 
                Quantity = product.StockByWarehouse?.Sum(s => s.Quantity) ?? 0,
                ReorderLevel = product.StockByWarehouse?.Any() == true
                    ? product.StockByWarehouse.Min(s => s.ReorderLevel)
                    : 0
            };
        }


        public static StockMovementDto ToDto(this StockMovement stockMovement)
        {
            return new StockMovementDto(stockMovement.Id, stockMovement.WarehouseItem.ProductId, stockMovement.MovementType,
                stockMovement.QuantityChanged, stockMovement.WarehouseItem.WarehouseId,stockMovement.DateCreated,stockMovement.PerformedByUserId ,stockMovement.Reason);
            
        }

        public static AuditLogReadDto MapToDto(this AuditLog log) =>
            new()
            {
                Id = log.Id,
                UserId = log.UserId,
                Timestamp = log.Timestamp,
                Action = log.Action,
                EntityName = log.EntityName,
                EntityId = log.EntityId,
                Description = log.Description,
                IpAddress = log.IpAddress,
                UserAgent = log.UserAgent
            };

        public static CartDto ToDto(this Cart cart) =>
        new CartDto
        {
            Id = cart.Id,
            UserId = cart.UserId,
            SessionId = cart.SessionId,
            IsLinked = cart.IsLinked,
        };

        public static CartItemDto ToDto(this CartItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            return new CartItemDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                CartId = item.CartId,
                Quantity = item.Quantity,
                ProductName = item.Product?.Name ?? string.Empty,
                UnitPrice = item.Product?.SellingPrice ?? 0m,
                ProductImg = item.Product?.ImageUrl ?? string.Empty
            };
        }

        public static IEnumerable<CartItemDto> ToDtoList(this IEnumerable<CartItem> items)
        {
            return items.Select(i => i.ToDto()).ToList();
        }

        public static WalletTransactionDto AsDto(this WalletTransaction transaction)
        {
            return new WalletTransactionDto
            {
                Id = transaction.Id,
                WalletId = transaction.WalletId,
                Amount = transaction.Amount,
                Type = transaction.Type,
                Reference = transaction.Reference,
                Description = transaction.Description,
                PaymentId = transaction.PaymentId,
                CreatedAt = transaction.DateCreated
            };
        }

        public static PaymentDto AsDto(this Payment payment)
        {
            return new PaymentDto
            {
                Id = payment.Id,
                PaymentReference = payment.PaymentReference,
                InvoiceId = payment.InvoiceId,
                Amount = payment.Amount,
                Method = payment.Method,
                Status = payment.Status,
                Note = payment.Note,
                PayerId = payment.PayerId,
                PayerName = payment.Payer.FullName,
                Purpose = payment.Purpose,
                WalletTransactionId = payment.WalletTransactionId,
                CreatedAt = payment.DateCreated
            };
        }
    }
}
