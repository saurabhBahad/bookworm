using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Models;

[Table("product_master")]
[Index("LanguageId", Name = "FK98ccg011o5dskuffl8qf7o7kk")]
[Index("GenreId", Name = "FKceskcho96iufjsecekgohckua")]
[Index("TypeId", Name = "FKkqx9yklv6gwn0rx53udabv5bd")]
[Index("AuthorId", Name = "FKotv711yebb5lsr8crrjsx13ke")]
public partial class ProductMaster
{
    [Key]
    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("is_rentable", TypeName = "bit(1)")]
    public Boolean IsRentable { get; set; }

    [Column("min_rent_days")]
    public int? MinRentDays { get; set; }

    [Column("product_base_price")]
    public double? ProductBasePrice { get; set; }

    [Column("product_description_long")]
    [StringLength(255)]
    public string? ProductDescriptionLong { get; set; }

    [Column("product_description_short")]
    [StringLength(255)]
    public string? ProductDescriptionShort { get; set; }

    [Column("product_english_name")]
    [StringLength(255)]
    public string? ProductEnglishName { get; set; }

    [Column("product_isbn")]
    [StringLength(255)]
    public string? ProductIsbn { get; set; }

    [Column("product_name")]
    [StringLength(255)]
    public string? ProductName { get; set; }

    [Column("product_off_price_expiry_date")]
    public DateOnly? ProductOffPriceExpiryDate { get; set; }

    [Column("product_offer_price")]
    public double? ProductOfferPrice { get; set; }

    [Column("product_path")]
    [StringLength(255)]
    public string ProductPath { get; set; } = null!;

    [Column("product_sp_cost")]
    public double? ProductSpCost { get; set; }

    [Column("rent_per_day")]
    public double? RentPerDay { get; set; }

    [Column("author_id")]
    public int? AuthorId { get; set; }

    [Column("genre_id")]
    public int? GenreId { get; set; }

    [Column("language_id")]
    public int? LanguageId { get; set; }

    [Column("type_id")]
    public int? TypeId { get; set; }

    [ForeignKey("AuthorId")]
    [InverseProperty("ProductMasters")]
    public virtual AuthorMaster? Author { get; set; }

    [InverseProperty("Product")]
    [JsonIgnore]
    public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();

    [ForeignKey("GenreId")]
    [InverseProperty("ProductMasters")]
    public virtual GenreMaster? Genre { get; set; }

    [InverseProperty("Product")]
    [JsonIgnore]
    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();

    [ForeignKey("LanguageId")]
    [InverseProperty("ProductMasters")]
    public virtual LanguageMaster? Language { get; set; }

    [InverseProperty("Product")]
    [JsonIgnore]
    public virtual ICollection<ProductArribute> ProductArributes { get; set; } = new List<ProductArribute>();

    [InverseProperty("Product")]
    [JsonIgnore]
    public virtual ICollection<ProductBeneficiary> ProductBeneficiaries { get; set; } = new List<ProductBeneficiary>();

    [InverseProperty("Product")]
    [JsonIgnore]
    public virtual ICollection<RentDetail> RentDetails { get; set; } = new List<RentDetail>();

    [InverseProperty("ProductProduct")]
    [JsonIgnore]
    public virtual ICollection<RoyaltyCalculation> RoyaltyCalculations { get; set; } = new List<RoyaltyCalculation>();

    [InverseProperty("Product")]
    [JsonIgnore]
    public virtual ICollection<ShelfDetail> ShelfDetails { get; set; } = new List<ShelfDetail>();

    [ForeignKey("TypeId")]
    [InverseProperty("ProductMasters")]
    public virtual ProductTypeMaster? Type { get; set; }
}
