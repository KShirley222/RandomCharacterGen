using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CharacterGenerator.Models
{
    public class PlayerStat
    {
        [Key]
        public int PlayerStatId {get; set;}
        // Base Stats
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma {get; set;}

        // Some other stuff
        public int ArmorClass { get; set; }
        public double Proficiency { get; set; }
        public int HitPoints { get; set; }
        
        // Modifiiers
        public int StrMod { get; set; }
        public int DexMod { get; set; }
        public int ConMod { get; set; }
        public int IntMod { get; set; }
        public int WisMod { get; set; }
        public int ChaMod { get; set; }

        // Skills
        public int Acrobatics { get; set; }
        public int AnimalHandling { get; set; }
        public int Arcana { get; set; }
        public int Athletics { get; set; }
        public int Deception { get; set; }
        public int History { get; set; }
        public int Intimidation { get; set; }
        public int Investigation { get; set; }
        public int Insight { get; set; }
        public int Medicine { get; set; }
        public int Nature { get; set; }
        public int Perception { get; set; }
        public int Performance { get; set; }
        public int Persuasion { get; set; }
        public int Religion { get; set; }
        public int Sleight { get; set; }
        public int Stealth { get; set; }
        public int Survival { get; set; }

        // Skills Bool Check
        public bool AcrobaticsB { get; set; } = false;
        public bool AnimalHandlingB { get; set; } = false;
        public bool ArcanaB { get; set; } = false;
        public bool AthleticsB { get; set; } = false;
        public bool DeceptionB { get; set; } = false;
        public bool HistoryB { get; set; } = false;
        public bool InsightB { get; set; } = false;
        public bool IntimidationB { get; set; } = false;
        public bool InvestigationB { get; set; } = false;
        public bool MedicineB { get; set; } = false;
        public bool NatureB { get; set; } = false;
        public bool PerceptionB { get; set; } = false;
        public bool PerformanceB { get; set; } = false;
        public bool PersuasionB { get; set; } = false;
        public bool ReligionB { get; set; } = false;
        public bool SleightB { get; set; } = false;
        public bool StealthB { get; set; } = false;
        public bool SurvivalB { get; set; } = false;

        //Languages
                //Standard Languages
                public bool LangCommon {get; set;} = true;
                public bool LangDwarvish {get;set;} = false;
                public bool LangElvish {get; set;} = false;
                public bool LangGiant {get; set;} = false;
                public bool LangGnomish {get; set;} = false;
                public bool LangGoblin {get; set;} = false;
                public bool LangHalfling {get; set;} = false;
                public bool LangOrcish {get; set;} = false;
                //Uncommon Languages
                public bool LangAbyssal {get; set;} = false;
                public bool LangCelestial {get; set;} = false;
                public bool LangDraconic {get; set;} = false;
                public bool LangDeepSpeech {get; set;} = false;
                public bool LangInfernal {get; set;} = false;
                public bool LangPrimordial {get; set;} = false;
                public bool LangSylvan {get; set;} = false;
                public bool LangUndercommon {get; set;} = false;

        //Trying to implement Features and Spells here

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        public PlayerStat(){}
        public PlayerStat(int level)
        {
            int Level = level;
            // Base Stats
            int[] stats = { 10, 12, 15, 13, 14, 8};
            int [] shuffleStats = Randomize(stats, 6);
            Strength = shuffleStats[0];
            Dexterity = shuffleStats[1];
            Constitution = shuffleStats[2];
            Intelligence = shuffleStats[3];
            Wisdom = shuffleStats[4];
            Charisma = shuffleStats[5];

            // Modifiers
            //mathabs(num/2)-5
            StrMod = Math.Abs(Strength/2)-5;
            DexMod = Math.Abs(Dexterity/2)-5;
            ConMod = Math.Abs(Constitution/2)-5;
            IntMod = Math.Abs(Intelligence/2)-5;
            WisMod = Math.Abs(Wisdom/2)-5;
            ChaMod = Math.Abs(Charisma/2)-5; 
            // Other Stats
            ArmorClass = 0;
            Proficiency = Math.Ceiling(1+(.25*Level));
            HitPoints = 0; 

            // Skills - bool
            // AcrobaticsB = false; 
            // AnimalHandlingB = false;
            // ArcanaB = false;
            // AthleticsB = false;
            // DeceptionB = false;
            // HistoryB = false;
            // IntimidationB = false; 
            // InsightB = false;
            // InvestigationB = false;
            // MedicineB = false;
            // NatureB = false;
            // PerceptionB = false;
            // PerformanceB = false;
            // PersuasionB = false;
            // ReligionB = false;
            // SleightB = false;
            // StealthB = false;
            // SurvivalB = false;
            
        }

            // SKills int

        public PlayerStat UpdatePro(PlayerStat stat)
        {
            StrMod = Math.Abs(Strength/2)-5;
            DexMod = Math.Abs(Dexterity/2)-5;
            ConMod = Math.Abs(Constitution/2)-5;
            IntMod = Math.Abs(Intelligence/2)-5;
            WisMod = Math.Abs(Wisdom/2)-5;
            ChaMod = Math.Abs(Charisma/2)-5; 

            if(stat.AcrobaticsB == true)
                {
                    stat.Acrobatics = stat.DexMod + (int)stat.Proficiency;
                }
            else
                {
                    stat.Acrobatics = stat.DexMod;
                }
            if(stat.AnimalHandlingB == true)
                {
                    stat.AnimalHandling = stat.WisMod + (int)stat.Proficiency;
                }
            else
                {
                    stat.AnimalHandling = stat.WisMod;
                }
            if(stat.ArcanaB == true)
                {
                    stat.Arcana = stat.IntMod + (int)stat.Proficiency;
                }
            else
                {
                    stat.Arcana = stat.IntMod;
                }
            if(stat.AthleticsB == true)
                {
                    stat.Athletics = stat.StrMod + (int)stat.Proficiency;
                }
            else
                {
                    stat.Athletics = stat.StrMod;
                }
            if(stat.DeceptionB == true)
                {
                stat.Deception = stat.ChaMod + (int)stat.Proficiency;
                }
            else
                {
                stat.Deception = stat.ChaMod;
                }   
            if(stat.HistoryB == true)
                {
                stat.History = stat.IntMod + (int)stat.Proficiency;
                }
            else
                {
                stat.History = stat.IntMod;
                }
            if(stat.IntimidationB == true)
                {
                stat.Intimidation = stat.ChaMod + (int)stat.Proficiency;
                }
            else
                {
                stat.Intimidation = stat.ChaMod;
                }
            if(stat.InsightB == true)
                {
                stat.Insight = stat.WisMod + (int)stat.Proficiency;
                }
            else
                {
                stat.Insight = stat.WisMod;
                } 
            if(stat.InvestigationB == true)
                {
                stat.Investigation = stat.IntMod + (int)stat.Proficiency;
                }
            else
                {
                stat.Investigation = stat.IntMod;
                }
            if(stat.MedicineB == true)
                {
                stat.Medicine = stat.WisMod + (int)stat.Proficiency;
                }
            else
                {
                stat.Medicine = stat.WisMod;
                }
            if(stat.NatureB == true)
                {
                stat.Nature = stat.IntMod + (int)stat.Proficiency;
                }
            else
                {
                stat.Nature = stat.IntMod;
                }
            if(stat.PerceptionB == true)
                {
                stat.Perception = stat.WisMod + (int)stat.Proficiency;
                }
            else
                {
                stat.Perception = stat.WisMod;
                }
            if(stat.PerformanceB == true)
                {
                stat.Performance = stat.ChaMod + (int)stat.Proficiency;
                }
            else
                {
                stat.Performance = stat.ChaMod;
                }
            if(stat.PersuasionB == true)
                {
                stat.Persuasion = stat.ChaMod + (int)stat.Proficiency;
                }
            else
                {
                stat.Persuasion = stat.ChaMod;
                }
            if(stat.ReligionB == true)
                {
                stat.Religion = stat.IntMod + (int)stat.Proficiency;
                }
            else
                {
                stat.Religion = stat.IntMod;
                }
            if(stat.SleightB == true)
                {
                stat.Sleight = stat.DexMod + (int)stat.Proficiency;
                }
            else
                {
                stat.Sleight = stat.DexMod;
                }
            if(stat.StealthB == true)
                {
                stat.Stealth = stat.DexMod + (int)stat.Proficiency;
                }
            else
                {
                stat.Stealth = stat.DexMod;
                }
            if(stat.SurvivalB == true)
                {
                stat.Survival = stat.WisMod + (int)stat.Proficiency;
                }
            else
                {
                stat.Survival = stat.WisMod;
                }
            return stat;
        }

        public static int[] Randomize(int[] arr, int n) 
        { 
        // Creating a object 
        // for Random class 
            Random r = new Random(); 
            
            // Start from the last element and 
            // swap one by one. We don't need to 
            // run for the first element  
            // that's why i > 0 
            for (int i = n - 1; i > 0; i--)  
            { 
                
                // Pick a random index 
                // from 0 to i 
                int j = r.Next(0, i+1); 
                
                // Swap arr[i] with the 
                // element at random index 
                int temp = arr[i]; 
                arr[i] = arr[j]; 
                arr[j] = temp; 
            } 
            // Prints the random array 
        return arr; 
        }
    }
}