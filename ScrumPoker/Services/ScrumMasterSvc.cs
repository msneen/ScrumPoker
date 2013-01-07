using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security;
using System.Security.Principal;

namespace ScrumPoker.Services
{
    public class ScrumMasterSvc
    {
        public static bool IsScrumMaster()
        {
            if ("msneen".Equals(HttpContext.Current.User.Identity.Name, StringComparison.OrdinalIgnoreCase)
            || "michael.sneen@gmail.com".Equals(HttpContext.Current.User.Identity.Name, StringComparison.OrdinalIgnoreCase)
            || "michael.sneen.gmail.com".Equals(HttpContext.Current.User.Identity.Name, StringComparison.OrdinalIgnoreCase)
            || HttpContext.Current.User.Identity.Name == "dle"
            || HttpContext.Current.User.Identity.Name == "jjohnston"
            || HttpContext.Current.User.Identity.Name == "rhanson"
            || HttpContext.Current.User.Identity.Name == "rodoughty")
            {
                return true;
            }
            return false;
        }
    }
}