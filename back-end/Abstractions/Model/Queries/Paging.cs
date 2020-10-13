using Microsoft.AspNetCore.Mvc;

namespace Abstractions.Model.Queries
{
    public class Paging
    {
        [FromQuery(Name = "start")]
        public int Start { get; set; }

        [FromQuery(Name = "length")]
        public int Length { get; set; }
    }
}
