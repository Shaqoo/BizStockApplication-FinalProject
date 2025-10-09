using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.ValueObjects;
using Infrastructures.Persistence.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructures.Persistence.Repositories
{
    

    public class UserRepository : IUserRepository
    {
        private readonly BizStockContext _context;

        public UserRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.Include(a => a.UserRoles).Include(a => a.RecoveryCodes)
                .Include(A => A.FidoCredentials).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> CheckIfExists(Expression<Func<User,bool>> expression)
        { 
            return await _context.Users.AnyAsync(expression);
        }

        public async Task<PaginatedList<User>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.Users.Include(a => a.UserRoles).AsQueryable();

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(u => u.FullName)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<User>(items, totalCount, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate)
        {
            return await _context.Users.Include(a => a.UserRoles).Where(predicate).ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.Include(a => a.UserRoles)
                .Include(a => a.RecoveryCodes)
                .FirstOrDefaultAsync(u => u.Email == new Email(email));
        }

        public async Task<PaginatedList<UserDto>> GetUsersByRoleAsync(Role role, PageRequest pageRequest)
        {
            var query = _context.Users
                .Include(a => a.UserRoles)
                .Where(u => u.UserRoles.Any(r => r.Role == role));

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(u => u.DateCreated).
                 Select(a => a.UserAsDto())
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<UserDto>(items, totalCount, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _context.Users.AnyAsync(u => u.Email == new Email(email));
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await Task.CompletedTask;
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user is not null)
            {
                _context.Users.Remove(user);
            }
        }

        public async Task<PaginatedList<User>> SearchUsers(string keyword, PageRequest pageRequest,Role? role)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return await GetAllAsync(pageRequest);
            }

            var formattedKeyword = keyword.Trim().Replace(" ", " & ");

            var query = _context.Users.Include(u => u.UserRoles).Where(u =>
                    EF.Functions.ILike(u.FullName, $"%{keyword}%") ||
                    EF.Functions.ILike((string)u.Email, $"%{keyword}%") ||
                    EF.Functions.ILike((string)u.PhoneNumber, $"%{keyword}%"));

            var totalCount = await query.CountAsync();

            if (totalCount == 0)
            {
                query = _context.Users.Include(u => u.UserRoles)
                        .Where(u =>
                            u.SearchVector.Matches(
                                EF.Functions.PlainToTsQuery("english", formattedKeyword)
                            ));
                totalCount = await query.CountAsync();
            }
            var customerItems = new List<User>();
            if (role is not null)
            {
                customerItems = await query.OrderBy(u => u.FullName)
                .Where(u => u.UserRoles.Any(r => r.Role == role))
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();
            }
            var items = await query
                .OrderBy(u => u.FullName)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<User>(role == null ? items : customerItems, totalCount, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task AddCode(UserRecoveryCode userRecoveryCode)
        {
            if (userRecoveryCode == null)
            {
                throw new ArgumentNullException(nameof(userRecoveryCode), "UserRecoveryCode cannot be null");
            }
            await _context.UserRecoveryCodes.AddAsync(userRecoveryCode);
        }

        public async Task<User?> GetByExpression(Expression<Func<User, bool>> predicate)
        {
             return await _context.Users
                .FirstOrDefaultAsync(predicate) 
                ?? throw new EntityNotFoundException("User","Expression");
        }

        public async Task<int> CountAsync(Expression<Func<User, bool>>? predicate = null)
        {
            IQueryable<User> query = _context.Users;

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            return await query.CountAsync();
        }

        public Task<User?> GetByRfreshToken(string rfreshToken)
        {
            return _context.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.RefreshToken == rfreshToken);
        }

        private static DateTime GetStartOfWeek(DateTime date, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            date = date.Date;
            int diff = (7 + (date.DayOfWeek - startOfWeek)) % 7;
            return date.AddDays(-diff);
        }

        public async Task<List<UserGrowthDto>> GetUserGrowthLast10WeeksAsync()
        {
            var today = DateTime.UtcNow.Date;
            var lastWeekStart = GetStartOfWeek(today, DayOfWeek.Monday);
            var firstWeekStart = lastWeekStart.AddDays(-7 * 9); 
            var fetchEndExclusive = lastWeekStart.AddDays(7);  

            var createdDates = await _context.Users
                .Where(u => u.DateCreated >= firstWeekStart && u.DateCreated < fetchEndExclusive)
                .Select(u => u.DateCreated)  
                .ToListAsync();

            var result = new List<UserGrowthDto>(10);

           
            for (var weekStart = firstWeekStart; weekStart <= lastWeekStart; weekStart = weekStart.AddDays(7))
            {
                var weekEnd = weekStart.AddDays(7);  
                var count = createdDates.Count(d => d >= weekStart && d < weekEnd);
                result.Add(new UserGrowthDto { WeekStart = weekStart, UserCount = count });
            }

            
            return result;
        }

        public async Task<TotalUserStatsDto> GetTotalUserStats()
        {
          
            var users = _context.Users.AsQueryable();

            var dto = new TotalUserStatsDto
            {
                TotalAdmins = await users.CountAsync(a => a.UserRoles.Any(u => u.Role == Role.Admin)),
                TotalCustomers = await users.CountAsync(a => a.UserRoles.Any(u => u.Role == Role.Customer)),
                TotalManagers = await users.CountAsync(a => a.UserRoles.Any(u => u.Role == Role.Manager)),
                TotalSuppliers = await users.CountAsync(a => a.UserRoles.Any(u => u.Role == Role.Supplier)),
                TotalDeliveryAgents = await users.CountAsync(a => a.UserRoles.Any(u => u.Role == Role.DeliveryAgent)),
                TotalCustomerServiceAgents = await users.CountAsync(a => a.UserRoles.Any(u => u.Role == Role.CustomerService)),
                TotalInventoryManagers = await users.CountAsync(a => a.UserRoles.Any(u => u.Role == Role.InventoryManager)),
                TotalUsers = await users.CountAsync()
            };
            return dto;
        }

    }

}
