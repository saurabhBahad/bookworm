using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Models;

[Table("product_beneficiary")]
[Index("ProductId", Name = "FKimuuqbtoxmdkej3yb1rhq7qoh")]
[Index("BeneficiaryId", Name = "FKivxugs1htmu5356ka6adepyo4")]
public partial class ProductBeneficiary
{
    [Key]
    [Column("product_beneficiary_id")]
    public int ProductBeneficiaryId { get; set; }

    [Column("percentage")]
    public double? Percentage { get; set; }

    [Column("beneficiary_id")]
    public int? BeneficiaryId { get; set; }

    [Column("product_id")]
    public int? ProductId { get; set; }

    [ForeignKey("BeneficiaryId")]
    [InverseProperty("ProductBeneficiaries")]
    public virtual BeneficiaryMaster? Beneficiary { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("ProductBeneficiaries")]
    public virtual ProductMaster? Product { get; set; }
}
