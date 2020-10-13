using Microsoft.AspNetCore.Mvc;

namespace Abstractions.Model.Queries
{
    public class ProjectSearch
    {
        [FromQuery(Name = "categorycode")]
        public string CategoryCode { get; set; }
    }
}
