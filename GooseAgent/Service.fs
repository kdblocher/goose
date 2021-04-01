namespace GooseAgent
open Microsoft.Extensions.Hosting
open System.Threading.Tasks
open System.Diagnostics
open Microsoft.AspNetCore.SignalR.Client
open Serilog
open System
open System.Security.Principal
open System.Threading
open Launcher

[<CLIMutable>]
type ServiceConfig = {
  ServerUrl: string
  ExecutablePath: string
  HandshakeTimeout: int
  ServerTimeout: int
}

type User = {
  Username: string
}

type Service (config: ServiceConfig, runningUser: User, lifetime: IHostApplicationLifetime) =
  inherit BackgroundService ()

  let connection =
    HubConnectionBuilder()
      .WithUrl(config.ServerUrl)
      .ConfigureLogging(fun logging -> ignore <| logging.AddSerilog(Log.Logger, true))
      .Build()

  let keepAlive = Func<exn, Task>(fun e ->
    async {
      Log.Warning (e, "")
      do! Async.Sleep ((config.HandshakeTimeout |> float |> TimeSpan.FromSeconds).TotalMilliseconds |> int)
      do! connection.StartAsync () |> Async.AwaitTask
    } |> Async.StartAsTask :> Task)

  let mutable exe : RunningProcess option = None
  let mutable exeToken : CancellationToken = CancellationToken.None

  let killProcess () = exe |> Option.iter stop; exe <- None
  let toggleProcess = Action<string>(fun targetUsername ->
    if runningUser.Username.Equals(targetUsername, StringComparison.InvariantCultureIgnoreCase) && exe.IsNone
    then exe <- Some <| start config.ExecutablePath exeToken
    else killProcess ()
  )
  do
    connection.add_Closed keepAlive
    connection.On<string>("SetUser", toggleProcess) |> ignore
    connection.On("Kill", killProcess) |> ignore
  
  override _.ExecuteAsync token =
    exeToken <- token
    connection.HandshakeTimeout <- config.HandshakeTimeout |> float |> TimeSpan.FromSeconds
    connection.ServerTimeout <- config.ServerTimeout |> float |> TimeSpan.FromSeconds
    async {
      try
        do! connection.StartAsync token |> Async.AwaitTask
        Log.Information("SignalR connection initialized")
      with e ->
        Log.Error (e, "Failed to connect to server")
        lifetime.StopApplication ()
    } |> Async.StartAsTask :> Task

  override _.StopAsync token =
    connection.remove_Closed keepAlive
    killProcess ()
    connection.StopAsync token