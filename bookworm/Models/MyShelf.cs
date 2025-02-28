using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Models;

[Table("my_shelf")]
public partial class MyShelf
{
    [Key]
    [Column("shelf_id")]
    public int ShelfId { get; set; }

    [Column("no_of_books")]
    public int? NoOfBooks { get; set; }

    [InverseProperty("Shelf")]
    [JsonIgnore]
    public virtual CustomerMaster? CustomerMaster { get; set; }

    [InverseProperty("Shelf")]
    [JsonIgnore]
    public virtual ICollection<ShelfDetail> ShelfDetails { get; set; } = new List<ShelfDetail>();
}
