using System;
using System.Collections.Generic;

#nullable disable

namespace SM.Entity.Entity
{
    public partial class UserRole
    {
        public int Compsitekey { get; set; }
        public int UserId { get; set; }
        public int UserRoleId { get; set; }

        public virtual User User { get; set; }
    }
}
