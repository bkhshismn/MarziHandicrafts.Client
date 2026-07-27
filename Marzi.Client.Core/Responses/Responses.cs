using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marzi.Client.Core.Responses
{
    public class LoginResponse
    {
        public Guid UserId { get; set; }

        public string AccessToken { get; set; } = "";

        public string RefreshToken { get; set; } = "";

        public string FullName { get; set; } = "";
    }
}
