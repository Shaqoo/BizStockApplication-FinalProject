using Application.Interfaces.Repository.BaseRepository;
using Application.Pagination;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
    public interface IInvoiceRepository : IBaseRepository<Invoice>
    {
        Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber);
        Task<PaginatedList<Invoice>> GetByCustomerIdAsync(Guid customerId,PageRequest pageRequest);
        Task<IEnumerable<Invoice>> GetUnpaidInvoicesAsync(Guid customerId);
        Task<PaginatedList<Invoice>> GetOverdueInvoicesAsync(PageRequest pageRequest);
        Task<decimal> GetTotalOutstandingAsync(Guid customerId);
        Task<bool> IsInvoicePaidAsync(Guid invoiceId);
        Task UpdateInvoice(Invoice invoice);
        Task DeleteInvoiceAsync(Guid invoiceId);
    }

}
