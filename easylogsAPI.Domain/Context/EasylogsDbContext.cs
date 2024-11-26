using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using easylogsAPI.Domain.Entities;

namespace easylogsAPI.Domain.Context;

public partial class EasyLogsDbContext : DbContext
{
    public EasyLogsDbContext()
    {
    }

    public EasyLogsDbContext(DbContextOptions<EasyLogsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Logtype> Logtypes { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Sessiontype> Sessiontypes { get; set; }

    public virtual DbSet<Tokenaccess> Tokenaccesses { get; set; }

    public virtual DbSet<Tokenrefresh> Tokenrefreshes { get; set; }

    public virtual DbSet<Userapp> Userapps { get; set; }

    public virtual DbSet<Userapppermission> Userapppermissions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=easylogs;User Id=postgres;Port=5432;Password=vmt1234");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("log_pkey");

            entity.ToTable("log");

            entity.Property(e => e.LogId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("log_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DataJson)
                .HasDefaultValueSql("'{}'::text")
                .HasColumnName("data_json");
            entity.Property(e => e.Exception).HasColumnName("exception");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.StackTrace).HasColumnName("stack_trace");
            entity.Property(e => e.Trace)
                .HasDefaultValueSql("'empty'::text")
                .HasColumnName("trace");
            entity.Property(e => e.Type)
                .HasDefaultValue(2)
                .HasColumnName("type");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Logs)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("log_type_fkey");
        });

        modelBuilder.Entity<Logtype>(entity =>
        {
            entity.HasKey(e => e.LogTypeId).HasName("logtype_pkey");

            entity.ToTable("logtype");

            entity.Property(e => e.LogTypeId).HasColumnName("log_type_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.ShowName)
                .HasMaxLength(100)
                .HasColumnName("show_name");
            entity.Property(e => e.StyleClass)
                .HasMaxLength(255)
                .HasColumnName("style_class");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("permission_pkey");

            entity.ToTable("permission");

            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasDefaultValueSql("'without description'::character varying")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.ShowName)
                .HasMaxLength(50)
                .HasColumnName("show_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Sessiontype>(entity =>
        {
            entity.HasKey(e => e.SessionTypeId).HasName("sessiontype_pkey");

            entity.ToTable("sessiontype");

            entity.Property(e => e.SessionTypeId).HasColumnName("session_type_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Tokenaccess>(entity =>
        {
            entity.HasKey(e => e.TokenAccessId).HasName("tokenaccess_pkey");

            entity.ToTable("tokenaccess");

            entity.Property(e => e.TokenAccessId).HasColumnName("token_access_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Expiration).HasColumnName("expiration");
            entity.Property(e => e.Ip)
                .HasMaxLength(255)
                .HasColumnName("ip");
            entity.Property(e => e.IsApiKey)
                .HasDefaultValue(false)
                .HasColumnName("is_api_key");
            entity.Property(e => e.TokenRefreshId).HasColumnName("token_refresh_id");
            entity.Property(e => e.UserAppId).HasColumnName("user_app_id");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.TokenRefresh).WithMany(p => p.Tokenaccesses)
                .HasForeignKey(d => d.TokenRefreshId)
                .HasConstraintName("tokenaccess_token_refresh_id_fkey");

            entity.HasOne(d => d.UserApp).WithMany(p => p.Tokenaccesses)
                .HasForeignKey(d => d.UserAppId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tokenaccess_user_app_id_fkey");
        });

        modelBuilder.Entity<Tokenrefresh>(entity =>
        {
            entity.HasKey(e => e.TokenRefreshId).HasName("tokenrefresh_pkey");

            entity.ToTable("tokenrefresh");

            entity.Property(e => e.TokenRefreshId).HasColumnName("token_refresh_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Expiration).HasColumnName("expiration");
            entity.Property(e => e.Ip)
                .HasMaxLength(255)
                .HasColumnName("ip");
            entity.Property(e => e.IsApiKey)
                .HasDefaultValue(false)
                .HasColumnName("is_api_key");
            entity.Property(e => e.UserAppId).HasColumnName("user_app_id");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.UserApp).WithMany(p => p.Tokenrefreshes)
                .HasForeignKey(d => d.UserAppId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tokenrefresh_user_app_id_fkey");
        });

        modelBuilder.Entity<Userapp>(entity =>
        {
            entity.HasKey(e => e.UserAppId).HasName("userapp_pkey");

            entity.ToTable("userapp");

            entity.Property(e => e.UserAppId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("user_app_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.SessionTime)
                .HasDefaultValue(1)
                .HasColumnName("session_time");
            entity.Property(e => e.SessionTypeId)
                .HasDefaultValue(1)
                .HasColumnName("session_type_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.HasOne(d => d.SessionType).WithMany(p => p.Userapps)
                .HasForeignKey(d => d.SessionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("userapp_session_type_id_fkey");
        });

        modelBuilder.Entity<Userapppermission>(entity =>
        {
            entity.HasKey(e => e.UserAppPermissionId).HasName("userapppermission_pkey");

            entity.ToTable("userapppermission");

            entity.Property(e => e.UserAppPermissionId).HasColumnName("user_app_permission_id");
            entity.Property(e => e.GivenAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("given_at");
            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.UserAppId).HasColumnName("user_app_id");

            entity.HasOne(d => d.Permission).WithMany(p => p.Userapppermissions)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("userapppermission_permission_id_fkey");

            entity.HasOne(d => d.UserApp).WithMany(p => p.Userapppermissions)
                .HasForeignKey(d => d.UserAppId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("userapppermission_user_app_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
