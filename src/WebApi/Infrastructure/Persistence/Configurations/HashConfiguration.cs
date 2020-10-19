using LaserPointer.WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaserPointer.WebApi.Infrastructure.Persistence.Configurations
{
    public class HashConfiguration : IEntityTypeConfiguration<Hash>
    {
        public void Configure(EntityTypeBuilder<Hash> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Value)
                .IsRequired();
            
            builder.Property(e => e.PlainValue)
                .HasMaxLength(255)
                .IsRequired(false);

            builder.HasOne(e => e.Job)
                .WithMany(e => e.HashesToCrack);
        }
    }
}
