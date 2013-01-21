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
            return UserProfileSvc.IsInRole(userName, "ScrumMaster");
        }

    }
}