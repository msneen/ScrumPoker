using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;

namespace ScrumPoker
{
    public class DependencyMapper : NinjectModule
    {
        public override void Load()
        {
            this.Bind<Services.UserProfileSvc>().To<Services.UserProfileSvc>();
        }
    }
}