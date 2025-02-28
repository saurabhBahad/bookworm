using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Models;

[Table("rent_details")]
[Index("CustomerId", Name = "FKibhwskuwhrxv3d99r8vyd8xv2")]
[Index("ProductId", Name = "FKkn9g6d3w8jyxy23y1yree59f0")]
public partial class RentDetail
{
    [Key]
    [Column("rent_id")]
    public int RentId { get; set; }

    [Column("rent_end_date")]
    public DateOnly? RentEndDate { get; set; }

    [Column("rent_start_date")]
    public DateOnly? RentStartDate { get; set; }

    [Column("rent_status")]
    [StringLength(255)]
    public string? RentStatus { get; set; }

    [Column("customer_id")]
    public int? CustomerId { get; set; }

    [Column("product_id")]
    public int? ProductId { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("RentDetails")]
    public virtual CustomerMaster? Customer { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("RentDetails")]
    public virtual ProductMaster? Product { get; set; }

    [InverseProperty("Rent")]
    [JsonIgnore]
    public virtual ICollection<ShelfDetail> ShelfDetails { get; set; } = new List<ShelfDetail>();
}
