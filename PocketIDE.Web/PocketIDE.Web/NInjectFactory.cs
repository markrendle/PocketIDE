using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using Ninject;

namespace PocketIDE.Web
{
    public static class NInjectFactory
    {
        private static readonly Lazy<IKernel> Kernel = new Lazy<IKernel>(CreateKernel, LazyThreadSafetyMode.PublicationOnly);

        public static T Get<T>()
        {
            return Kernel.Value.Get<T>();
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            return kernel;
        }
    }
}