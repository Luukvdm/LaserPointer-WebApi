using System.Collections.Generic;
using System.Text.RegularExpressions;
using LaserPointer.WebApi.Domain.Enums;

namespace LaserPointer.WebApi.Domain.Entities
{
    public class HashAlgo
    {
        public HashAlgo()
        {
            Jobs = new List<Job>();
        }
        
        public int Id { get; set; }
        public HashAlgoType Type { get; set; }
        public string Format { get; set; }
        public IList<Job> Jobs { get; set; }

        public Regex GetFormatRegex()
        {
            return new Regex(Format);
        }
    }
}
