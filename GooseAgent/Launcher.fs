namespace GooseAgent
open System.Diagnostics
open System
open System.Threading.Tasks

module Async =
  let toTask token wf =
    Async.StartAsTask (wf, TaskCreationOptions.None, token) :> Task

module Launcher =
  type RunningProcess = {
    Process: Process
    RunningTask: Task
  }

  let start (path: string) token =
    let p = Process.Start path
    { Process = p; RunningTask = p.WaitForExitAsync token }

  let stop (p: RunningProcess) =
    p.Process.Kill ()