using Microsoft.AspNetCore.Mvc;

namespace SSPBC.Models
{
    public class ProjectSearch
    {
        [FromQuery(Name = "categorycode")]
        public string? CategoryCode { get; set; } = null;
    }
}
