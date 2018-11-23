using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SD.Data.Models.DomainModels;

namespace SD.Data.Context.Configurations
{
    internal class SensorDataConfiguration : IEntityTypeConfiguration<SensorData>
    {
        public void Configure(EntityTypeBuilder<SensorData> builder)
        {
            builder.ToTable("SensorsData");

            builder.HasOne(sd => sd.Sensor)
                .WithMany(s => s.SensorData)
                .HasForeignKey(sd => sd.SensorId)
                .HasPrincipalKey(s => s.SensorId);
        }
    }
}
