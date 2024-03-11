using ApiTcc.Infra.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Entities
{
    public class User : EntityBase
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string ResetPasswordToken { get; set; }
        public DateTime ResetPasswordTokenExpiry { get; set; }
        public Guid AlunoId { get; set; }
    }
}
