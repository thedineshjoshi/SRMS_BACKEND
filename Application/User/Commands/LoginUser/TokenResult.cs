using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.Commands.LoginUser
{
    public class TokenResult
    {
        public string? Token { get; set; }
        public DateTime Expiration { get; set; }
        public string Error { get; set; }
        public int StatusCode { get; set; }
        public bool Succeeded { get; set; }

    }
}
