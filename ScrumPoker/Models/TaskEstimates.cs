using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ScrumPoker.Models
{
    public class TaskEstimates
    {
          private static object estimateListLocker = new object();

          #region Session Project Id
          public static int GetProjectId()
          {
              int projectId = 0;
              if (HttpContext.Current.Session["CurrentProjectId"] != null)
              {
                  projectId = Convert.ToInt32(HttpContext.Current.Session["CurrentProjectId"]);
              }
              return projectId;
          }
          public static void SetProject(object projectId)
          {
              HttpContext.Current.Session["CurrentProjectId"] = projectId;
          }
          public static void ResetProject()
          {
              HttpContext.Current.Session.Remove("CurrentProjectId");
          }
          #endregion

          public static void SetVotingTaskId(string id)
          {
              lock (estimateListLocker)
              {
                  GetPokerApplicationData().TaskId = id;
              }
          }

          public static string GetVotingTaskId()
          {
              lock (estimateListLocker)
              {
                  return GetPokerApplicationData().TaskId;
              }
          }

          public static List<TaskEstimate> GetEstimateList()
          {
              
              lock (estimateListLocker)
              {
                    int projectId = GetProjectId();
                    if (projectId > 0)
                    {
                        PokerApplicationData pokerApplicationData = GetPokerApplicationData();

                        List<TaskEstimate> estimateList = pokerApplicationData.TaskEstimates;
                        if (estimateList != null)
                        {
                            return estimateList;
                        }
                        
                        return SetEstimateList(new List<TaskEstimate>(), projectId);
                    }
                    return null;
              }

          }

          public static List<TaskEstimate> GetEstimateList(int projectId)
          {
              if (projectId > 0)
              {
                  //add project id to session
                  SetProject(projectId);
                  return GetEstimateList();
              }
              return new List<TaskEstimate>();
          }

          public static List<TaskEstimate> SetEstimateList(List<TaskEstimate> estimateList)
          {
                int projectId = GetProjectId();
                if (projectId > 0)
                {
                    return SetEstimateList(estimateList, projectId);
                }
                return null;
          }

          public static List<TaskEstimate> SetEstimateList(List<TaskEstimate> estimateList, int projectId)
          {
              //save the list by id here
              SetProject( projectId);
              lock (estimateListLocker)
              {
                  PokerApplicationData pokerApplicationData = GetPokerApplicationData();
                  pokerApplicationData.TaskEstimates = estimateList;
                  
                  return estimateList;
              }              
          }

          private static PokerApplicationData GetPokerApplicationData()
          {
              string appName = string.Format("TE{0}", GetProjectId());
              PokerApplicationData pokerApplicationData = System.Web.HttpContext.Current.Application[appName] as PokerApplicationData;
              if (pokerApplicationData == null)
              {
                  pokerApplicationData = new PokerApplicationData();
                  System.Web.HttpContext.Current.Application[appName] = pokerApplicationData;
              }
              return pokerApplicationData;
          }                  
    }

    public class PokerApplicationData
    {
        public string TaskId { get; set; }
        public List<TaskEstimate> TaskEstimates { get; set; }
    }
}