using Application.Dto;
using Application.Interfaces.Repository.BaseRepository;
using Application.Pagination;
using Application.Queries.Payments.GetPaymentStats;
using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface IPaymentRepository : IBaseRepository<Payment>
    {
        Task<Payment?> GetByReferenceAsync(string paymentReference);
        Task<IEnumerable<Payment>> GetByInvoiceIdAsync(Guid invoiceId);
        Task<IEnumerable<Payment>> GetByPayerIdAsync(Guid payerId);
        Task<decimal> GetTotalPaidForInvoiceAsync(Guid invoiceId);
        Task<bool> IsInvoiceFullyPaidAsync(Guid invoiceId);
        Task<IEnumerable<Payment>> GetPendingPaymentsAsync();
        Task UpdateAsync(Payment payment); 
        Task DeleteAsync(Guid id);
        Task<PaymentStatsDto> GetPaymentStatsAsync();
        Task<PaginatedList<PaymentDto>> GetByCustomerIdAsync(Guid customerId, PageRequest pageRequest);
    }

}
