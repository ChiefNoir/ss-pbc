using Microsoft.AspNetCore.Mvc;

namespace API.Queries
{
    public class ProjectSearch
    {
        [FromQuery(Name = "categorycode")]
        public string CategoryCode { get; set; }
    }
}