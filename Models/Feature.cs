using System;
using System.ComponentModel.DataAnnotations;

namespace CharacterGenerator.Models
{
    public class Feature
    {
        [Key]
        public int FeatureId {get; set;}
        public int CharacterId {get; set;}
        public NewCharacter Owner {get; set;}
        public string FeatSource {get; set;}
        public string FeatureName {get; set;}
        public int FeatLevel {get; set;}

        public Feature(){}

        public Feature(string source, string name, int level_obtained)
        {
            FeatSource = source;
            FeatureName = name;
            FeatLevel = level_obtained;
        }
    }
}