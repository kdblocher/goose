namespace GooseServer
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Serilog
open Shared
open System
module Program =

  let getBuilder args =
    Log.Logger <- Logger.getBootstrap ()
    Host
      .CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(fun webHostBuilder ->
          ignore <| webHostBuilder
            .CaptureStartupErrors(true)
            .ConfigureAppConfiguration(Action<_,_>(fun _ -> Configuration.baseConfigure))
            .UseSetting("detailedErrors", "true")
            .UseStartup<Startup>())
        .UseSerilog(Action<_,_,_>(Logger.getMain))
        .ConfigureLogging(fun logging ->
          ignore <| logging
            .AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug)
            .AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Debug))

  [<EntryPoint>]
  let main args =
    let host = (getBuilder args).Build().Run()
    0