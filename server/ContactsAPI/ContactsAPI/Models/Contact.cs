using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactsAPI.Models
{
    public class Contact
    {
        public string IdNumber { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public string PhoneNum { get; set; }


    }
}