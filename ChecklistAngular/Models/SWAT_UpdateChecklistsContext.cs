using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ChecklistAngular.Models
{
    public partial class SWAT_UpdateChecklistsContext : DbContext
    {
        public SWAT_UpdateChecklistsContext()
        {
        }

        public SWAT_UpdateChecklistsContext(DbContextOptions<SWAT_UpdateChecklistsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ListPpack> ListPpack { get; set; }
        public virtual DbSet<ListProcess> ListProcess { get; set; }
        public virtual DbSet<ListProdLine> ListProdLine { get; set; }
        public virtual DbSet<ListRelease> ListRelease { get; set; }
        public virtual DbSet<ListSite> ListSite { get; set; }
        public virtual DbSet<ListStatusChecklist> ListStatusChecklist { get; set; }
        public virtual DbSet<ListStatusUpdate> ListStatusUpdate { get; set; }
        public virtual DbSet<ListSystem> ListSystem { get; set; }
        public virtual DbSet<ListType> ListType { get; set; }
        public virtual DbSet<ListUserApprove> ListUserApprove { get; set; }
        public virtual DbSet<LogChecklist> LogChecklist { get; set; }
        public virtual DbSet<LogChecklistHistory> LogChecklistHistory { get; set; }
        public virtual DbSet<LogChecklistIndex> LogChecklistIndex { get; set; }
        public virtual DbSet<LogChecklistSteps> LogChecklistSteps { get; set; }
        public virtual DbSet<LogUpdate> LogUpdate { get; set; }
        public virtual DbSet<LogUpdateHistory> LogUpdateHistory { get; set; }
        public virtual DbSet<LogUpdateSteps> LogUpdateSteps { get; set; }

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ListPpack>(entity =>
            {
                entity.HasKey(e => e.Ppack);

                entity.ToTable("ListPPack");

                entity.Property(e => e.Ppack)
                    .HasColumnName("PPack")
                    .HasColumnType("varchar(5)");
            });

            modelBuilder.Entity<ListProcess>(entity =>
            {
                entity.HasKey(e => e.Process);

                entity.Property(e => e.Process).HasColumnType("varchar(50)");

                entity.Property(e => e.SortOrder).HasColumnType("smallint(6)");
            });

            modelBuilder.Entity<ListProdLine>(entity =>
            {
                entity.HasKey(e => e.ProdLine);

                entity.Property(e => e.ProdLine).HasColumnType("varchar(10)");

                entity.Property(e => e.ProdLineName).HasColumnType("varchar(25)");
            });

            modelBuilder.Entity<ListRelease>(entity =>
            {
                entity.HasKey(e => e.Rel);

                entity.Property(e => e.Rel).HasColumnType("varchar(10)");
            });

            modelBuilder.Entity<ListSite>(entity =>
            {
                entity.HasKey(e => new { e.ProdLine, e.SiteKml });

                entity.Property(e => e.ProdLine).HasColumnType("varchar(10)");

                entity.Property(e => e.SiteKml).HasColumnType("varchar(25)");

                entity.Property(e => e.SiteName).HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<ListStatusChecklist>(entity =>
            {
                entity.HasKey(e => e.Status);

                entity.Property(e => e.Status).HasColumnType("varchar(25)");

                entity.Property(e => e.SortOrder).HasColumnType("smallint(6)");
            });

            modelBuilder.Entity<ListStatusUpdate>(entity =>
            {
                entity.HasKey(e => e.Status);

                entity.Property(e => e.Status).HasColumnType("varchar(25)");

                entity.Property(e => e.SortOrder).HasColumnType("smallint(6)");
            });

            modelBuilder.Entity<ListSystem>(entity =>
            {
                entity.HasKey(e => e.System);

                entity.Property(e => e.System).HasColumnType("varchar(25)");

                entity.Property(e => e.SortOrder).HasColumnType("smallint(6)");
            });

            modelBuilder.Entity<ListType>(entity =>
            {
                entity.HasKey(e => e.Type);

                entity.Property(e => e.Type).HasColumnType("varchar(25)");

                entity.Property(e => e.SortOrder).HasColumnType("smallint(6)");
            });

            modelBuilder.Entity<ListUserApprove>(entity =>
            {
                entity.HasKey(e => e.UserApprove);

                entity.Property(e => e.UserApprove).HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<LogChecklist>(entity =>
            {
                entity.HasKey(e => new { e.Idchecklist, e.Version });

                entity.Property(e => e.Idchecklist)
                    .HasColumnName("IDchecklist")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Version).HasColumnType("smallint(6)");

                entity.Property(e => e.Process).HasColumnType("varchar(50)");

                entity.Property(e => e.ProdLine).HasColumnType("varchar(10)");

                entity.Property(e => e.Rel).HasColumnType("varchar(10)");

                entity.Property(e => e.Scope).HasColumnType("text");

                entity.Property(e => e.Status).HasColumnType("varchar(25)");

                entity.Property(e => e.System).HasColumnType("varchar(25)");

                entity.Property(e => e.Title).HasColumnType("varchar(150)");

                entity.Property(e => e.Type).HasColumnType("varchar(25)");

                entity.HasOne(d => d.IdchecklistNavigation)
                    .WithMany(p => p.LogChecklist)
                    .HasForeignKey(d => d.Idchecklist)
                    .HasConstraintName("FK_LogChecklist_LogChecklistIndex");
            });

            modelBuilder.Entity<LogChecklistHistory>(entity =>
            {
                entity.HasKey(e => new { e.Idchecklist, e.Version, e.FileTime, e.FileBy, e.FileAction });

                entity.Property(e => e.Idchecklist)
                    .HasColumnName("IDchecklist")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Version).HasColumnType("smallint(6)");

                entity.Property(e => e.FileTime).HasColumnType("datetime");

                entity.Property(e => e.FileBy).HasColumnType("varchar(50)");

                entity.Property(e => e.FileAction).HasColumnType("varchar(50)");

                entity.Property(e => e.Status).HasColumnType("varchar(25)");

                entity.HasOne(d => d.IdchecklistNavigation)
                    .WithMany(p => p.LogChecklistHistory)
                    .HasForeignKey(d => d.Idchecklist)
                    .HasConstraintName("FK_LogChecklistHistory_LogChecklistIndex");

                entity.HasOne(d => d.LogChecklist)
                    .WithMany(p => p.LogChecklistHistory)
                    .HasForeignKey(d => new { d.Idchecklist, d.Version })
                    .HasConstraintName("FK_LogChecklistHistory_LogChecklist");
            });

            modelBuilder.Entity<LogChecklistIndex>(entity =>
            {
                entity.HasKey(e => e.Idchecklist);

                entity.Property(e => e.Idchecklist)
                    .HasColumnName("IDchecklist")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedBy).HasColumnType("varchar(50)");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<LogChecklistSteps>(entity =>
            {
                entity.HasKey(e => new { e.Idchecklist, e.Version, e.Step, e.Idstep });

                entity.HasIndex(e => e.Idstep)
                    .HasName("IDstep");

                entity.Property(e => e.Idchecklist)
                    .HasColumnName("IDchecklist")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Version).HasColumnType("smallint(6)");

                entity.Property(e => e.Step).HasColumnType("smallint(6)");

                entity.Property(e => e.Idstep)
                    .HasColumnName("IDstep")
                    .HasColumnType("int(100)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.StepText).HasColumnType("text");

                entity.HasOne(d => d.IdchecklistNavigation)
                    .WithMany(p => p.LogChecklistSteps)
                    .HasForeignKey(d => d.Idchecklist)
                    .HasConstraintName("FK_LogChecklistSteps_LogChecklistIndex");

                entity.HasOne(d => d.LogChecklist)
                    .WithMany(p => p.LogChecklistSteps)
                    .HasForeignKey(d => new { d.Idchecklist, d.Version })
                    .HasConstraintName("FK_LogChecklistSteps_LogChecklist");
            });

            modelBuilder.Entity<LogUpdate>(entity =>
            {
                entity.HasKey(e => e.Idupdate);

                entity.Property(e => e.Idupdate)
                    .HasColumnName("IDupdate")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.Idchecklist)
                    .HasColumnName("IDchecklist")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Note).HasColumnType("text");

                entity.Property(e => e.Process).HasColumnType("varchar(50)");

                entity.Property(e => e.ProdLine).HasColumnType("varchar(10)");

                entity.Property(e => e.SiteKml).HasColumnType("varchar(25)");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.Status).HasColumnType("varchar(25)");

                entity.Property(e => e.System).HasColumnType("varchar(25)");

                entity.Property(e => e.Task).HasColumnType("int(11)");

                entity.Property(e => e.UpdateNum).HasColumnType("smallint(6)");

                entity.Property(e => e.UpdatePpack)
                    .HasColumnName("UpdatePPack")
                    .HasColumnType("varchar(5)");

                entity.Property(e => e.UpdateRelease).HasColumnType("varchar(10)");

                entity.Property(e => e.Version).HasColumnType("smallint(6)");
            });

            modelBuilder.Entity<LogUpdateHistory>(entity =>
            {
                entity.HasKey(e => new { e.Idupdate, e.FileTime, e.FileBy, e.FileAction });

                entity.Property(e => e.Idupdate)
                    .HasColumnName("IDupdate")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FileTime).HasColumnType("datetime");

                entity.Property(e => e.FileBy).HasColumnType("varchar(50)");

                entity.Property(e => e.FileAction).HasColumnType("varchar(50)");

                entity.Property(e => e.Status).HasColumnType("varchar(25)");

                entity.HasOne(d => d.IdupdateNavigation)
                    .WithMany(p => p.LogUpdateHistory)
                    .HasForeignKey(d => d.Idupdate)
                    .HasConstraintName("FK_LogUpdateHistory_LogUpdate");
            });

            modelBuilder.Entity<LogUpdateSteps>(entity =>
            {
                entity.HasKey(e => new { e.Idupdate, e.Step });

                entity.Property(e => e.Idupdate)
                    .HasColumnName("IDupdate")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Step).HasColumnType("smallint(6)");

                entity.Property(e => e.Comment).HasColumnType("varchar(225)");

                entity.Property(e => e.Progress).HasColumnType("varchar(10)");

                entity.Property(e => e.StepText).HasColumnType("text");

                entity.HasOne(d => d.IdupdateNavigation)
                    .WithMany(p => p.LogUpdateSteps)
                    .HasForeignKey(d => d.Idupdate)
                    .HasConstraintName("FK_LogUpdateSteps_LogUpdate");
            });
        }
    }
}
