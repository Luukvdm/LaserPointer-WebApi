using System;
using System.Linq;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Domain.Entities;
using LaserPointer.WebApi.Domain.Enums;

namespace LaserPointer.WebApi.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedSampleDataAsync(ApplicationDbContext context, IDateTime dateTime)
        {
            // Seed, if necessary
            if (!context.Jobs.Any())
            {
                context.Jobs.Add(new Job
                {
                    Id = 1,
                    Status = JobStatus.InQueue,
                    HashType = HashType.Sha256,
                    HashesToCrack =
                    {
                        new Hash
                        {
                            Id = 1,
                            Value = new byte[32] { 0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1 },
                            PlainValue = ""
                        }
                    }
                });

                await context.SaveChangesAsync();
            }
        } 
    }
}
