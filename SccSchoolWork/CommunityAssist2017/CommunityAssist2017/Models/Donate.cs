using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityAssist2017.Models
{

    using System;
    using System.Collections.Generic;
    public class Donate
    {
        public int PersonKey { get; set; }
        public decimal DonationAmount { get; set; }
        public Nullable<System.DateTime> DonationDate { get; set; }


    }
}