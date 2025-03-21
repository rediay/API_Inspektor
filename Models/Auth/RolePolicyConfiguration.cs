using System.Collections.Generic;

namespace Inspektor_API_REST.Models.Auth
{
    public class RolePolicyConfiguration
    {
        public string policy { get; set; }
        public string[] roles { get; set; }
    }
}
