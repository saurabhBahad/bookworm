using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Models;

[Table("royalty_calculation")]
[Index("ProductProductId", Name = "FK9foyl4957umjemojjuynv4kg2")]
[Index("BeneficiaryMasterBenId", Name = "FKa9pdowai5o2poxgxqunct3jqp")]
public partial class RoyaltyCalculation
{
    [Key]
    [Column("roy_cal_id")]
    public int RoyCalId { get; set; }

    [Column("base_price")]
    public double BasePrice { get; set; }

    [Column("royalty_on_base_price")]
    public double RoyaltyOnBasePrice { get; set; }

    [Column("royalty_tran_date")]
    public DateOnly RoyaltyTranDate { get; set; }

    [Column("sales_price")]
    public double SalesPrice { get; set; }

    [Column("transaction_type")]
    [StringLength(255)]
    public string TransactionType { get; set; } = null!;

    [Column("beneficiary_master_ben_id")]
    public int? BeneficiaryMasterBenId { get; set; }

    [Column("product_product_id")]
    public int? ProductProductId { get; set; }

    [ForeignKey("BeneficiaryMasterBenId")]
    [InverseProperty("RoyaltyCalculations")]
    public virtual BeneficiaryMaster? BeneficiaryMasterBen { get; set; }

    [ForeignKey("ProductProductId")]
    [InverseProperty("RoyaltyCalculations")]
    public virtual ProductMaster? ProductProduct { get; set; }
}
