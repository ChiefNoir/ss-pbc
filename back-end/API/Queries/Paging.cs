using Microsoft.AspNetCore.Mvc;

namespace API.Queries
{
    public class Paging
    {
        [FromQuery(Name = "start")]
        public int Start { get; set; }

        [FromQuery(Name = "length")]
        public int Length { get; set; }
    }
}