using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace versión_5_asp.Models
{
    public class UserInfo
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName{ get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public Municipio Municipality { get; set; }
    }
}
