# Installation
A few development tools are needed to build the source. No runtime tools are needed, since [.NET 5 allows self-contained deployments](https://docs.microsoft.com/en-us/dotnet/core/deploying/single-file).

### Dev tools
- [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
- [Node.js](https://nodejs.org/en/download) (v12.0 or later should suffice)
  - [Yarn](https://classic.yarnpkg.com/en/docs/install)

#### For debugging / testing
- [Visual Studio Code](https://code.visualstudio.com/download) - the launch/tasks configuration files are in the source at the root directory.

### Instructions
1. Clone the source to your dev server.
2. Download [Desktop Goose](https://samperson.itch.io/desktop-goose) and put it somewhere on your network where all users have read+execute permissions.

#### Server
1. Edit `GooseServer/config/appsettings.json`.
    1. In the `Domain` section, change `Name` and `Container` to the local AD domain name and the top-level organizational unit that your user accounts reside under.
2. Run `dotnet run` in the `GooseServer` directory.
    1. If you want to publish the server somewhere instead, you can use `dotnet publish --self-contained --runtime win10-x64 -p:PublishSingleFile=true` and then run it as an executable elsewhere.
    2. Replace `win10-x64` with the runtime of your target OS if not Windows.

#### Client
1. Edit `goose-ui/.env`, setting the value of `REACT_APP_GOOSE_API_URL` to the base URL of wherever you deployed your server.
2. Run `yarn install`.
3. Run `yarn start` and verify the client comes up (and users show up in the dropdown)
4. Run `yarn run build` for hosting it elsewhere - the output shows up in the `build` folder.

#### Agent
1. Edit `GooseAgent/config/appsettings.json`.
    1. In the `Service` section, change:
    2. `ServerUrl` to `{baseUrl}/goose`, where `{baseUrl}` is wherever you deployed your server.
    3. `ExecutablePath` to the location of `GooseDesktop.exe` on the network. (You will have to escape any backslashes, e.g. `\\\\server\\share\\path\\to\\GooseDesktop.exe`
2. Run `dotnet run` in the `GooseAgent` directory.
3. If you did everything correctly, you can set the goose on yourself by picking your current user account in the client dropdown!
4. Run `dotnet publish --self-contained --runtime win10-x64 -p:PublishSingleFile=true`.
5. Use your IT admin tool of choice to get the agent running automatically under every user account (on login, or periodically, or whatever).

   
