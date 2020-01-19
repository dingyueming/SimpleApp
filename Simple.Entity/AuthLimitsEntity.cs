using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Entity
{
    [Table("AUTH_LIMITS")]
    public class AuthLimitsEntity
    {
        [ExplicitKey]
        public int UserId { get; set; }

        public int CarId { get; set; }

        public int IsSendad { get; set; }
    }
}
