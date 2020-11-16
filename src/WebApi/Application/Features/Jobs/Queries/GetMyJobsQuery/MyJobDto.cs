﻿using System;
using LaserPointer.WebApi.Application.Common.Mappings;
using LaserPointer.WebApi.Domain.Entities;
using LaserPointer.WebApi.Domain.Enums;

namespace LaserPointer.WebApi.Application.Features.Jobs.Queries.GetMyJobsQuery
{
    public class MyJobDto: IMapFrom<Job>
    {
        public int Id { get; set; }
        public HashType HashType { get; set; }
        public JobStatus Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }
    }
}