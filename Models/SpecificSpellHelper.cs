using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CharacterGenerator.Models
{
    public class SpecificSpellHelperModel
    {
        public string Name {get;set;}
        public int Level {get; set;}
        public Array Classes {get; set;}
    }
}