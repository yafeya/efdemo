using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfDemo
{
    public class PortalContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        static PortalContext()
        {
            Database.SetInitializer<PortalContext>(new CreateDatabaseIfNotExists<PortalContext>());
            Database.SetInitializer<PortalContext>(new DropCreateDatabaseIfModelChanges<PortalContext>());
        }

        public PortalContext() : base("Portal")
        { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().Property(t => t.ID).HasColumnName("id")
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_ProjectSectionOd", 1) { IsUnique = true }));
            modelBuilder.Entity<Product>().Property(t => t.Name).HasColumnName("name").IsRequired().HasMaxLength(256);

            modelBuilder.Entity<User>().Property(t => t.ID).HasColumnName("id")
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_ProjectSectionOd", 1) { IsUnique = true }));
            modelBuilder.Entity<User>().Property(t => t.Name).HasColumnName("name").IsRequired().HasMaxLength(256);
            modelBuilder.Entity<User>().Property(t => t.ProductID).HasColumnName("product_id").IsRequired();

            modelBuilder.Entity<Product>().HasMany<User>(p => p.Users)
                                          .WithRequired(u => u.Product)
                                          .HasForeignKey(u => u.ProductID);
        }
    }
}
