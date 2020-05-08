using System;
using System.ComponentModel.DataAnnotations;

namespace CharacterGenerator.Models
{
    public class Feature
    {
        [Key]
        public int FeatureId {get; set;}
        public int PlayerStatId {get; set;}
        public PlayerStat Owner {get; set;}
        public string FeatSource {get; set;}
        public string FeatureName {get; set;}

        public Feature(){}

        public Feature(string source, string name)
        {
            FeatSource = source;
            FeatureName = name;
        }
    }
}