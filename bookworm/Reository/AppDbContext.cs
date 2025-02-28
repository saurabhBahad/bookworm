
using Bookworm.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Repository;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AttributeMaster> AttributeMasters { get; set; }

    public virtual DbSet<AuthorMaster> AuthorMasters { get; set; }

    public virtual DbSet<BeneficiaryMaster> BeneficiaryMasters { get; set; }

    public virtual DbSet<CartDetail> CartDetails { get; set; }

    public virtual DbSet<CartMaster> CartMasters { get; set; }

    public virtual DbSet<CustomerMaster> CustomerMasters { get; set; }

    public virtual DbSet<GenreMaster> GenreMasters { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; }

    public virtual DbSet<LanguageMaster> LanguageMasters { get; set; }

    public virtual DbSet<MyShelf> MyShelves { get; set; }

    public virtual DbSet<ProductArribute> ProductArributes { get; set; }

    public virtual DbSet<ProductBeneficiary> ProductBeneficiaries { get; set; }

    public virtual DbSet<ProductMaster> ProductMasters { get; set; }

    public virtual DbSet<ProductTypeMaster> ProductTypeMasters { get; set; }

    public virtual DbSet<RentDetail> RentDetails { get; set; }

    public virtual DbSet<RoyaltyCalculation> RoyaltyCalculations { get; set; }

    public virtual DbSet<ShelfDetail> ShelfDetails { get; set; }

//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//         => optionsBuilder.UseMySql("server=localhost;database=bookworm;user=root;password=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.39-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<AttributeMaster>(entity =>
        {
            entity.HasKey(e => e.AttributeId).HasName("PRIMARY");
        });

        modelBuilder.Entity<AuthorMaster>(entity =>
        {
            entity.HasKey(e => e.AuthorId).HasName("PRIMARY");
        });

        modelBuilder.Entity<BeneficiaryMaster>(entity =>
        {
            entity.HasKey(e => e.BenId).HasName("PRIMARY");
        });

        modelBuilder.Entity<CartDetail>(entity =>
        {
            entity.HasKey(e => e.CartDetailsId).HasName("PRIMARY");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartDetails).HasConstraintName("FK5u7nakxaradawhygg84syvu92");

            entity.HasOne(d => d.Product).WithMany(p => p.CartDetails).HasConstraintName("FKlfyc1r1caest795hguh2nto2m");
        });

        modelBuilder.Entity<CartMaster>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PRIMARY");

            entity.HasOne(d => d.Customer).WithMany(p => p.CartMasters).HasConstraintName("FK44sbajofqx6cngygmmwui5igc");
        });

        modelBuilder.Entity<CustomerMaster>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PRIMARY");

            entity.HasOne(d => d.Shelf).WithOne(p => p.CustomerMaster).HasConstraintName("FKmphhgq22uo5h5jerykgxsf7f7");
        });

        modelBuilder.Entity<GenreMaster>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("PRIMARY");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PRIMARY");

            entity.Property(e => e.Amount).HasDefaultValueSql("'0'");

            entity.HasOne(d => d.Cart).WithMany(p => p.Invoices).HasConstraintName("FK74rjp8604l111tb50mbg1ubbd");

            entity.HasOne(d => d.Customer).WithMany(p => p.Invoices).HasConstraintName("FKk9j7m0iwl2u5ccibh3piocfj");
        });

        modelBuilder.Entity<InvoiceDetail>(entity =>
        {
            entity.HasKey(e => e.InvDtlId).HasName("PRIMARY");

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceDetails).HasConstraintName("FKpc7xa72mljy7weoct7uubgjy7");

            entity.HasOne(d => d.Product).WithMany(p => p.InvoiceDetails).HasConstraintName("FK1anfj9yh7l91txbjf905la63l");
        });

        modelBuilder.Entity<LanguageMaster>(entity =>
        {
            entity.HasKey(e => e.LanguageId).HasName("PRIMARY");
        });

        modelBuilder.Entity<MyShelf>(entity =>
        {
            entity.HasKey(e => e.ShelfId).HasName("PRIMARY");

            entity.Property(e => e.NoOfBooks).HasDefaultValueSql("'0'");
        });

        modelBuilder.Entity<ProductArribute>(entity =>
        {
            entity.HasKey(e => e.ProductAttributeId).HasName("PRIMARY");

            entity.HasOne(d => d.Attribute).WithMany(p => p.ProductArributes).HasConstraintName("FKbdigfhujyub7ojp7lirf5l6d0");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductArributes).HasConstraintName("FKeu0ww30gewhci44umhb3we5x1");
        });

        modelBuilder.Entity<ProductBeneficiary>(entity =>
        {
            entity.HasKey(e => e.ProductBeneficiaryId).HasName("PRIMARY");

            entity.HasOne(d => d.Beneficiary).WithMany(p => p.ProductBeneficiaries).HasConstraintName("FKivxugs1htmu5356ka6adepyo4");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductBeneficiaries).HasConstraintName("FKimuuqbtoxmdkej3yb1rhq7qoh");
        });

        modelBuilder.Entity<ProductMaster>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PRIMARY");

            entity.Property(e => e.MinRentDays).HasDefaultValueSql("'3'");

            entity.HasOne(d => d.Author).WithMany(p => p.ProductMasters).HasConstraintName("FKotv711yebb5lsr8crrjsx13ke");

            entity.HasOne(d => d.Genre).WithMany(p => p.ProductMasters).HasConstraintName("FKceskcho96iufjsecekgohckua");

            entity.HasOne(d => d.Language).WithMany(p => p.ProductMasters).HasConstraintName("FK98ccg011o5dskuffl8qf7o7kk");

            entity.HasOne(d => d.Type).WithMany(p => p.ProductMasters).HasConstraintName("FKkqx9yklv6gwn0rx53udabv5bd");
        });

        modelBuilder.Entity<ProductTypeMaster>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PRIMARY");
        });

        modelBuilder.Entity<RentDetail>(entity =>
        {
            entity.HasKey(e => e.RentId).HasName("PRIMARY");

            entity.HasOne(d => d.Customer).WithMany(p => p.RentDetails).HasConstraintName("FKibhwskuwhrxv3d99r8vyd8xv2");

            entity.HasOne(d => d.Product).WithMany(p => p.RentDetails).HasConstraintName("FKkn9g6d3w8jyxy23y1yree59f0");
        });

        modelBuilder.Entity<RoyaltyCalculation>(entity =>
        {
            entity.HasKey(e => e.RoyCalId).HasName("PRIMARY");

            entity.HasOne(d => d.BeneficiaryMasterBen).WithMany(p => p.RoyaltyCalculations).HasConstraintName("FKa9pdowai5o2poxgxqunct3jqp");

            entity.HasOne(d => d.ProductProduct).WithMany(p => p.RoyaltyCalculations).HasConstraintName("FK9foyl4957umjemojjuynv4kg2");
        });

        modelBuilder.Entity<ShelfDetail>(entity =>
        {
            entity.HasKey(e => e.ShelfDtlId).HasName("PRIMARY");

            entity.HasOne(d => d.Product).WithMany(p => p.ShelfDetails).HasConstraintName("FKe25l2k2ixibv21bhqn9414fh5");

            entity.HasOne(d => d.Rent).WithMany(p => p.ShelfDetails).HasConstraintName("FKfnnpae86nrfusj3evgjq33v62");

            entity.HasOne(d => d.Shelf).WithMany(p => p.ShelfDetails).HasConstraintName("FK1e0rtn8mpuis2c1o6y2hj2b8i");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
