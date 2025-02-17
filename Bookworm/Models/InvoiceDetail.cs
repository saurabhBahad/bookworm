using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Models;

[Table("invoice_details")]
[Index("ProductId", Name = "FK1anfj9yh7l91txbjf905la63l")]
[Index("InvoiceId", Name = "FKpc7xa72mljy7weoct7uubgjy7")]
public partial class InvoiceDetail
{
    [Key]
    [Column("inv_dtl_id")]
    public int InvDtlId { get; set; }

    [Column("base_price")]
    public double? BasePrice { get; set; }

    [Column("rent_no_of_days")]
    public int? RentNoOfDays { get; set; }

    [Column("sale_price")]
    public double? SalePrice { get; set; }

    [Column("tran_type")]
    [StringLength(255)]
    public string? TranType { get; set; }

    [Column("invoice_id")]
    public int? InvoiceId { get; set; }

    [Column("product_id")]
    public int? ProductId { get; set; }

    [ForeignKey("InvoiceId")]
    [InverseProperty("InvoiceDetails")]
    public virtual Invoice? Invoice { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("InvoiceDetails")]
    public virtual ProductMaster? Product { get; set; }
}
