using System;
using System.ComponentModel.DataAnnotations;

namespace CharacterGenerator.Models
{
    public class SpellAssoc
    {
        [Key]
        public int SpellAssocId { get; set; }
        public int SpellId { get; set; }
        public Spell SpellA  { get; set; }
        public int CharacterId { get; set; }
        public bool Prepped { get; set; }
        public bool AlwaysPrepped { get; set; }
        public NewCharacter PlayerA { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public SpellAssoc()
        {
            
        }
        public SpellAssoc (NewCharacter character, Spell spell)
        {
            CharacterId = character.CharacterId;
            PlayerA = character;
            SpellA = spell;
            SpellId = spell.SpellId;
            Prepped = false;
            AlwaysPrepped = false;
        }
    }
}