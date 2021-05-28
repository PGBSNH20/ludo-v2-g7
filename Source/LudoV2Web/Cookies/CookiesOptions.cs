using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoV2Web.Cookies
{
    public static class CustomCookiesOptions
    {
        public static CookieOptions CustomCookieOptions()
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1)
            };

            return cookieOptions;
        }
    }
}
