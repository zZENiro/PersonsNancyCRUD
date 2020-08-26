using System;
using System.Collections.Generic;
using Nancy.Bootstrapper;
using Nancy.Hosting.Self;
using Serilog;

namespace Persons
{
    public class NancyServiceConfiguration
    {
        public List<Uri> Uris { get; set; }

        public Action<HostConfiguration> NancyHostConfigurator { get; set; }

        public Action<LoggerConfiguration> LoggerConfigurator { get; set; }

        public INancyBootstrapper Bootstrapper { get; set; }

        public NancyServiceConfiguration() =>
            Uris = new List<Uri>();

        /// <summary>
        /// Configure Logger via Serilog.LoggerConfiguration
        /// </summary>
        /// <param name="loggerConfigurator"></param>
        public void ConfigureLogger(Action<LoggerConfiguration> loggerConfigurator) =>
            LoggerConfigurator = loggerConfigurator;

        /// <summary>
        /// Configure NancyHost via Nancy.Hosting.Self.HostConfiguration.
        /// </summary>
        public void ConfigureNancyHost(Action<HostConfiguration> nancyHostConfigurator) =>
            NancyHostConfigurator = nancyHostConfigurator;

        /// <summary>
        /// Adds a new Host for Nancy to listen on.
        /// </summary>
        public void AddHost(string scheme = "http", string domain = "localhost", int port = 8080, string path = "") =>
            Uris.Add(new UriBuilder(scheme, domain, port, path).Uri);

        /// <summary>
        /// Set the INancyBootstrapper instance that Nancy will use.
        /// </summary>
        public void UseBootstrapper(INancyBootstrapper bootstrapper) =>
            Bootstrapper = bootstrapper;
    }
}
