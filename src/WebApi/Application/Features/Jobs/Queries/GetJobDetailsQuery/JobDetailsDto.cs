using System;
using System.Collections.Generic;
using AutoMapper;
using LaserPointer.WebApi.Application.Common.Mappings;
using LaserPointer.WebApi.Domain.Entities;
using LaserPointer.WebApi.Domain.Enums;

namespace LaserPointer.WebApi.Application.Features.Jobs.Queries.GetJobDetailsQuery
{
    public class JobDetailsDto : IMapFrom<Job>
    {
        public int Id { get; set; }
        public HashAlgoType HashType { get; set; }
        public JobStatus Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }
        
        public IList<HashDetailsDto> Hashes { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Job, JobDetailsDto>()
                .ForMember(dest => dest.Hashes,
                    src => src.MapFrom(e => e.HashesToCrack)
                    );
        }
    }
}
