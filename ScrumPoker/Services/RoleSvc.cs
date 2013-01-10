using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ScrumPoker.Models;
namespace ScrumPoker.Services
{
    public class RoleSvc
    {
        public List<Roles> GetAll()
        {

            using (ScrumPoker.Entities entitiesDb = new Entities())
            {
                var Rolesquery = (from r in entitiesDb.Roles
                                  select r).ToList<Roles>();
                
                return Rolesquery;
            }            
        }
    }
}