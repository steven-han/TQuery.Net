using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace TQuery.Net
{
    public class EncryptHelper
    {
        public static string MD5(string Str)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(Str, "MD5");
        }


    }
}
