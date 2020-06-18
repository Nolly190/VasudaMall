namespace VasudaDataAccess.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class VasudaModel : DbContext
    {
        public VasudaModel()
            : base("name=VasudaModel")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AuditTrialTable> AuditTrialTables { get; set; }
        public virtual DbSet<CategoryTable> CategoryTables { get; set; }
        public virtual DbSet<ExchangeRateTable> ExchangeRateTables { get; set; }
        public virtual DbSet<ImageTable> ImageTables { get; set; }
        public virtual DbSet<OrderTable> OrderTables { get; set; }
        public virtual DbSet<PaymentHistoryTable> PaymentHistoryTables { get; set; }
        public virtual DbSet<PriceTable> PriceTables { get; set; }
        public virtual DbSet<ProductTable> ProductTables { get; set; }
        public virtual DbSet<SettingTable> SettingTables { get; set; }
        public virtual DbSet<SubCategoryTable> SubCategoryTables { get; set; }
        public virtual DbSet<SupportTable> SupportTables { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<VendorTable> Vendors { get; set; }
        public virtual DbSet<NotificationTable> NotificationTables { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.NotificationTables)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.OrderTables)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.SupportTables)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CategoryTable>()
                .HasMany(e => e.SubCategoryTables)
                .WithRequired(e => e.CategoryTable)
                .HasForeignKey(e => e.CategoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OrderTable>()
                .HasMany(e => e.ImageTables)
                .WithOptional(e => e.OrderTable)
                .HasForeignKey(e => e.OrderId);

            modelBuilder.Entity<ProductTable>()
                .HasMany(e => e.ImageTables)
                .WithOptional(e => e.ProductTable)
                .HasForeignKey(e => e.ProductId);

            modelBuilder.Entity<VendorTable>()
                .HasMany(e => e.OrderTables)
                .WithRequired(e => e.Vendor)
                .WillCascadeOnDelete(false);
        }
    }
}
