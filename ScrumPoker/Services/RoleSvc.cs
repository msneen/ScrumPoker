using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ScrumPoker.Models;
namespace ScrumPoker.Services
{
    public class RoleSvc
    {
        Entities _entitiesDb; // = new Entities();
        public RoleSvc(Entities entitiesDb)
        {
            _entitiesDb = entitiesDb;
        }
        public List<Role> GetAll()
        {
            var Rolesquery = (from r in _entitiesDb.Roles1
                                select r).ToList<Role>();
                
            return Rolesquery;                       
        }
    }
}