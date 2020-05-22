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
    public class SpellArrayHelperModel
    {
        public string Name {get; set;}
        public int Level {get; set;}
        // public List<string> Classes {get;set;}
    }
}