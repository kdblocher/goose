module GooseServer.Formatters
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc.Formatters
open System.IO
open System.Text

let ContentType = "text/plain"

type PlainTextInputFormatter () as this =
  inherit TextInputFormatter ()

  do
    this.SupportedMediaTypes.Add ContentType
    this.SupportedEncodings.Add Encoding.UTF8
    this.SupportedEncodings.Add Encoding.Unicode

  override _.CanRead context =
    context.HttpContext.Request.ContentType.StartsWith ContentType
  override _.CanReadType t =
    t = typeof<string>
  override _.ReadRequestBodyAsync (context, encoding) =
    async {
      use reader = new StreamReader(context.HttpContext.Request.Body, encoding)
      let! content = reader.ReadToEndAsync() |> Async.AwaitTask
      return! InputFormatterResult.SuccessAsync content |> Async.AwaitTask
    } |> Async.StartAsTask
  
type PlainTextOutputFormatter () as this =
  inherit TextOutputFormatter ()

  do
    this.SupportedMediaTypes.Add ContentType
    this.SupportedEncodings.Add Encoding.UTF8
    this.SupportedEncodings.Add Encoding.Unicode

  override _.CanWriteResult context =
    context.ObjectType = typeof<string>
  override _.WriteResponseBodyAsync (context, encoding) =
    context.HttpContext.Response.WriteAsync (context.Object.ToString(), encoding)