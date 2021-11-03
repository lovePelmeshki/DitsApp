using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DitsApp.Model
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
        public virtual DbSet<EquipmentStatus> EquipmentStatuses { get; set; }
        public virtual DbSet<EquipmentType> EquipmentTypes { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<EventType> EventTypes { get; set; }
        public virtual DbSet<Line> Lines { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Maintenance> Maintenances { get; set; }
        public virtual DbSet<Point> Points { get; set; }
        public virtual DbSet<Station> Stations { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=PELMESHKI_PC\\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("departments");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DepartmentName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("department_name");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("employees");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DepartmentId).HasColumnName("department_id");

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("firstname");

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("lastname");

                entity.Property(e => e.Middlename)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("middlename");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_employees_departments");
            });

            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.ToTable("equipment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.SerialNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("serial_number");

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Equipment)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_equipment_equipment_types");
            });

            modelBuilder.Entity<EquipmentClass>(entity =>
            {
                entity.ToTable("equipment_classes");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClassName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("class_name");
            });

            modelBuilder.Entity<EquipmentStatus>(entity =>
            {
                entity.ToTable("equipment_status");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ChangeDate)
                    .HasColumnType("datetime")
                    .HasColumnName("change_date");

                entity.Property(e => e.CheckupDate)
                    .HasColumnType("datetime")
                    .HasColumnName("checkup_date");

                entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");

                entity.Property(e => e.InstallDate)
                    .HasColumnType("datetime")
                    .HasColumnName("install_date");

                entity.Property(e => e.MaintainerId).HasColumnName("maintainer_id");

                entity.Property(e => e.MaintenanceDate)
                    .HasColumnType("datetime")
                    .HasColumnName("maintenance_date");

                entity.Property(e => e.NextCheckupDate)
                    .HasColumnType("datetime")
                    .HasColumnName("next_checkup_date");

                entity.Property(e => e.NextMaintenanceDate)
                    .HasColumnType("datetime")
                    .HasColumnName("next_maintenance_date");

                entity.Property(e => e.PointId).HasColumnName("point_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.StatusType)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("status_type");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.EquipmentStatuses)
                    .HasForeignKey(d => d.EquipmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_equipment_status_equipment");

                entity.HasOne(d => d.Maintainer)
                    .WithMany(p => p.EquipmentStatuses)
                    .HasForeignKey(d => d.MaintainerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_equipment_status_employees");

                entity.HasOne(d => d.Point)
                    .WithMany(p => p.EquipmentStatuses)
                    .HasForeignKey(d => d.PointId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_equipment_status_points");
            });

            modelBuilder.Entity<EquipmentType>(entity =>
            {
                entity.ToTable("equipment_types");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClassId).HasColumnName("class_id");

                entity.Property(e => e.InstallDuration).HasColumnName("install_duration");

                entity.Property(e => e.MaintenanceDuration).HasColumnName("maintenance_duration");

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("type_name");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.EquipmentTypes)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_equipment_types_equipment_classes");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("events");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CloseDate)
                    .HasColumnType("datetime")
                    .HasColumnName("close_date");

                entity.Property(e => e.Comment)
                    .HasColumnType("ntext")
                    .HasColumnName("comment");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date");

                entity.Property(e => e.CreatorId).HasColumnName("creator_id");

                entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");

                entity.Property(e => e.LocationId).HasColumnName("location_id");

                entity.Property(e => e.RespoinderId).HasColumnName("respoinder_id");

                entity.Property(e => e.RespondDate)
                    .HasColumnType("datetime")
                    .HasColumnName("respond_date");

                entity.Property(e => e.StationId).HasColumnName("station_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.HasOne(d => d.Station)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.StationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_events_stations");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_events_event_types");
            });

            modelBuilder.Entity<EventType>(entity =>
            {
                entity.ToTable("event_types");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasColumnType("ntext")
                    .HasColumnName("description");

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("type_name");
            });

            modelBuilder.Entity<Line>(entity =>
            {
                entity.ToTable("lines");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.LineName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("line_name");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("locations");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LocationName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("location_name");

                entity.Property(e => e.StationId).HasColumnName("station_id");

                entity.HasOne(d => d.Station)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.StationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_locations_stations");
            });

            modelBuilder.Entity<Maintenance>(entity =>
            {
                entity.ToTable("maintenances");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Comment)
                    .HasColumnType("ntext")
                    .HasColumnName("comment");

                entity.Property(e => e.DurationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("duration_date");

                entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");

                entity.Property(e => e.MainteinerId).HasColumnName("mainteiner_id");

                entity.Property(e => e.MaintenanceDate)
                    .HasColumnType("datetime")
                    .HasColumnName("maintenance_date");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.Maintenances)
                    .HasForeignKey(d => d.EquipmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_maintenances_equipment");

                entity.HasOne(d => d.Mainteiner)
                    .WithMany(p => p.Maintenances)
                    .HasForeignKey(d => d.MainteinerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_maintenances_employees");
            });

            modelBuilder.Entity<Point>(entity =>
            {
                entity.ToTable("points");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LocationId).HasColumnName("location_id");

                entity.Property(e => e.PointName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("point_name");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Points)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_points_locations");
            });

            modelBuilder.Entity<Station>(entity =>
            {
                entity.ToTable("stations");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LineId).HasColumnName("line_id");

                entity.Property(e => e.StationName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("station_name");

                entity.HasOne(d => d.Line)
                    .WithMany(p => p.Stations)
                    .HasForeignKey(d => d.LineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_stations_lines");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Hash)
                    .HasColumnType("ntext")
                    .HasColumnName("hash");

                entity.Property(e => e.Permission).HasColumnName("permission");

                entity.Property(e => e.UserName)
                    .HasMaxLength(255)
                    .HasColumnName("user_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
