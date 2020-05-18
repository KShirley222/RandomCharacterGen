using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

// *
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CharacterGenerator.Models
{
    public class SpellArrayHelper
    {
        public string Index {get; set;}
        public string Name {get; set;}
        public string url {get; set;}

    // public void spellJson( object spell)
    // {
    //     SpellArrayHelper myJsonObject = JsonConvert.DeserializeObject<SpellArrayHelper>((string)spell);
    //     Console.WriteLine(myJsonObject);
    // }
    }
}