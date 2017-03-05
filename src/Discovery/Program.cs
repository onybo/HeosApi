using Rssdp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discovery
{
    class Program
    {
        static void Main(string[] args)
        {
            var devices = 
                HeosApi.Discovery.searchForDenonDevices(
                    new[] { "kjøkken", "stue" })
                    .Result;
            devices
                .ToList()
                .ForEach(HeosApi.Discovery.displayDevice);
              
            Console.ReadKey();
        }
    }        
}
