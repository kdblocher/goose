namespace GooseServer

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Serilog
open Formatters
open Microsoft.Extensions.Configuration

type Startup (config: IConfiguration) =

  member _.ConfigureServices (services: IServiceCollection) : unit =
    ignore <| services
      .AddMvc().Services
      .AddControllers(fun options ->
        options.InputFormatters.Insert(0, PlainTextInputFormatter())
        options.OutputFormatters.Insert(0, PlainTextOutputFormatter())).Services
      .AddSignalR().Services
      .AddSingleton(config.GetSection("Domain").Get<DomainConfig>())

  // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
  member _.Configure (app: IApplicationBuilder) : unit =
    ignore <| app
      // .UseSerilogRequestLogging()
      .UseCors(fun options ->
        ignore <| options
          .AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod())
      .UseRouting()
      .UseEndpoints(fun endpoints ->
        ignore <| endpoints.MapHub<GooseHub>("/goose")
        ignore <| endpoints.MapControllers())