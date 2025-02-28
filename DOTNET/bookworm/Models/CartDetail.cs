using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Models;

[Table("cart_details")]
[Index("CartId", Name = "FK5u7nakxaradawhygg84syvu92")]
[Index("ProductId", Name = "FKlfyc1r1caest795hguh2nto2m")]
public partial class CartDetail
{
    [Key]
    [Column("cart_details_id")]
    public int CartDetailsId { get; set; }

    [Column("is_rented", TypeName = "bit(1)")]
    public Boolean IsRented { get; set; }

    [Column("rent_no_of_days")]
    public int? RentNoOfDays { get; set; }

    [Column("cart_id")]
    public int? CartId { get; set; }

    [Column("product_id")]
    public int? ProductId { get; set; }

    [ForeignKey("CartId")]
    [InverseProperty("CartDetails")]    
    [JsonIgnore]
    public virtual CartMaster? Cart { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("CartDetails")]
    public virtual ProductMaster? Product { get; set; }
}
