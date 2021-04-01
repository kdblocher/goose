namespace GooseServer
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.SignalR
open Microsoft.Extensions.Caching.Memory
open System.DirectoryServices.AccountManagement

type User = {
  Username: string
  DisplayName: string
}

[<CLIMutable>]
type DomainConfig = {
  Name: string
  Container: string
}

type GooseController (hubContext: GooseHub IHubContext, cache: IMemoryCache, config: DomainConfig) =
  inherit Controller ()

  [<HttpGet>]
  [<Route "/user">]
  member _.GetUser () =
    cache.Get "User" |> string

  [<HttpPut>]
  [<Route "/user">]
  member _.SetUser ([<FromBody>] userName: string) =
    ignore <| cache.Set ("User", userName)
    hubContext.Clients.All.SendAsync ("SetUser", userName)

  [<HttpGet>]
  [<Route "/users">]
  member _.GetAllUsers () =
    use context = new PrincipalContext(ContextType.Domain, config.Name, config.Container)
    use user = new UserPrincipal(context)
    use search = new PrincipalSearcher(user)
    search.FindAll()
      |> Seq.map (fun u -> {
        Username = u.SamAccountName
        DisplayName = u.DisplayName
      })
      |> Seq.filter (fun u -> u.DisplayName.Contains(" "))
      |> Seq.sortBy (fun u -> u.DisplayName)
      |> Seq.toArray