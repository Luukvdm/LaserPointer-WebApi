using System.Collections;
using System.Collections.Generic;

namespace LaserPointer.WebApi.Application.Features.Jobs.Queries.GetMyJobsQuery
{
    public class MyJobsVm
    {
        public MyJobsVm(IList<MyJobDto> jobs) => Jobs = jobs;
        public IList<MyJobDto> Jobs { get; set; }
    }
}
