using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CharacterGenerator.Models
{
    public class Spell
    {
        [Key]
        public int SpellId {get; set;}
        public int CharacterId {get; set;}
        public PlayerStat Caster {get; set;}
        public string SpellName {get; set;}
        public string SpellSource {get; set;}
        public int SpellLevel {get; set;}

        public List<SpellAssoc> Players { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Spell(){}
        public Spell(int SPL_LVL, List<string> sources, string name)
        {
            SpellLevel = SPL_LVL;
            SpellName = name;

        }
    }

}