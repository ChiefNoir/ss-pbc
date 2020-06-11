using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Model
{
    class Identity
    {
        public string Login { get; set; }
        public string Token { get; set; }
        public int TokenLifeTimeMinutes { get; set; }
    }
}
