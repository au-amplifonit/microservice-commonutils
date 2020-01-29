using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Fox.Microservices.CommonUtils.Models.Entities
{
    public partial class CommonUtilsContext : DbContext
    {
        public CommonUtilsContext()
        {
        }

        public CommonUtilsContext(DbContextOptions<CommonUtilsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CM_B_COUNTER> CM_B_COUNTER { get; set; }
        public virtual DbSet<CM_S_AREA_BOOK> CM_S_AREA_BOOK { get; set; }
        public virtual DbSet<CM_S_CITY_BOOK> CM_S_CITY_BOOK { get; set; }
        public virtual DbSet<CM_S_STATE_EXT_AUS> CM_S_STATE_EXT_AUS { get; set; }
        public virtual DbSet<SY_GLOBAL_PARAMETER> SY_GLOBAL_PARAMETER { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=CAU02DB01FOXSIT.D09.ROOT.SYS;Database=FoxAustralia_SIT2;Trusted_Connection=False;User ID=foxuser;Password=Df0x35ZZ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<CM_B_COUNTER>(entity =>
            {
                entity.HasKey(e => new { e.COMPANY_CODE, e.DIVISION_CODE, e.SHOP_CODE, e.LAPTOP_CODE, e.TABLE_NAME, e.FIELD_NAME });

                entity.HasIndex(e => e.ROWGUID)
                    .HasName("UQ_CM_B_COUNTER_ROWGUID")
                    .IsUnique();

                entity.HasIndex(e => new { e.SHOP_CODE, e.LAPTOP_CODE, e.VALUE, e.COMPANY_CODE, e.DIVISION_CODE, e.TABLE_NAME, e.FIELD_NAME })
                    .HasName("missing_index_31919");

                entity.HasIndex(e => new { e.SHOP_CODE, e.VALUE, e.MIN_VALUE, e.MAX_VALUE, e.COMPANY_CODE, e.DIVISION_CODE, e.TABLE_NAME, e.FIELD_NAME })
                    .HasName("missing_index_31922");

                entity.Property(e => e.COMPANY_CODE).HasMaxLength(3);

                entity.Property(e => e.DIVISION_CODE).HasMaxLength(3);

                entity.Property(e => e.SHOP_CODE).HasMaxLength(3);

                entity.Property(e => e.LAPTOP_CODE).HasMaxLength(3);

                entity.Property(e => e.TABLE_NAME).HasMaxLength(50);

                entity.Property(e => e.FIELD_NAME).HasMaxLength(50);

                entity.Property(e => e.DATEFIELDFORCHECK).HasMaxLength(50);

                entity.Property(e => e.DT_END).HasColumnType("date");

                entity.Property(e => e.DT_INSERT).HasColumnType("datetime");

                entity.Property(e => e.DT_START).HasColumnType("date");

                entity.Property(e => e.DT_UPDATE).HasColumnType("datetime");

                entity.Property(e => e.FLG_AUTOMOVENEXT).HasMaxLength(1);

                entity.Property(e => e.FLG_BASE36).HasMaxLength(1);

                entity.Property(e => e.FLG_MANUAL_RESET).HasMaxLength(1);

                entity.Property(e => e.ROWGUID).HasDefaultValueSql("(newid())");

                entity.Property(e => e.USERINSERT).HasMaxLength(50);

                entity.Property(e => e.USERUPDATE).HasMaxLength(50);
            });

            modelBuilder.Entity<CM_S_AREA_BOOK>(entity =>
            {
                entity.HasKey(e => new { e.COUNTRY_CODE, e.AREA_CODE })
                    .HasName("PK_CM_AREA_BOOK");

                entity.HasIndex(e => e.ROWGUID)
                    .HasName("UQ_CM_S_AREA_BOOK_ROWGUID")
                    .IsUnique();

                entity.Property(e => e.COUNTRY_CODE).HasMaxLength(3);

                entity.Property(e => e.AREA_CODE).HasMaxLength(3);

                entity.Property(e => e.AREA_DESCR).HasMaxLength(255);

                entity.Property(e => e.DT_END).HasColumnType("date");

                entity.Property(e => e.DT_INSERT).HasColumnType("datetime");

                entity.Property(e => e.DT_START).HasColumnType("date");

                entity.Property(e => e.DT_UPDATE).HasColumnType("datetime");

                entity.Property(e => e.REGION_CODE).HasMaxLength(2);

                entity.Property(e => e.ROWGUID).HasDefaultValueSql("(newid())");

                entity.Property(e => e.USERINSERT).HasMaxLength(50);

                entity.Property(e => e.USERUPDATE).HasMaxLength(50);
            });

            modelBuilder.Entity<CM_S_CITY_BOOK>(entity =>
            {
                entity.HasKey(e => new { e.COUNTRY_CODE, e.AREA_CODE, e.ZIP_CODE, e.CITY_COUNTER })
                    .HasName("PK_CM_CITY_BOOK");

                entity.HasIndex(e => e.CITY_NAME);

                entity.HasIndex(e => e.ROWGUID)
                    .HasName("UQ_CM_S_CITY_BOOK_ROWGUID")
                    .IsUnique();

                entity.HasIndex(e => e.ZIP_CODE);

                entity.HasIndex(e => new { e.COUNTRY_CODE, e.AREA_CODE })
                    .HasName("IDX_CM_S_CITY_BOOK_CM_S_AREA_BOOK");

                entity.Property(e => e.COUNTRY_CODE).HasMaxLength(3);

                entity.Property(e => e.AREA_CODE).HasMaxLength(3);

                entity.Property(e => e.ZIP_CODE).HasMaxLength(15);

                entity.Property(e => e.CITY_NAME).HasMaxLength(25);

                entity.Property(e => e.DT_INSERT).HasColumnType("datetime");

                entity.Property(e => e.DT_UPDATE).HasColumnType("datetime");

                entity.Property(e => e.ROWGUID).HasDefaultValueSql("(newid())");

                entity.Property(e => e.USERINSERT).HasMaxLength(50);

                entity.Property(e => e.USERUPDATE).HasMaxLength(50);

                entity.Property(e => e.ZIP_CODE_UNIQUE_ID).HasMaxLength(50);

                entity.HasOne(d => d.CM_S_AREA_BOOK)
                    .WithMany(p => p.CM_S_CITY_BOOK)
                    .HasForeignKey(d => new { d.COUNTRY_CODE, d.AREA_CODE })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CM_S_CITY_BOOK_CM_S_AREA_BOOK");
            });

            modelBuilder.Entity<CM_S_STATE_EXT_AUS>(entity =>
            {
                entity.HasKey(e => new { e.COMPANY_CODE, e.DIVISION_CODE, e.STATE_CODE });

                entity.HasIndex(e => e.ROWGUID)
                    .HasName("UQ_CM_S_STATE_EXT_AUS_ROWGUID")
                    .IsUnique();

                entity.HasIndex(e => new { e.COMPANY_CODE, e.DIVISION_CODE, e.STATE_CODE })
                    .HasName("IX_CM_S_STATE_EXT_AUS")
                    .IsUnique();

                entity.Property(e => e.COMPANY_CODE).HasMaxLength(3);

                entity.Property(e => e.DIVISION_CODE).HasMaxLength(3);

                entity.Property(e => e.STATE_CODE).HasMaxLength(3);

                entity.Property(e => e.DEFAULT_AREA_CODE).HasMaxLength(3);

                entity.Property(e => e.DT_INSERT).HasColumnType("datetime");

                entity.Property(e => e.DT_UPDATE).HasColumnType("datetime");

                entity.Property(e => e.ROWGUID).HasDefaultValueSql("(newid())");

                entity.Property(e => e.STATE_NAME).HasMaxLength(50);

                entity.Property(e => e.USERINSERT).HasMaxLength(50);

                entity.Property(e => e.USERUPDATE).HasMaxLength(50);
            });

            modelBuilder.Entity<SY_GLOBAL_PARAMETER>(entity =>
            {
                entity.HasKey(e => new { e.COMPANY_CODE, e.DIVISION_CODE, e.SHOP_CODE, e.PARAMETER_NAME });

                entity.HasIndex(e => e.PARAMETER_NAME)
                    .HasName("missing_index_24701");

                entity.HasIndex(e => e.ROWGUID)
                    .HasName("UQ_SY_GLOBAL_PARAMETER_ROWGUID")
                    .IsUnique();

                entity.HasIndex(e => new { e.CONTROLGROUP_CODE, e.PARAMETER_NAME })
                    .HasName("IX_SY_GLOBAL_PARAMETER_CONTROLGROUPCODE_PARAMETERNAME");

                entity.HasIndex(e => new { e.SHOP_CODE, e.PARAMETER_NAME })
                    .HasName("missing_index_25141");

                entity.HasIndex(e => new { e.COMPANY_CODE, e.DIVISION_CODE, e.CONTROLGROUP_CODE })
                    .HasName("IDX_SY_GLOBAL_PARAMETER_SY_CONTROL_GROUP");

                entity.HasIndex(e => new { e.COMPANY_CODE, e.DIVISION_CODE, e.SHOP_CODE })
                    .HasName("IDX_SY_GLOBAL_PARAMETER_CM_B_SHOP");

                entity.HasIndex(e => new { e.PARAMETER_NAME, e.DT_START, e.DT_END })
                    .HasName("missing_index_17745");

                entity.Property(e => e.COMPANY_CODE).HasMaxLength(3);

                entity.Property(e => e.DIVISION_CODE).HasMaxLength(3);

                entity.Property(e => e.SHOP_CODE).HasMaxLength(3);

                entity.Property(e => e.PARAMETER_NAME).HasMaxLength(50);

                entity.Property(e => e.CONTROLGROUP_CODE).HasMaxLength(3);

                entity.Property(e => e.DT_END).HasColumnType("date");

                entity.Property(e => e.DT_INSERT).HasColumnType("datetime");

                entity.Property(e => e.DT_START).HasColumnType("date");

                entity.Property(e => e.DT_UPDATE).HasColumnType("datetime");

                entity.Property(e => e.PARAMETER_VALUE).HasMaxLength(500);

                entity.Property(e => e.USERINSERT).HasMaxLength(50);

                entity.Property(e => e.USERUPDATE).HasMaxLength(50);
            });

            modelBuilder.HasSequence("NextFoxid").StartsAt(0);

            modelBuilder.HasSequence("GETNEXTBATCHNUMBER").StartsAt(200);

            modelBuilder.HasSequence("NextFoxCommonVoucherId");

            modelBuilder.HasSequence("NextFoxCouponid").StartsAt(0);

            modelBuilder.HasSequence("NextFoxid").StartsAt(4000);

            modelBuilder.HasSequence("NextFoxVoucherId");
        }
    }
}
