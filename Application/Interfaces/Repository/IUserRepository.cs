using Application.Interfaces.Repository.BaseRepository;
using Application.Pagination;
using Domain.Entities;
using Domain.Enums;
using System.Linq.Expressions;

namespace Application.Interfaces.Repository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task AddCode(UserRecoveryCode userRecoveryCode);
        Task<User?> GetByRfreshToken(string rfreshToken);
        Task<User?> GetByEmailAsync(string email);
        Task<PaginatedList<User>> GetUsersByRoleAsync(Role role,PageRequest pageRequest);
        Task<bool> IsEmailUniqueAsync(string email);
        Task<bool> CheckIfExists(Expression<Func<User, bool>> expression);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(Guid userId);
        Task<PaginatedList<User>> SearchUsers(string keyword, PageRequest pageRequest);
        Task<int> CountAsync(Expression<Func<User, bool>>? predicate = null);

    }
}
