using Microsoft.AspNetCore.Mvc;

namespace SSPBC.Models
{
    public class Paging
    {
        [FromQuery(Name = "start")]
        public int Start { get; set; }

        [FromQuery(Name = "length")]
        public int Length { get; set; }

        public Paging() 
        { 
        }

        public Paging(int start, int length)
        {
            Start = start;
            Length = length;
        }
    }
}
