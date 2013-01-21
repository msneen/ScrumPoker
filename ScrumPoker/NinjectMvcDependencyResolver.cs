﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;

namespace ScrumPoker
{
    public class NinjectMvcDependencyResolver : NinjectDependencyScope,
                                           System.Web.Mvc.IDependencyResolver
    {
        private IKernel kernel;

        public NinjectMvcDependencyResolver(IKernel kernel)
            : base(kernel)
        {
            this.kernel = kernel;
        }
    }
}