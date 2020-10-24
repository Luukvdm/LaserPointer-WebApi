using System;
using AutoMapper;
using LaserPointer.WebApi.Application.Common.Mappings;
using LaserPointer.WebApi.Application.Common.Mappings.CommonActions;
using LaserPointer.WebApi.Application.Common.Models.DtoAbstractions;
using LaserPointer.WebApi.Domain.Entities;
using LaserPointer.WebApi.Domain.Enums;

namespace LaserPointer.WebApi.Application.Features.Jobs.Queries.GetUnfinishedJobsQuery
{
    public class JobDto : IMapFrom<Job>, ICreatedByCurrentUser
    {
        public int Id { get; set; }
        public JobStatus Status { get; set; }
        public DateTime Created { get; set; }
        public bool IsCreatedByCurrentUser { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Job, JobDto>()
                .AfterMap<SetByCurrentUserAction>();
        }

    }
}
