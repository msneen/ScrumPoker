using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumPoker.Services
{
    public class SiteAdminSvc
    {
        public static bool IsSiteAdmin()
        {
            //this is just temporary.  Should be determined by membership classes
            return ScrumMasterSvc.IsScrumMaster();
        }
    }
}