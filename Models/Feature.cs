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

        public Feature(){}

        public Feature(string source, string name, PlayerStat playerStat)
        {
            FeatSource = source;
            FeatureName = name;
        }

        public void BuildFeatureTable(){
            Feature Rage = new Feature(Class, "Rage", playerStat);
            Feature UnDef = new Feature(Class, "Unarmored Defense (Barbarian)", playerStat);
        }
    }
}