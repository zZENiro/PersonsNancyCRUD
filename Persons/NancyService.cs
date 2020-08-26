using System;
using System.Linq;
using Nancy.Hosting.Self;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Persons
{
    public class NancyService
    {
        private Lazy<NancyHost> _nancyHost { get; set; }

        private HostConfiguration _nancyHostConfiguration { get; set; }

        private NancyServiceConfiguration _nancyServiceConfiguration { get; set; }

        private static ILogger _logger;

        public void ConfigureLogger(LoggerConfiguration loggerConfiguration)
        {
            _logger = loggerConfiguration.CreateLogger();
        }

        public void Configure(NancyServiceConfiguration nancyServiceConfiguration)
        {
            var nancyHostConfiguration = new HostConfiguration();
            var loggerConfig = new LoggerConfiguration();

            nancyServiceConfiguration.NancyHostConfigurator?.Invoke(nancyHostConfiguration);
            nancyServiceConfiguration.LoggerConfigurator?.Invoke(loggerConfig);

            _logger = loggerConfig.CreateLogger();
            _nancyServiceConfiguration = nancyServiceConfiguration;
            _nancyHostConfiguration = nancyHostConfiguration;

            _nancyHost = new Lazy<NancyHost>(CreateNancyHost);
        }

        private NancyHost CreateNancyHost()
        {
            var uris = _nancyServiceConfiguration.Uris.ToArray();

            if (_nancyServiceConfiguration.Bootstrapper != null)
                return new NancyHost(_nancyServiceConfiguration.Bootstrapper, _nancyHostConfiguration, uris);

            return new NancyHost(_nancyHostConfiguration, uris);
        }

        //CreateNancyHost

        public void Start()
        {
            _logger.Information("[Person.Service] Starting NancyHost");
            _nancyHost.Value.Start();
            _logger.Information("[Person.Service] NancyHost started");
        }

        public void Stop()
        {
            _logger.Information("[Person.Service] Stopping NancyHost");
            _nancyHost.Value.Stop();
            _logger.Information("[Person.Service] NancyHost stopped");
        }
    }
}
