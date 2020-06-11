using System;
using EFcore.MultiTenant.Performance.Test.Entity;
using kiwiho.EFcore.MultiTenant.DAL.Impl;
using kiwiho.EFcore.MultiTenant.Model;
using Microsoft.EntityFrameworkCore;

namespace EFcore.MultiTenant.Performance.Test.DbContext
{
    public class DemoContext: TenantBaseDbContext
    {
        public DbSet<Tabel1> Tabel1 => this.Set<Tabel1>();
        public DbSet<Tabel2> Tabel2 => this.Set<Tabel2>();
        public DbSet<Tabel3> Tabel3 => this.Set<Tabel3>();
        public DbSet<Tabel4> Tabel4 => this.Set<Tabel4>();
        public DbSet<Tabel5> Tabel5 => this.Set<Tabel5>();
        public DbSet<Tabel6> Tabel6 => this.Set<Tabel6>();
        public DbSet<Tabel7> Tabel7 => this.Set<Tabel7>();
        public DbSet<Tabel8> Tabel8 => this.Set<Tabel8>();
        public DbSet<Tabel9> Tabel9 => this.Set<Tabel9>();
        public DbSet<Tabel10> Tabel10 => this.Set<Tabel10>();
        public DbSet<Tabel11> Tabel11 => this.Set<Tabel11>();
        public DbSet<Tabel12> Tabel12 => this.Set<Tabel12>();
        public DbSet<Tabel13> Tabel13 => this.Set<Tabel13>();
        public DbSet<Tabel14> Tabel14 => this.Set<Tabel14>();
        public DbSet<Tabel15> Tabel15 => this.Set<Tabel15>();
        public DbSet<Tabel16> Tabel16 => this.Set<Tabel16>();
        public DbSet<Tabel17> Tabel17 => this.Set<Tabel17>();
        public DbSet<Tabel18> Tabel18 => this.Set<Tabel18>();
        public DbSet<Tabel19> Tabel19 => this.Set<Tabel19>();
        public DbSet<Tabel20> Tabel20 => this.Set<Tabel20>();

        private readonly TenantInfo tenantInfo;


        private static object locker = new object();
        private static int countdown= 0 ;

        public int CountDown=>countdown;

        public DemoContext(DbContextOptions<DemoContext> options, TenantInfo tenant, IServiceProvider serviceProvider) 
            : base(options, tenant, serviceProvider)
        {
            this.tenantInfo = tenant;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            lock (locker)
            {
                countdown++;
            }
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Tabel1>().ToTable("Tabel1");
            modelBuilder.Entity<Tabel2>().ToTable("Tabel2");
            modelBuilder.Entity<Tabel3>().ToTable("Tabel3");
            modelBuilder.Entity<Tabel4>().ToTable("Tabel4");
            modelBuilder.Entity<Tabel5>().ToTable("Tabel5");
            modelBuilder.Entity<Tabel6>().ToTable("Tabel6");
            modelBuilder.Entity<Tabel7>().ToTable("Tabel7");
            modelBuilder.Entity<Tabel8>().ToTable("Tabel8");
            modelBuilder.Entity<Tabel9>().ToTable("Tabel9");
            modelBuilder.Entity<Tabel10>().ToTable("Tabel10");
            modelBuilder.Entity<Tabel11>().ToTable("Tabel11");
            modelBuilder.Entity<Tabel12>().ToTable("Tabel12");
            modelBuilder.Entity<Tabel13>().ToTable("Tabel13");
            modelBuilder.Entity<Tabel14>().ToTable("Tabel14");
            modelBuilder.Entity<Tabel15>().ToTable("Tabel15");
            modelBuilder.Entity<Tabel16>().ToTable("Tabel16");
            modelBuilder.Entity<Tabel17>().ToTable("Tabel17");
            modelBuilder.Entity<Tabel18>().ToTable("Tabel18");
            modelBuilder.Entity<Tabel19>().ToTable("Tabel19");
            modelBuilder.Entity<Tabel20>().ToTable("Tabel20");
        }
    }
}
