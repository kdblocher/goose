namespace GooseAgent
open Microsoft.Extensions.Hosting
open Serilog
open Shared
open System
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging

module Program =

  let getBuilder args =
    Log.Logger <- Logger.getBootstrap ()
    Host
      .CreateDefaultBuilder(args)
      .UseSerilog(Action<_,_,_>(Logger.getMain))
      .ConfigureHostConfiguration(Action<_>(Configuration.baseConfigure))
      .ConfigureServices(Action<HostBuilderContext, IServiceCollection>(fun context services ->
        ignore <| services
          .AddSingleton({ Username = Environment.UserName })
          .AddSingleton(context.Configuration.GetSection("Service").Get<ServiceConfig>())
          .AddHostedService<Service>()))
      
  [<EntryPoint>]
  let main args =
    let host = (getBuilder args).Build().Run()
    0