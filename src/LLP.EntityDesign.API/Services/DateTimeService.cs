using LLP.EntityDesign.API.Contracts;
using System;

namespace LLP.EntityDesign.API.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
