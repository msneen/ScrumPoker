using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace ScrumPoker.Services
{
    public class ProjectSvc
    {
        private Entities _entitiesDb;

        public ProjectSvc(Entities entitiesDb)
        {
            _entitiesDb = entitiesDb;
        }
        public List<Project> GetAll()
        {
            var projectQuery = from p in _entitiesDb.Projects
                                select p;
            return projectQuery.ToList<Project>();           
        }

        public Project Find(int id)
        {
                return _entitiesDb.Set<Project>().Include(p => p.TeamMembers).FirstOrDefault(p => p.id == id);            
        }
    }
}