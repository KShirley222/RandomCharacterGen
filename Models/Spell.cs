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
        public string Source1 {get; set;}
        public string Source2 {get; set;}
        public string Source3 {get; set;}
        public string Source4 {get; set;}
        public string Source5 {get;set;}
        public string Source6 {get;set;}
        public string Source7 {get;set;}

        public int SpellLevel {get; set;}

        public List<SpellAssoc> Players { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Spell(){}
        public Spell(int SPL_LVL, string name, string source1)
        {
            SpellLevel = SPL_LVL;
            SpellName = name;
            Source1 = source1;
        }
        public Spell(int SPL_LVL, string name, string source1, string source2)
        {
            SpellLevel = SPL_LVL;
            SpellName = name;
            Source1 = source1;
            Source2 = source2;
        }
        public Spell(int SPL_LVL, string name, string source1, string source2, string source3)
        {
            SpellLevel = SPL_LVL;
            SpellName = name;
            Source1 = source1;
            Source2 = source2;
            Source3 = source3;
        }
        public Spell(int SPL_LVL, string name, string source1, string source2, string source3, string source4)
        {
            SpellLevel = SPL_LVL;
            SpellName = name;
            Source1 = source1;
            Source2 = source2;
            Source3 = source3;
            Source4 = source4;
        }
        public Spell(int SPL_LVL, string name, string source1, string source2, string source3, string source4, string source5)
        {
            SpellLevel = SPL_LVL;
            SpellName = name;
            Source1 = source1;
            Source2 = source2;
            Source3 = source3;
            Source4 = source4;
            Source5 = source5;
        }
        public Spell(int SPL_LVL, string name, string source1, string source2, string source3, string source4, string source5, string source6)
        {
            SpellLevel = SPL_LVL;
            SpellName = name;
            Source1 = source1;
            Source2 = source2;
            Source3 = source3;
            Source4 = source4;
            Source5 = source5;
            Source6 = source6;
        }
        public Spell(int SPL_LVL, string name, string source1, string source2, string source3, string source4, string source5, string source6, string source7)
        {
            SpellLevel = SPL_LVL;
            SpellName = name;
            Source1 = source1;
            Source2 = source2;
            Source3 = source3;
            Source4 = source4;
            Source5 = source5;
            Source6 = source6;
            Source7 = source7;
        }
    }

}