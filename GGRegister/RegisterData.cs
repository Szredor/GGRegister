using System;
using System.Collections.Generic;
using System.Text;

namespace GGRegister
{
    class RegisterData
    {
        //Pierwsze okienko
        public string Email { get; set; }
        public string Password { get; set; }

        //Drugie okienko
        public string FirstLastName { get; set; }
        public string BirthDate { get; set; }
        public string Town { get; set; }

        //Trzecie okienko
        public string PhoneNumber { get; set; }
    }
}
