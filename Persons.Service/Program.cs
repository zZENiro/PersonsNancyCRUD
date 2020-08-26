using Microsoft.Extensions.Configuration;
using Nancy.Hosting.Self;
using Serilog;
using System.IO;
using Topshelf;
using Topshelf.LibLog;
using Topshelf.LibLog.Logging;
using Topshelf.Nancy;
using Serilog.Sinks.SystemConsole;

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;
using Serilog.Configuration;
using Serilog.Settings.Configuration;
using Serilog.Settings.Configuration.Assemblies;


namespace Persons.Service
{
    class Program
    {
        static void Main()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path: "config.json", optional: false, reloadOnChange: true)
                .Build();

            var loggerConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration);

            var host = HostFactory.New(x =>
            {
                x.Service<ApplicationService>(s =>
                {
                    s.ConstructUsing(settings => new ApplicationService());
                    s.WhenStarted(service => service.Start());
                    s.WhenStopped(service => service.Stop());
                    s.WithNancyEndpoint(x, c =>
                    {
                        c.ConfigureNancy(config =>
                            config.UrlReservations = new UrlReservations() { CreateAutomatically = true, User = "Everyone" });

                        c.UseBootstrapper(new Bootstrapper(loggerConfig, configuration));
                        
                        c.AddHost(port: 20005);
                        c.AddHost(port: 20006);
                        c.CreateUrlReservationsOnInstall();
                        c.OpenFirewallPortsOnInstall(firewallRuleName: "Persons.Service.ApplicationService");
                    });
                });
                x.StartAutomatically();
                x.SetServiceName("Persons.Service.ApplicationService");
                x.SetDisplayName("Persons.Service.ApplicationService");
                x.SetDescription("Persons.Service.ApplicationService project");
                x.RunAsNetworkService();
            });

            host.Run();
        }
    }
}
