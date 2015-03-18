using Chicken.DAL;
using Chicken.Domain.Interfaces;
using Chicken.Services;
using Ninject;
using Ninject.Web.Common;

namespace Chicken.IoC
{
    public static class NinjectConfig
    {
        public static NinjectDependencyResolver GetNinjectDependencyResolver()
        {
            var ninjectKernel = new StandardKernel();
            ninjectKernel.Bind<ChickenDbContext>().ToSelf().InRequestScope();
            ninjectKernel.Bind(typeof(IRepository<>)).To(typeof(EFRepository<>));
            ninjectKernel.Bind(typeof (ChickenService)).ToSelf();
            return new NinjectDependencyResolver(ninjectKernel);
        }
    }
}