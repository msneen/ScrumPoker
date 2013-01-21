using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Ninject;

namespace ScrumPoker
{
    public class IocConfig
    {
        public static void RegisterIoc(HttpConfiguration config)
        {
            var kernel = new StandardKernel(); // Ninject IoC

            System.Web.Mvc.DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}