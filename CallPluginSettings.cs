using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wox.Plugin.Call.ViewModels;

namespace Wox.Plugin.Call
{
    class CallPluginSettings
    {
        [JsonProperty]
        public List<Entry> Entries { get; set; } = new List<Entry>();

        [JsonProperty]
        public string CallCommandTemplate { get; set; } = "tel://{number}";
    }
    
    class Entry
    {        
        [JsonProperty]
        public string Name { get; set; }
        
        [JsonProperty]
        public string Number { get; set; }   
        
        public Entry()
        {
            Name = "";
            Number = "";
        }

        public Entry(string name, String number)
        {
            Name = name ?? "";
            Number = number ?? "";
        }
    }
}
