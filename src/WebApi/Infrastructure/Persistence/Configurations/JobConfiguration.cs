using LaserPointer.WebApi.Domain.Entities;
using LaserPointer.WebApi.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LaserPointer.WebApi.Infrastructure.Persistence.Configurations
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.Ignore(e => e.DomainEvents);
            
            builder.HasKey(e => e.Id);

            builder.Property(e => e.HashType).HasConversion(new EnumToStringConverter<HashType>())
                .IsRequired();
            
            builder.Property(e => e.Status).HasConversion(new EnumToNumberConverter<JobStatus, int>())
                .IsRequired();

            builder.HasMany(e => e.HashesToCrack)
                .WithOne(e => e.Job);
        }
    }
}
