[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Finapp.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Finapp.App_Start.NinjectWebCommon), "Stop")]

namespace Finapp.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using System.Web.Http;
    using Ninject.Web.WebApi;
    using Ninject.Web.Common.WebHost;
    using Finapp.Models;
    using Finapp.IServices;
    using Finapp.Services;
    using Finapp.Interfaces;
    using Finapp.Implementations;
    using Finapp.CreateDatabase;
    using Finapp.ICreateDatabase;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<FinapEntities1>().ToSelf().InRequestScope();
            kernel.Bind<ICreditorAccountService>().To<CreditorAccountService>().InRequestScope();
            kernel.Bind<IDebtorAccountService>().To<DebtorAccountService>().InRequestScope();
            kernel.Bind<IDebtorService>().To<DebtorService>().InRequestScope();
            kernel.Bind<ICreditorService>().To<CreditorService>().InRequestScope();
            kernel.Bind<IAlgorithms>().To<Algorithms>().InRequestScope();
            kernel.Bind<ITransactionOutService>().To<TransactionOutService>().InRequestScope();
            kernel.Bind<IDebtorViewModelService>().To<DebtorViewModelService>().InRequestScope();
            kernel.Bind<ICreditorViewModelService>().To<CreditorViewModelService>().InRequestScope();
            kernel.Bind<ICreator>().To<Creator>().InRequestScope();
            kernel.Bind<IAssociateService>().To<AssociateService>().InRequestScope();
            kernel.Bind<IAssociateViewModelService>().To<AssociateViewModelService>().InRequestScope();
            kernel.Bind<IStatisticsViewModelService>().To<StatisticsViewModelService>().InRequestScope();
            kernel.Bind<ISummaryService>().To<SummaryService>().InRequestScope();
            kernel.Bind<ISummaryViewModelService>().To<SummaryViewModelService>().InRequestScope();
            kernel.Bind<ICreditorToSummaryViewModelService>().To<CreditorToSummaryViewModelService>().InRequestScope();
            kernel.Bind<IDebtorToSummaryViewModelService>().To<DebtorToSummaryViewModelService>().InRequestScope();
            kernel.Bind<ICreditorRankService>().To<CreditorRankService>().InRequestScope();
            kernel.Bind<IDebtorRankService>().To<DebtorRankService>().InRequestScope();
        }
    }
}