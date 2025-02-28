using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Models;

[Table("author_master")]
public partial class AuthorMaster
{
    [Key]
    [Column("author_id")]
    public int AuthorId { get; set; }

    [Column("author_name")]
    [StringLength(255)]
    public string? AuthorName { get; set; }

    [InverseProperty("Author")]
    [JsonIgnore]
    public virtual ICollection<ProductMaster> ProductMasters { get; set; } = new List<ProductMaster>();
}
