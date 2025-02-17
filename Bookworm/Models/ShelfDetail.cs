using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Models;

[Table("shelf_details")]
[Index("ShelfId", Name = "FK1e0rtn8mpuis2c1o6y2hj2b8i")]
[Index("ProductId", Name = "FKe25l2k2ixibv21bhqn9414fh5")]
[Index("RentId", Name = "FKfnnpae86nrfusj3evgjq33v62")]
public partial class ShelfDetail
{
    [Key]
    [Column("shelf_dtl_id")]
    public int ShelfDtlId { get; set; }

    [Column("base_price")]
    public double? BasePrice { get; set; }

    [Column("tran_type")]
    [StringLength(255)]
    public string? TranType { get; set; }

    [Column("product_id")]
    public int? ProductId { get; set; }

    [Column("rent_id")]
    public int? RentId { get; set; }

    [Column("shelf_id")]
    public int? ShelfId { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("ShelfDetails")]
    public virtual ProductMaster? Product { get; set; }

    [ForeignKey("RentId")]
    [InverseProperty("ShelfDetails")]
    public virtual RentDetail? Rent { get; set; }

    [ForeignKey("ShelfId")]
    [InverseProperty("ShelfDetails")]
    public virtual MyShelf? Shelf { get; set; }
}
