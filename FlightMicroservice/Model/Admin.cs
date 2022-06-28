using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMicroservice.Model
{
    public class Admin
    {
        public int adminId { get; set; }

        public string adminName { get; set; }
        public string adminEmailId { get; set; }

        public string adminPasskey { get; set; }
    }
}
