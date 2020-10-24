using System.Collections.Generic;
using LaserPointer.WebApi.Domain.Entities;

namespace LaserPointer.WebApi.Application.Features.Jobs.Queries.GetUnfinishedJobsQuery
{
    public class UnfinishedJobsVm
    {
        public UnfinishedJobsVm(IList<JobDto> jobs) => Jobs = jobs;
        public IList<JobDto> Jobs { get; set; }
    }
}
