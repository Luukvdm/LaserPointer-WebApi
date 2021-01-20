using System.Linq;
using FluentValidation;
using LaserPointer.WebApi.Application.Features.HashAlgorithms.Queries.GetHashAlgorithmQuery;
using MediatR;

namespace LaserPointer.WebApi.Application.Features.Jobs.Commands.CreateJobCommand
{
    public class CreateJobCommandValidator : AbstractValidator<CreateJobCommand>
    {
        public CreateJobCommandValidator(IMediator mediator)
        {
            RuleFor(j => j.HexHashes).NotEmpty();
            
            RuleFor(j => j.HexHashes).MustAsync(async (command, hashes, cancellation) =>
            {
                var algo = await mediator.Send(new GetHashAlgorithmQuery(command.HashAlgoId));
                return algo != null && hashes.All(hash => algo.GetFormatRegex().IsMatch(hash));
            }).WithMessage("Hashes don't match format or hash algorithm id is unknown");

            RuleFor(j => j.HashAlgoId)
                .NotNull().NotEqual(0);
        }
    }
}
