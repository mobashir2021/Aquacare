using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnDemandService.Models
{
    public class UserData
    {
        public int PartnerId { get; set; }
        public int Userid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Service { get; set; }
        public string Balance { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }

        public string Mobileno { get; set; }
    }
}