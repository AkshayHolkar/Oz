using System.Collections.Generic;

namespace Oz.Authentication
{
    public class AuthFailResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
