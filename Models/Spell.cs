using System;
using System.ComponentModel.DataAnnotations;

namespace CharacterGenerator.Models
{
    public class Spell
    {
        [Key]
        public int SpellId {get; set;}
        public int PlayerStatId {get; set;}
        public PlayerStat Caster {get; set;}
        public string SpellName {get; set;}
        public string SpellSource {get; set;}
        public int SpellLevel {get; set;}
    }

}