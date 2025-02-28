using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Models;

[Table("genre_master")]
public partial class GenreMaster
{
    [Key]
    [Column("genre_id")]
    public int GenreId { get; set; }

    [Column("genre_desc")]
    [StringLength(255)]
    public string? GenreDesc { get; set; }

    [InverseProperty("Genre")]
    [JsonIgnore]
    public virtual ICollection<ProductMaster> ProductMasters { get; set; } = new List<ProductMaster>();
}
