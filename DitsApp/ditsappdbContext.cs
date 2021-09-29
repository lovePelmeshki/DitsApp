using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DitsApp
{
    public partial class ditsappdbContext : DbContext
    {
        public ditsappdbContext()
        {
        }

        public ditsappdbContext(DbContextOptions<ditsappdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Equipment> Equipment { get; set; }
        public virtual DbSet<EquipmentClass> EquipmentClasses { get; set; }
        public virtual DbSet<EquipmentType> EquipmentTypes { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<EventType> EventTypes { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Maintenance> Maintenances { get; set; }
        public virtual DbSet<Station> Stations { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:ditsapp.database.windows.net,1433;Initial Catalog=ditsappdb;Persist Security Info=False;User ID=lovepelmeshki;Password=90f8b7rr#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(e => e.DepartmentId).ValueGeneratedNever();

                entity.Property(e => e.DepartmentName).HasMaxLength(50);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.EmployeeId).ValueGeneratedNever();

                entity.Property(e => e.Firstname).HasMaxLength(255);

                entity.Property(e => e.Lastname).HasMaxLength(255);

                entity.Property(e => e.Middlename).HasMaxLength(255);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_Employees_Departments");
            });

            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.Property(e => e.EquipmentId).HasMaxLength(50);

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Equipment)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_Equipment_Locations");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Equipment)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_Equipment_EquipmentTypes");
            });

            modelBuilder.Entity<EquipmentClass>(entity =>
            {
                entity.Property(e => e.EquipmentClassId).ValueGeneratedNever();

                entity.Property(e => e.EquipmentClassName).HasMaxLength(50);
            });

            modelBuilder.Entity<EquipmentType>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.Property(e => e.TypeId).ValueGeneratedNever();

                entity.Property(e => e.TypeName).HasMaxLength(50);

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.EquipmentTypes)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK_EquipmentTypes_EquipmentClasses");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.EventId).ValueGeneratedNever();

                entity.Property(e => e.CloseDate).HasColumnType("date");

                entity.Property(e => e.Comment).HasColumnType("text");

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.EquipmentId).HasMaxLength(50);

                entity.Property(e => e.RespondDate).HasColumnType("date");

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.EventCreators)
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("FK_Events_Employees1");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.EquipmentId)
                    .HasConstraintName("FK_Events_Equipment");

                entity.HasOne(d => d.EventType)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.EventTypeId)
                    .HasConstraintName("FK_Events_EventTypes");

                entity.HasOne(d => d.Respoinder)
                    .WithMany(p => p.EventRespoinders)
                    .HasForeignKey(d => d.RespoinderId)
                    .HasConstraintName("FK_Events_Employees");
            });

            modelBuilder.Entity<EventType>(entity =>
            {
                entity.Property(e => e.EventTypeId).ValueGeneratedNever();

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.EventName).HasMaxLength(50);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.LocationId).ValueGeneratedNever();

                entity.Property(e => e.Desctiption)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Point)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Post).HasDefaultValueSql("((-1))");

                entity.HasOne(d => d.Station)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.StationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Locations_Stations");
            });

            modelBuilder.Entity<Maintenance>(entity =>
            {
                entity.Property(e => e.Comment).HasColumnType("text");

                entity.Property(e => e.DueDate).HasColumnType("date");

                entity.Property(e => e.EquipmentId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MaintenanceDate).HasColumnType("date");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.Maintenances)
                    .HasForeignKey(d => d.EquipmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Maintenances_Equipment");

                entity.HasOne(d => d.Maintainer)
                    .WithMany(p => p.Maintenances)
                    .HasForeignKey(d => d.MaintainerId)
                    .HasConstraintName("FK_Maintenances_Employees");
            });

            modelBuilder.Entity<Station>(entity =>
            {
                entity.Property(e => e.StationId).ValueGeneratedNever();

                entity.Property(e => e.StationName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Hash).HasColumnType("text");

                entity.Property(e => e.UserName).HasMaxLength(255);
            });

            modelBuilder.HasSequence<int>("SalesOrderNumber", "SalesLT");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
