using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumPoker.Services
{
    public class FinalEstimateSvc
    {
        private Entities _entitiesDb; // = new Entities();

        public FinalEstimateSvc(Entities entitiesDb)
        {
            _entitiesDb = entitiesDb;
        }

        public List<FinalEstimate> GetAll()
        {

            var projectQuery = from p in _entitiesDb.FinalEstimates
                                select p;
            return projectQuery.ToList<FinalEstimate>();
            
        }

        public FinalEstimate Find(int id)
        {
            return _entitiesDb.Set<FinalEstimate>().FirstOrDefault(p => p.id == id);            
        }

        public void Add(FinalEstimate finalEstimate)
        {
            var existingFinalEstimate = (from fe in _entitiesDb.FinalEstimates
                        where fe.ProjectId == finalEstimate.ProjectId
                        && fe.TaskId == finalEstimate.TaskId
                            select fe).FirstOrDefault();
            if (existingFinalEstimate != null)
            {
                existingFinalEstimate.Estimate = finalEstimate.Estimate;
            }
            else
            {
                _entitiesDb.FinalEstimates.Add(finalEstimate);
            }
            _entitiesDb.SaveChanges();            
        }
    }
}