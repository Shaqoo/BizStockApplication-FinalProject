using Application.Interfaces.Repository.BaseRepository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }

}
