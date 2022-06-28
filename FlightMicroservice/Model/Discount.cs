using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMicroservice.Model
{
    public class Discount
    {
        public int DiscountId { get; set; }
        public string DiscountCode { get; set; }

        public int DiscountAmount { get; set; }

      

    }
}
