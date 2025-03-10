﻿using CrossCutting.Core.Bootstrapping;
using CrossCutting.Core.Configuration;
using CrossCutting.Core.Configuration.CommandLine;
using CrossCutting.Core.Configuration.ConfigObjects;
using CrossCutting.Core.Configuration.File;
using CrossCutting.Core.Contract.Bootstrapping;
using CrossCutting.Core.Contract.Configuration;
using CrossCutting.Core.Contract.DependencyInjection;
using CrossCutting.Core.Contract.DependencyInjection.DataClasses;
using CrossCutting.Core.Contract.EventBrokerage;
using CrossCutting.Core.Contract.Logging;
using CrossCutting.Core.Contract.Scheduling;
using CrossCutting.Core.DI.AutofacAdapter;
using CrossCutting.Core.EventBrokerage;
using CrossCutting.Core.Logging.NLogAdapter;
using CrossCutting.Core.Scheduling.QuartzAdapter;
using Logic.Business.ImageExtractor;
using Logic.Domain.Kuriimu2.KomponentAdapter;
using Logic.Domain.Kuriimu2.KompressionAdapter;
using Logic.Domain.Kuriimu2.KryptographyAdapter;
using Logic.Domain.Level5Management;

namespace Mappings.ImageExtractor
{
    public class KernelInitializer : IKernelInitializer
    {
        private IKernelContainer _kernelContainer;

        public IKernelContainer CreateKernelContainer()
        {
            if (_kernelContainer == null)
            {
                _kernelContainer = new KernelContainer();
            }
            return _kernelContainer;
        }

        public void Initialize()
        {
            RegisterCoreComponents(_kernelContainer.Kernel);
            ActivateComponents(_kernelContainer.Kernel);
        }

        private void RegisterCoreComponents(ICoCoKernel kernel)
        {
            kernel.Register<IBootstrapper, Bootstrapper>(ActivationScope.Unique);
            kernel.Register<IEventBroker, EventBroker>(ActivationScope.Unique);
            kernel.Register<IConfigurationRepository, FileConfigurationRepository>();
            kernel.Register<IConfigurationRepository, CommandLineConfigurationRepository>();
            kernel.Register<IConfigurator, Configurator>(ActivationScope.Unique);
            kernel.Register<IConfigObjectProvider, ConfigObjectProvider>(ActivationScope.Unique);
            kernel.Register<ILogger, Logger>(ActivationScope.Unique);
            kernel.Register<IScheduler, Scheduler>(ActivationScope.Unique);
        }

        private void ActivateComponents(ICoCoKernel kernel)
        {
            kernel.RegisterComponent<ImageExtractorActivator>();
            kernel.RegisterComponent<Kuriimu2KomponentActivator>();
            kernel.RegisterComponent<Kuriimu2KompressionActivator>();
            kernel.RegisterComponent<Kuriimu2KryptographyActivator>();
            kernel.RegisterComponent<Level5ManagementActivator>();
        }
    }
}