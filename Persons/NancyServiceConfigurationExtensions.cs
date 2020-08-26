using Serilog;
using System;
using Topshelf;
using Topshelf.HostConfigurators;
using Topshelf.ServiceConfigurators;

namespace Persons
{
    public static class NancyServiceConfiguratorExtensions
    {
        public static ServiceConfigurator<T> WithNancyEndpoint<T>(
            this ServiceConfigurator<T> configurator,
            Action<NancyServiceConfiguration> nancyConfigurator) where T : class
        {
            var nancyServiceConfiguration = new NancyServiceConfiguration();
            
            nancyConfigurator(nancyServiceConfiguration);
            
            var nancyService = new NancyService();

            nancyService.Configure(nancyServiceConfiguration);

            configurator.AfterStartingService(t => nancyService.Start());

            configurator.BeforeStoppingService(t => nancyService.Stop());

            return configurator;
        }
    }
}