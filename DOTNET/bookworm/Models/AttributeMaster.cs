using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Models;

[Table("attribute_master")]
public partial class AttributeMaster
{
    [Key]
    [Column("attribute_id")]
    public int AttributeId { get; set; }

    [Column("attribute_name")]
    [StringLength(255)]
    public string? AttributeName { get; set; }

    [InverseProperty("Attribute")]
    [JsonIgnore]
    public virtual ICollection<ProductArribute> ProductArributes { get; set; } = new List<ProductArribute>();
}
