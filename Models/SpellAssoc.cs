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
        public NewCharacter PlayerA { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}