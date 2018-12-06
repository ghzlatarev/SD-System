using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SD.Data.Context.Configurations;
using SD.Data.Context.Configurations.Identity;
using SD.Data.Models;
using SD.Data.Models.Contracts;
using SD.Data.Models.Identity;
using SD.Data.Models.DomainModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SD.Data.Context
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public virtual DbSet<Sensor> Sensors { get; set; }

        public virtual DbSet<SensorData> SensorData { get; set; }

        public virtual DbSet<UserSensor> UserSensors { get; set; }

		public virtual DbSet<Notifications> Notifications { get; set; }

		public override int SaveChanges()
        {
            this.ApplyDeletionRules();
            this.ApplyAuditInfoRules();
            return base.SaveChanges();
        }

        public virtual async Task<int> SaveChangesAsync(bool applyDeletionRules = true, bool applyAuditInfoRules = true)
        {
            if (applyDeletionRules == true)
            {
                this.ApplyDeletionRules();
            }

            if (applyAuditInfoRules == true)
            {
                this.ApplyAuditInfoRules();
            }

            return await base.SaveChangesAsync();
        }

        public override DbSet<TEntity> Set<TEntity>()
        {
            return base.Set<TEntity>();
        }

        public override EntityEntry Entry(object entity)
        {
            return base.Entry(entity);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Identity model configuration
            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());

            //API model configurations
            modelBuilder.ApplyConfiguration(new SensorConfiguration());
            modelBuilder.ApplyConfiguration(new SensorDataConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        private void ApplyDeletionRules()
        {
            var entitiesForDeletion = this.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Deleted && e.Entity is IDeletable);

            foreach (var entry in entitiesForDeletion)
            {
                var entity = (IDeletable)entry.Entity;
                entity.DeletedOn = DateTime.UtcNow.AddHours(2);
                entity.IsDeleted = true;
                entry.State = EntityState.Modified;
            }
        }

        private void ApplyAuditInfoRules()
        {
            var newlyCreatedEntities = this.ChangeTracker.Entries()
                .Where(e => e.Entity is IAuditable && ((e.State == EntityState.Added) || (e.State == EntityState.Modified)));

            foreach (var entry in newlyCreatedEntities)
            {
                var entity = (IAuditable)entry.Entity;

                if (entry.State == EntityState.Added && entity.CreatedOn == null)
                {
                    entity.CreatedOn = DateTime.UtcNow.AddHours(2);
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow.AddHours(2);
                }
            }
        }
    }
}