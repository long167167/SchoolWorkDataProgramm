using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bakery.Models
{

    using System;
    using System.Collections.Generic;

    public class CustomerRegister
    {
        public int PersonKey { get; set; }
        public string PersonLastName { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonEmail { get; set; }
        public string PersonPhone { get; set; }
        public Nullable<System.DateTime> PersonDateAdded { get; set; }
    }
}