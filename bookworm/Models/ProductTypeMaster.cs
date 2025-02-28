using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Models;

[Table("product_type_master")]
public partial class ProductTypeMaster
{
    [Key]
    [Column("type_id")]
    public int TypeId { get; set; }

    [Column("type_desc")]
    [StringLength(255)]
    public string? TypeDesc { get; set; }

    [InverseProperty("Type")]
    [JsonIgnore]
    public virtual ICollection<ProductMaster> ProductMasters { get; set; } = new List<ProductMaster>();
}
