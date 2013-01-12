using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace ScrumPoker.Services
{
    public class ProjectSvc
    {
        public List<Project> GetAll()
        {
            using (ScrumPoker.Entities entitiesDb = new Entities())
            {
                var projectQuery = from p in entitiesDb.Projects
                                   select p;
                return projectQuery.ToList<Project>();
            }
        }

        public Project Find(int id)
        {
            using (ScrumPoker.Entities entitiesDb = new Entities())
            {
                return entitiesDb.Set<Project>().Include(p => p.TeamMembers).FirstOrDefault(p => p.id == id);
            }
        }
    }
}