namespace GooseServer
open Microsoft.AspNetCore.SignalR

type GooseHub () =
  inherit Hub ()

  member this.SetUser (user: string) =
    this.Clients.All.SendAsync ("SetUser", user)
  member this.Kill () =
    this.Clients.All.SendAsync "Kill"