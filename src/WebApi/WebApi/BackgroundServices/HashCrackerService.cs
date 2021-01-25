using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Application.Common.Models;
using LaserPointer.WebApi.Application.Features.Hashes.Commands.UpdateHashCommand;
using LaserPointer.WebApi.Application.Features.Jobs.Commands.UpdateJobCommand;
using LaserPointer.WebApi.Application.Features.Jobs.Queries.GetNextJobQuery;
using LaserPointer.WebApi.Domain.Entities;
using LaserPointer.WebApi.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LaserPointer.WebApi.WebApi.BackgroundServices
{
    public class HashCrackerService : IHostedService, IDisposable
    {
        private readonly ILogger<HashCrackerService> _logger;
        private readonly GlobalSettings _globalSettings;

        private static readonly string[] s_wordList = { "welkom", "Welkom", "welcome", "Welcome", "@Welkom1", "@Welcom1" };
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
        private Task _executingTask;
        
        public IServiceProvider Services { get; }

        public HashCrackerService(ILogger<HashCrackerService> logger, IServiceProvider services, GlobalSettings globalSettings)
        {
            _logger = logger;
            _globalSettings = globalSettings;
            Services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{ProjectName} HashCrackerService: Starting hash cracking background service ...", _globalSettings.ProjectName);
            _executingTask = CrackHashes(cancellationToken);
            return _executingTask.IsCompleted ? _executingTask : Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executingTask == null) return;

            try {
                _stoppingCts.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

        public void Dispose()
        {
            _stoppingCts.Cancel();
        }

        private async Task CrackHashes(CancellationToken cancellationToken)
        {
            using var scope = Services.CreateScope();
            var httpContext = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
            httpContext.HttpContext?.User?.AddIdentity(new ClaimsIdentity());
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            
            while (!_stoppingCts.Token.IsCancellationRequested)
            {
                var job = await mediator.Send(new GetNextJobQuery(), cancellationToken);
                
                if (job == null)
                {
                    _logger.LogTrace("{ProjectName} HashCrackerService Out of hashes to crack, going to sleep for a while ...", _globalSettings.ProjectName);
                    await Task.Delay(20 * 1000, cancellationToken);
                    continue;
                }
                
                _logger.LogTrace("{ProjectName} HashCrackerService: Starting job {id}", _globalSettings.ProjectName, job.Id);
                
                // Lets ignore unfinished jobs (like this application) for now
                await mediator.Send(new UpdateJobCommand {Id = job.Id, JobStatus = JobStatus.InProgress}, cancellationToken);
                
                if (job.HashAlgo.Type == HashAlgoType.Sha256)
                {
                    var sha = new SHA256Managed();
                    foreach (var toCrack in job.HashesToCrack)
                    {
                        foreach (string guess in s_wordList)
                        {
                            var plainText = Encoding.UTF8.GetBytes(guess);
                            var guessHash = sha.ComputeHash(plainText);
                            if (!CompareByteArrays(guessHash, toCrack.Value))
                            {
                                continue;
                            }

                            // Yay !
                            // toCrack.PlainValue = guess;
                            await mediator.Send(new UpdateHashCommand {Id = toCrack.Id, PlainValue = guess },
                                cancellationToken);
                            break;
                        }
                    }
                    // Wait 5 seconds for effect
                    await Task.Delay(5 * 1000, cancellationToken);
                }
                else
                {
                    // uhhhh ....
                    await Task.Delay(30 * 1000, cancellationToken);
                }
                
                _logger.LogTrace("{ProjectName} HashCrackerService: Finished job, going to update db", _globalSettings.ProjectName);
                
                // Kinda lazy but whatever
                /* foreach (var hash in job.HashesToCrack)
                {
                    if (!string.IsNullOrEmpty(hash.PlainValue))
                    {
                        await mediator.Send(new UpdateHashCommand {Id = hash.Id, PlainValue = hash.PlainValue},
                            cancellationToken);
                    }
                } */
                
                // Update the db with cracking results
                await mediator.Send(new UpdateJobCommand {Id = job.Id, JobStatus = JobStatus.Done}, cancellationToken);
            }
        }
        
        private static bool CompareByteArrays(IReadOnlyList<byte> array1, IReadOnlyList<byte> array2)
        {
            if (array1.Count != array2.Count)
            {
                return false;
            }

            return !array1.Where((t, i) => t != array2[i]).Any();
        }

    }
}
