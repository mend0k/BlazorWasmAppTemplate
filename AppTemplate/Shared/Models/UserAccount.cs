using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace AppTemplate.Shared.Models
{
    // TODO: Add validations
    public class UserAccount
    {
        public int ID_UserAccount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DOB { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string AltPhone { get; set; }

    }
}
