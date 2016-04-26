[<AutoOpen>]
module C4FSharp.Helpers

open System.Configuration
open System.Diagnostics
open Microsoft.ApplicationInsights.TraceListener
open Suave.Logging

///// Starts all trace listeners.
let startTracing() =
    Trace.AutoFlush <- true
    Trace.Listeners.Add (new ConsoleTraceListener()) |> ignore
    Trace.Listeners.Add (new ApplicationInsightsTraceListener()) |> ignore   

let getSetting (key:string) = ConfigurationManager.AppSettings.[key]

let logger =    
    { new Logger with
        member __.Log _ fline =
            let logLine = fline()
            match logLine with
            | { ``exception`` = Some exn } -> Trace.TraceError(exn.ToString())
            | { level = LogLevel.Error } -> Trace.TraceError logLine.message
            | { level = LogLevel.Warn } -> Trace.TraceWarning logLine.message
            | _ -> Trace.TraceInformation logLine.message
            Trace.Flush() }
