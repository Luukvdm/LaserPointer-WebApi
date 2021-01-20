using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Domain.Entities;
using LaserPointer.WebApi.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LaserPointer.WebApi.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedSampleDataAsync(ApplicationDbContext context, IDateTime dateTime)
        {
            // Seed, if necessary
            // if (!context.Jobs.Any())
            // {
                /*context.Jobs.Add(new Job
                {
                    Status = JobStatus.InQueue,
                    HashType = HashType.Sha256,
                    HashesToCrack =
                    {
                        new Hash
                        {
                            Value = new byte[32] { 0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1 },
                            PlainValue = ""
                        }
                    }
                });

                await context.SaveChangesAsync();*/
            // }

            // Clean up hash algorithms in database
            context.RemoveRange(await context.HashAlgos.ToListAsync());
            await context.SaveChangesAsync();
            
            // Seed all the possible algorithms
            await context.HashAlgos.AddAsync(new HashAlgo {Type = HashAlgoType.Sha256, Format = "\b[a-fA-F\\d]{64}\b"});
            await context.HashAlgos.AddAsync(new HashAlgo {Type = HashAlgoType.Md5, Format = "\b[a-fA-F\\d]{32}\b"});
            await context.SaveChangesAsync();
        } 
    }
}
