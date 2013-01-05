using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ScrumPoker.Models
{
    public class TaskEstimates
    {
          private static object theLocker = new object();
          
          public static List<TaskEstimate> EstimateList
          {
            get
            {
              lock (theLocker)
              {
                  return System.Web.HttpContext.Current.Application["TaskEstimates"] as List<TaskEstimate>;
              }
            }
            set
            {
              lock (theLocker)
              {
                  System.Web.HttpContext.Current.Application["TaskEstimates"] = value;
              }
            }
          }
    }
}