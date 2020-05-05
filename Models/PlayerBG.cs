using System;
using System.ComponentModel.DataAnnotations;

namespace CharacterGenerator.Models
{
    public class PlayerBG
    {
        [Key]
        public int PlayerBGId { get; set; }
        public string Background { get; set; }
        public bool Proficiencies { get; set; } = false; 
        public string Feature { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public PlayerBG()
        {

        }
        
        public PlayerStat BGSelector(PlayerStat playerStat, PlayerBG playerBG)
        {
            Random rand = new Random();
            int num = rand.Next(0,16);
            switch(num)
            {
                case 0:
                    playerBG.Background = "Acolyte";
                    playerStat.InsightB = true;
                    playerStat.ReligionB = true;
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "Shelter of the Fainthful";
                    return playerStat;
                case 1:
                    playerBG.Background = "Charlatan";
                    playerStat.DeceptionB = true;
                    playerStat.SleightB = true;
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "False Identity";
                    return playerStat;
                case 2:
                    playerBG.Background = "Criminal / Spy";
                    playerStat.DeceptionB = true;
                    playerStat.StealthB = true;
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "Criminal Contact";
                    return playerStat;
                case 3:
                    playerBG.Background = "Entertainer";
                    playerStat.AcrobaticsB = true;
                    playerStat.PerformanceB = true;
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "By Popular Demand";
                    return playerStat;
                case 4:
                    playerBG.Background = "Folk Hero";
                    playerStat.AnimalHandlingB = true;
                    playerStat.SurvivalB = true;
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "Rustic Hospitality";
                    return playerStat;
                case 5:
                    playerBG.Background = "Gladiator";
                    playerStat.AcrobaticsB = true;
                    playerStat.PerformanceB = true;
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "By Popular Demand";
                    return playerStat;
                case 6:
                    playerBG.Background = "Guild Artisan";
                    playerStat.InsightB = true;
                    playerStat.PersuasionB = true;
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "Guild Membership";
                    return playerStat;
                case 7:
                    playerBG.Background = "Hermit";
                    playerStat.MedicineB = true;
                    playerStat.ReligionB= true;
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "Discovery";
                    return playerStat;
                case 8:
                    playerBG.Background = "Knight";
                    playerStat.HistoryB = true;
                    playerStat.PersuasionB = true;
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "Retainers";
                    return playerStat;
                case 9:
                    playerBG.Background = "Noble";
                    playerStat.HistoryB = true;
                    playerStat.PersuasionB = true;
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "Position of Privilege";
                    return playerStat;
                case 10:
                    playerBG.Background = "Outlander";
                    playerStat.AthleticsB = true;
                    playerStat.SurvivalB = true;
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "Wanderer";
                    return playerStat;
                case 11:
                    playerBG.Background = "Pirate";
                    playerStat.AthleticsB = true;
                    playerStat.PerceptionB = true;
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "Bad Reputation";
                    return playerStat;
                case 12:
                    playerBG.Background = "Sage";
                    playerStat.ArcanaB = true;
                    playerStat.HistoryB = true;
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "Researcher";
                    return playerStat;
                case 13:
                    playerBG.Background = "Sailor";
                    playerStat.AthleticsB = true;
                    playerStat.PerceptionB = true;
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "Ship's Passage";
                    return playerStat;
                case 14:
                    playerBG.Background = "Soldier";
                    playerStat.AthleticsB = true;
                    playerStat.IntimidationB = true;
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "Military Rank";
                    return playerStat;
                case 15:
                    playerBG.Background = "Urchin";
                    playerStat.SleightB = true;
                    playerStat.StealthB = true;
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "City Secrets";
                    return playerStat;
            }
            return playerStat;
        }
    }
}