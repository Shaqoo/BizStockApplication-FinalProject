using Application.Interfaces.Repository.BaseRepository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
    public interface IInvoiceItemRepository : IBaseRepository<InvoiceItem>
    {
        Task<IEnumerable<InvoiceItem>> GetByInvoiceIdAsync(Guid invoiceId);
        Task AddRangeAsync(IEnumerable<InvoiceItem> items);
    }

}
