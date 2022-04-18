using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace SM.Entity
{
    public partial class UserRole
    {
        [DisplayName("Compsitekey")]
        public int Compsitekey { get; set; }

        [DisplayName("User Id")]
        public int UserId { get; set; }

        [DisplayName("UserRole Id")]
        public int UserRoleId { get; set; }

        public virtual User User { get; set; }
    }
}
