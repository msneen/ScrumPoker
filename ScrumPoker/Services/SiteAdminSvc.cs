using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumPoker.Services
{
    public class SiteAdminSvc
    {
        public static bool IsSiteAdmin(string userName)
        {
            return false; // UserProfileSvc.IsInRole(userName, "SiteAdmin"); // turn this on to add roles to your database
        }
    }
}