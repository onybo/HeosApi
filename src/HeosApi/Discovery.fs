namespace HeosApi

module Discovery =

   open Rssdp
   open System

   let private contains (toCheck:string) (source:string) = 
      source.IndexOf (toCheck,StringComparison.OrdinalIgnoreCase) >= 0

   let private getDetails names (foundDevice:DiscoveredSsdpDevice)  = 
      async {
         let! fullDevice = foundDevice.GetDeviceInfo() |> Async.AwaitTask
         printfn "%s" fullDevice.FriendlyName
         if (Seq.exists (fun n -> contains n fullDevice.FriendlyName) names) then
               printfn "%s" fullDevice.DeviceType
               printfn "%s" fullDevice.Manufacturer
               printfn ""     
         return 10             
      }  

   let searchForDenonDevices2 (names:seq<string>) =
      async {
         use deviceLocator = new SsdpDeviceLocator()
         let! devices = deviceLocator.SearchAsync("urn:schemas-denon-com:device:ACT-Denon:1") |> Async.AwaitTask
         return! Seq.map 
                  (fun (d:DiscoveredSsdpDevice) -> d.GetDeviceInfo() |> Async.AwaitTask) 
                     devices
                  |> Async.Parallel
      } |> Async.StartAsTask    
   
   let searchForDenonDevices (names:seq<string>) =
      let matchDeviceByName (device:SsdpDevice) =
         Seq.exists (fun (n:string) -> device.FriendlyName.ToLower() = "act-" + n.ToLower()) names 

      async {
         use deviceLocator = new SsdpDeviceLocator()
         let! discovered = deviceLocator.SearchAsync("urn:schemas-denon-com:device:ACT-Denon:1") |> Async.AwaitTask
         let! devices = Seq.map 
                          (fun (d:DiscoveredSsdpDevice) -> d.GetDeviceInfo() |> Async.AwaitTask) 
                          discovered 
                        |> Async.Parallel
         return Seq.filter 
                     (fun (device:SsdpDevice) -> matchDeviceByName device) 
                     devices
      } |> Async.StartAsTask 
      
   let displayDevice (d:SsdpDevice) =
      printfn "%s" d.FriendlyName         
      printfn "%s" d.ModelName
      match d with 
      | :? SsdpRootDevice as root -> printfn "%s" root.Location.Host
      | _ -> ()
      printfn ""      