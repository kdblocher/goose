namespace Shared
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open System.IO
open System

module Configuration =
  let baseConfigure (configurationBuilder: IConfigurationBuilder) =
    ignore <| configurationBuilder
      .SetBasePath(Path.Combine(AppContext.BaseDirectory, "config"))
      .AddJsonFile("appsettings.json", optional = true, reloadOnChange = false)    