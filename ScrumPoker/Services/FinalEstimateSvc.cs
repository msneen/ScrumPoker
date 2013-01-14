using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumPoker.Services
{
    public class FinalEstimateSvc
    {
        public List<FinalEstimate> GetAll()
        {
            using (ScrumPoker.Entities entitiesDb = new Entities())
            {
                var projectQuery = from p in entitiesDb.FinalEstimates
                                   select p;
                return projectQuery.ToList<FinalEstimate>();
            }
        }

        public FinalEstimate Find(int id)
        {
            using (ScrumPoker.Entities entitiesDb = new Entities())
            {
                return entitiesDb.Set<FinalEstimate>().FirstOrDefault(p => p.id == id);
            }
        }

        public void Add(FinalEstimate finalEstimate)
        {
            using (ScrumPoker.Entities entitiesDb = new Entities())
            {
                var existingFinalEstimate = (from fe in entitiesDb.FinalEstimates
                            where fe.ProjectId == finalEstimate.ProjectId
                            && fe.TaskId == finalEstimate.TaskId
                                select fe).FirstOrDefault();
                if (existingFinalEstimate != null)
                {
                    existingFinalEstimate.Estimate = finalEstimate.Estimate;
                }
                else
                {
                    entitiesDb.FinalEstimates.Add(finalEstimate);
                }
                entitiesDb.SaveChanges();
            }
        }
    }
}