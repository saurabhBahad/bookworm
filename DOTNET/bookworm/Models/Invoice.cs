using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Models;

[Table("invoice")]
[Index("CartId", Name = "FK74rjp8604l111tb50mbg1ubbd")]
[Index("CustomerId", Name = "FKk9j7m0iwl2u5ccibh3piocfj")]
public partial class Invoice
{
    [Key]
    [Column("invoice_id")]
    public int InvoiceId { get; set; }

    [Column("amount")]
    [Precision(10, 0)]
    public decimal? Amount { get; set; }

    [Column("date")]
    public DateOnly? Date { get; set; }

    [Column("cart_id")]
    public int? CartId { get; set; }

    [Column("customer_id")]
    public int? CustomerId { get; set; }

    [ForeignKey("CartId")]
    [InverseProperty("Invoices")]
    [JsonIgnore]
    public virtual CartMaster? Cart { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Invoices")]
    [JsonIgnore]
    public virtual CustomerMaster? Customer { get; set; }

    [InverseProperty("Invoice")]
    [JsonIgnore]
    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
}
