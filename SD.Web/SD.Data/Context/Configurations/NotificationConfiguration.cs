using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SD.Data.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SD.Data.Context.Configurations
{
	internal class NotificationConfiguration : IEntityTypeConfiguration<Notifications>
	{
		public void Configure(EntityTypeBuilder<Notifications> builder)
		{
			builder.ToTable("Sensor Notifications");

			builder.HasOne(n => n.User)
				.WithMany(u => u.Notifications)
				.HasForeignKey(n => n.UserId)
				.HasPrincipalKey(u => u.Id);
		}
	}
}
