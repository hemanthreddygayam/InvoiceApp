using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace InvoiceApplication.DbModels
{
    public partial class TrackingDbContext : DbContext
    {
        public virtual DbSet<BtsinvoiceAr> BtsinvoiceAr { get; set; }
        public virtual DbSet<Btsuser> Btsuser { get; set; }
        public virtual DbSet<UserEmail> UserEmail { get; set; }
        public virtual DbSet<VesselDocuments> VesselDocuments { get; set; }

        public TrackingDbContext(DbContextOptions<TrackingDbContext> options) : base(options) { }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BtsinvoiceAr>(entity =>
            {
                entity.HasKey(e => e.InvoiceId);

                entity.ToTable("BTSInvoiceAR");

                entity.Property(e => e.InvoiceId).ValueGeneratedNever();

                entity.Property(e => e.AccountDate).HasColumnType("datetime");

                entity.Property(e => e.ApprovedBy)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ApprovedDate)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.BillingAddress1)
                    .IsRequired()
                    .HasMaxLength(19)
                    .IsUnicode(false);

                entity.Property(e => e.BillingAddress2)
                    .IsRequired()
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.BillingCountry)
                    .IsRequired()
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.CheckedBy)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.CheckedDate)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreditTerms)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.CurrencyCode)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerCode)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerName)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.DeliveryDate).HasColumnType("datetime");

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.EditBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EditDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNo)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TotalAmt).HasColumnType("money");

                entity.Property(e => e.TotalLocalAmt).HasColumnType("money");
            });

            modelBuilder.Entity<Btsuser>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("BTSUser");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Designation)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserCode)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UserEmail>(entity =>
            {
                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VesselDocuments>(entity =>
            {
                entity.HasKey(e => e.FileId);

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FilePath)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });
        }
    }
}
