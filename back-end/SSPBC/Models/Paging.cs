using Microsoft.AspNetCore.Mvc;

namespace SSPBC.Models
{
    public class Paging
    {
        [FromQuery(Name = "start")]
        public int Start { get; set; }

        [FromQuery(Name = "length")]
        public int Length { get; set; }
    }
}
