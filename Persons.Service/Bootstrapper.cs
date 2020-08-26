using Microsoft.Extensions.Configuration;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Extensions;
using Nancy.TinyIoc;
using Persons.Abstractions.Models;
using Persons.Abstractions.Repositories;
using Persons.Data;
using Serilog;
using Serilog.Core;

namespace Persons.Service
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private readonly LoggerConfiguration _loggerConfiguration;

        public Bootstrapper(LoggerConfiguration loggerConfiguration, IConfigurationRoot configuration)
        {
            _loggerConfiguration = loggerConfiguration;
            Configuration = configuration;
        }

        public IConfigurationRoot Configuration { get; }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            container.Register<ILogger, Logger>(_loggerConfiguration.CreateLogger());

            container.Register<IAgeComputingFactory, PersonAgeComputingFactory>();

            container.Register<IPersonRepository, PersonRepository>(
                new PersonRepository(Configuration.GetSection("ConnectionString").Value, container.Resolve<ILogger>()));
        }
    }
}
