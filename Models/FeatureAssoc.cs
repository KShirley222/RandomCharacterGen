using System;
using System.ComponentModel.DataAnnotations;

namespace CharacterGenerator.Models
{
    public class FeatureAssoc
    {
        [Key]
        public int FeatureAssocId { get; set; }
        public int FeatureId { get; set; }
        public Feature FeatureA  { get; set; }
        public int CharacterId { get; set; }
        public NewCharacter PlayerA { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    
        public FeatureAssoc(){}
        public FeatureAssoc (NewCharacter character, Feature feat)
        {
            CharacterId = character.CharacterId;
            PlayerA = character;
            FeatureA = feat;
            FeatureId = feat.FeatureId; 
        }
    }
}