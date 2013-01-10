using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security;
using System.Security.Principal;
using WebMatrix.WebData;
using System.Web.Security;
using System.Web.Mvc;
using ScrumPoker.Filters;
using ScrumPoker.Models;
namespace ScrumPoker.Services
{    
    [InitializeSimpleMembership]
    public class ScrumMasterSvc
    {
        public static bool IsScrumMaster(string userName)
        {
            //if ("msneen".Equals(HttpContext.Current.User.Identity.Name, StringComparison.OrdinalIgnoreCase)
            //|| "michael.sneen@gmail.com".Equals(HttpContext.Current.User.Identity.Name, StringComparison.OrdinalIgnoreCase)
            //|| "michael.sneen.gmail.com".Equals(HttpContext.Current.User.Identity.Name, StringComparison.OrdinalIgnoreCase)
            //|| HttpContext.Current.User.Identity.Name == "dle"
            //|| HttpContext.Current.User.Identity.Name == "jjohnston"
            //|| HttpContext.Current.User.Identity.Name == "rhanson"
            //|| HttpContext.Current.User.Identity.Name == "rodoughty")
            //{
            //    return true;
            //}

            return UserProfileSvc.IsInRole(userName, "ScrumMaster");
        }

    }
}