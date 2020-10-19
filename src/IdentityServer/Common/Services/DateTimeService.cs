using System;
using LaserPointer.IdentityServer.Common.Interfaces;

namespace LaserPointer.IdentityServer.Common.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
