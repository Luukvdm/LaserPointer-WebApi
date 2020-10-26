using FluentValidation;

namespace LaserPointer.WebApi.Application.Features.Jobs.Commands.CreateJobCommand
{
    public class CreateJobCommandValidator : AbstractValidator<CreateJobCommand>
    {
        public CreateJobCommandValidator()
        {
            RuleFor(j => j.HashType)
                .NotNull();
            
            // TODO create a dynamic length per hash type (not all hashes are 64 chars in hex)
            // Better solution might be to create a HashType entity in the domain layer ...
            RuleForEach(j => j.HexHashes)
                .Length(64)
                .NotEmpty();
        }
    }
}
