using CrossCutting.Core.Contract.Bootstrapping;
using CrossCutting.Core.Contract.Configuration;
using CrossCutting.Core.Contract.DependencyInjection;
using CrossCutting.Core.Contract.DependencyInjection.DataClasses;
using CrossCutting.Core.Contract.EventBrokerage;
using Logic.Business.ImageExtractor.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Business.ImageExtractor.InternalContract;

namespace Logic.Business.ImageExtractor
{
    public class ImageExtractorActivator : IComponentActivator
    {
        public void Activating()
        {
        }

        public void Activated()
        {
        }

        public void Deactivating()
        {
        }

        public void Deactivated()
        {
        }

        public void Register(ICoCoKernel kernel)
        {
            kernel.Register<IImageExtractorWorkflow, ImageExtractorWorkflow>(ActivationScope.Unique);
            kernel.Register<IExtractionWorkflow, ExtractionWorkflow>(ActivationScope.Unique);

            kernel.Register<IConfigurationValidator, ConfigurationValidator>(ActivationScope.Unique);

            kernel.RegisterConfiguration<ImageExtractorConfiguration>();
        }

        public void AddMessageSubscriptions(IEventBroker broker)
        {
        }

        public void Configure(IConfigurator configurator)
        {
        }
    }
}
