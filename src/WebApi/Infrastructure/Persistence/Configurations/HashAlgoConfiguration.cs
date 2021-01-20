using LaserPointer.WebApi.Domain.Entities;
using LaserPointer.WebApi.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LaserPointer.WebApi.Infrastructure.Persistence.Configurations
{
    public class HashAlgoConfiguration : IEntityTypeConfiguration<HashAlgo>
    {
        public void Configure(EntityTypeBuilder<HashAlgo> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasAlternateKey(e => e.Type);

            builder.Property(e => e.Type)
                .HasConversion(new EnumToStringConverter<HashAlgoType>())
                .IsRequired();

            builder.Property(e => e.Format)
                .IsRequired();

            builder.HasMany<Job>().WithOne(e => e.HashAlgo);
        }
    }
}
