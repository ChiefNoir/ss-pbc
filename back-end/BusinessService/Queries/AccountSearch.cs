using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Queries
{
    public class AccountSearch
    {
        [FromQuery(Name = "keyword")]
        public string Keyword { get; set; }

    }
}
