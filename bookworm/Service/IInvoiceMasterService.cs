using Bookworm.Models;

namespace Bookworm.Service
{
    public interface IInvoiceMasterService
    {
        Task<List<InvoiceDetail>> GetInvoiceDetail(int id);
        Task<List<InvoiceDetail>> GenerateInvoice(int custId);
    }
}
