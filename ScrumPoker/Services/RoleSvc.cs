using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ScrumPoker.Models;
namespace ScrumPoker.Services
{
    public class RoleSvc
    {
        public List<webpages_Roles> GetAll()
        {

            using (ScrumPoker.Entities entitiesDb = new Entities())
            {
                var Rolesquery = (from r in entitiesDb.webpages_Roles
                                  select r).ToList<webpages_Roles>();
                
                return Rolesquery;
            }            
        }
    }
}