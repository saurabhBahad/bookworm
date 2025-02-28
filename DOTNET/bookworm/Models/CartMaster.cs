using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Models;

[Table("cart_master")]
[Index("CustomerId", Name = "FK44sbajofqx6cngygmmwui5igc")]
public partial class CartMaster
{
    [Key]
    [Column("cart_id")]
    public int CartId { get; set; }

    [Column("cost")]
    public double? Cost { get; set; }

    [Column("is_active", TypeName = "bit(1)")]
    public Boolean IsActive { get; set; }

    [Column("customer_id")]
    public int? CustomerId { get; set; }

    [InverseProperty("Cart")]
    [JsonIgnore]
    public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();

    [ForeignKey("CustomerId")]
    [InverseProperty("CartMasters")]
    [JsonIgnore]
    public virtual CustomerMaster? Customer { get; set; }

    [InverseProperty("Cart")]
    [JsonIgnore]
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

}
