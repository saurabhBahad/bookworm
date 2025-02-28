using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Models;

[Table("product_arribute")]
[Index("AttributeId", Name = "FKbdigfhujyub7ojp7lirf5l6d0")]
[Index("ProductId", Name = "FKeu0ww30gewhci44umhb3we5x1")]
public partial class ProductArribute
{
    [Key]
    [Column("product_attribute_id")]
    public int ProductAttributeId { get; set; }

    [Column("attribute_value")]
    [StringLength(255)]
    public string? AttributeValue { get; set; }

    [Column("attribute_id")]
    public int? AttributeId { get; set; }

    [Column("product_id")]
    public int? ProductId { get; set; }

    [ForeignKey("AttributeId")]
    [InverseProperty("ProductArributes")]
    public virtual AttributeMaster? Attribute { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("ProductArributes")]
    public virtual ProductMaster? Product { get; set; }
}
