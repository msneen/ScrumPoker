using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ScrumPoker.Models
{
    public class TaskEstimates
    {
          private static object theLocker = new object();

          public static List<TaskEstimate> GetEstimateList()
          {
              int? projectId = System.Web.HttpContext.Current.Session["CurrentProjectId"] as int?;

              if (projectId != null && projectId.HasValue == true)
              {
                  return GetEstimateList(projectId.Value);
              }
              return new List<TaskEstimate>();
          }

          public static List<TaskEstimate> GetEstimateList(int projectId)
          {
              string appName = string.Format("TE{0}", projectId.ToString());
              lock (theLocker)
              {
                  List<TaskEstimate> estimateList = System.Web.HttpContext.Current.Application[appName] as List<TaskEstimate>;
                  if (estimateList == null)
                  {
                      return SetEstimateList(new List<TaskEstimate>(), projectId);
                  }
                  else
                  {
                      return estimateList;
                  }
              }
              
          }

          public static List<TaskEstimate> SetEstimateList(List<TaskEstimate> estimateList)
          {
                int? projectId = System.Web.HttpContext.Current.Session["CurrentProjectId"] as int?;

                if (projectId != null && projectId.HasValue == true)
                {
                    return SetEstimateList(estimateList, projectId.Value);
                }
                return null;
          }

          public static List<TaskEstimate> SetEstimateList(List<TaskEstimate> estimateList, int projectId)
          {
              //save the list by id here
              System.Web.HttpContext.Current.Session["CurrentProjectId"] = projectId;
              string appName = string.Format("TE{0}", projectId.ToString());
              lock (theLocker)
              {
                  System.Web.HttpContext.Current.Application[appName] = estimateList;
                  return estimateList;
              }              
          }
                   
          //public static List<TaskEstimate> EstimateList
          //{
          //  get
          //  {
          //    lock (theLocker)
          //    {
          //        return System.Web.HttpContext.Current.Application["TaskEstimates"] as List<TaskEstimate>;
          //    }
          //  }
          //  set
          //  {
          //    lock (theLocker)
          //    {
          //        System.Web.HttpContext.Current.Application["TaskEstimates"] = value;
          //    }
          //  }
          //}
    }
}