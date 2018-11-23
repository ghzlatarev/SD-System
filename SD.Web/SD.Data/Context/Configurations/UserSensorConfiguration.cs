using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SD.Data.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SD.Data.Context.Configurations
{
	internal class UserSensorConfiguration : IEntityTypeConfiguration<UserSensor>
	{
		public void Configure(EntityTypeBuilder<UserSensor> builder)
		{
			builder.ToTable("UserSensors");

			builder.HasOne(us => us.User)
				.WithMany(u => u.PersonalSensors)
				.HasForeignKey(us => us.UserId)
				.HasPrincipalKey(u => u.Id);

			builder.HasOne(us => us.Sensor)
				.WithMany(s => s.UserSensors)
				.HasForeignKey(us => us.SensorId)
				.HasPrincipalKey(s => s.SensorId);
		}
	}
}
