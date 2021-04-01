namespace Shared
open Serilog
open Serilog.Events
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open System

module Logger =

  let getBootstrap = 
    LoggerConfiguration >> fun config ->
    config
      .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
      .Enrich.FromLogContext()
      .WriteTo.Console()
      .CreateBootstrapLogger()

  let getMain (context: HostBuilderContext) (services: IServiceProvider)(config: LoggerConfiguration) =
    ignore <| config
      // .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
      .ReadFrom.Configuration(context.Configuration)
      .ReadFrom.Services(services)
      .Enrich.FromLogContext()
      .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
      .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
      .Enrich.WithMachineName()
      .Enrich.WithEnvironmentUserName()
      .Enrich.WithAssemblyName()
      .Enrich.WithAssemblyInformationalVersion()