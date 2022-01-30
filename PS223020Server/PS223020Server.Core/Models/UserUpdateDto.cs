using System;
using System.Collections.Generic;
using System.Text;

namespace PS223020Server.Core.Models
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public bool IsBoy { get; set; }
        public string Password { get; set; }
        public string PhoneNumberPrefix { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public DateTimeOffset Birthday { get; set; }
        public string AvatarUrl { get; set; }



        public string CurrentPhoneNumber { get; set; }
        public string CurrentNumberPrefix { get; set; }
        public string CurrentNumberPassword { get; set; }
    }
}

