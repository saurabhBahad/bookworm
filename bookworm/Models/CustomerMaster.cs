using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Models;

[Table("customer_master")]
[Index("ShelfId", Name = "UKi98kbs8t81f7lpo1jugxjk2dh", IsUnique = true)]
[Index("Email", Name = "UKsnf65l86t4b0xj6v0f9nymegs", IsUnique = true)]
public partial class CustomerMaster
{
    [Key]
    [Column("customer_id")]
    public int CustomerId { get; set; }

    [Column("age")]
    public int? Age { get; set; }

    [Column("dob")]
    public DateOnly? Dob { get; set; }

    [Column("email")]
    public string Email { get; set; } = null!;

    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Column("pan")]
    [StringLength(255)]
    public string? Pan { get; set; }

    [Column("password")]
    [StringLength(255)]
    //[JsonIgnore]
    public string Password { get; set; } = null!;

    [Column("phone_number")]
    [StringLength(255)]
    public string? PhoneNumber { get; set; }

    [Column("shelf_id")]
    public int? ShelfId { get; set; }

    [InverseProperty("Customer")]
    [JsonIgnore]
    public virtual ICollection<CartMaster> CartMasters { get; set; } = new List<CartMaster>();
    

    [InverseProperty("Customer")]
    [JsonIgnore]
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    [InverseProperty("Customer")]
    [JsonIgnore]
    public virtual ICollection<RentDetail> RentDetails { get; set; } = new List<RentDetail>();

    [ForeignKey("ShelfId")]
    [InverseProperty("CustomerMaster")]
    [JsonIgnore]

    public virtual MyShelf? Shelf { get; set; }
}
