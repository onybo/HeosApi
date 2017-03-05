// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#r @"..\..\packages\Rssdp.3.0.0\lib\net45\Rssdp.Native.dll"
#r @"System.Net.Http.dll"
#load "Discovery.fs"
open HeosApi
open Rssdp

Discovery.searchForDenonDevices ["kjøkken" ;  "stue"] 
|> Async.AwaitTask
|> Async.RunSynchronously
|> Seq.iter (fun d -> Discovery.displayDevice d)
