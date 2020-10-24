using AutoMapper;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Application.Common.Models.DtoAbstractions;
using LaserPointer.WebApi.Domain.Common;

namespace LaserPointer.WebApi.Application.Common.Mappings.CommonActions
{
    public class SetByCurrentUserAction : IMappingAction<AuditableEntity, ICreatedByCurrentUser>
    {
        private readonly ICurrentUserService _currentUserService;

        public SetByCurrentUserAction(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public void Process(AuditableEntity source, ICreatedByCurrentUser destination, ResolutionContext context)
        {
            destination.IsCreatedByCurrentUser = source.CreatedBy == _currentUserService.UserId;
        }
    }
}
