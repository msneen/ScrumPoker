using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumPoker.Models
{
    public class Users
    {
        private static object theLocker = new object();

        public static List<User> UserList
        {
            get
            {
                lock (theLocker)
                {
                    return System.Web.HttpContext.Current.Application["Users"] as List<User>;
                }
            }
            set
            {
                lock (theLocker)
                {
                    System.Web.HttpContext.Current.Application["Users"] = value;
                }
            }
        }
    }
}