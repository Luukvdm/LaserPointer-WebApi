using System;
using LaserPointer.WebApi.Application.Common.Interfaces;

namespace LaserPointer.WebApi.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
