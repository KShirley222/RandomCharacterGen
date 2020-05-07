using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CharacterGenerator.Models
{
    public class PlayerClass
    {
        [Key]
        public int PlayerClassId { get; set; } 
        public string ClassName { get; set; }
        // Placeholder, may be useful for more purposeful generation
        public string SubClassName {get; set;}
        public int ASI { get; set; }
        // simply tracks where the function has run to post pproficiencies bools. True will represent proficencies accounted for and the int will track the value
        public bool Proficiencies { get; set; }
        public int ClassHP { get; set; }
        public int ClassLevel {get; set;}
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public PlayerClass()
        {
            
        }
        public PlayerClass(int level, PlayerStat stats)
        {
            // Class Selector will select and determine class, passing it to a class function 
            // class function will be child of this class
            // HP should be calculated within the class
            int Level = level;

            // Generate these numbers and update stat sheet.
            // Function needed to gernate ASI result should include pushing ASI to PlayerStats in random selection per each level;
            // ASI = ASIGen(Level, Stats);
            ASI = 0;

            // Proficiensies should be true and the selected profic should be true in PlayerStat;
            // Proficiencies = ProGen(Level, Stats);
            Proficiencies = false;

            // Generate Hp and update PlayerStat, happen in each Player class;
            ClassHP = 0;
        }


        public int ASIGen( int level, PlayerStat stats, PlayerClass pClass)
        {
            int Level = level;
            PlayerStat Stats = stats;
            // Levels player earns +2 so a certain skill
            // This works for every class except Fighter and Rogue
            // Fighter = {4,6,8,12,14,16,19}
            // Rogue = {4,8,10,12,16,19}
            // Override in ClassSelector
            List<int> TargetLevels = new List<int>();
            if(pClass.ClassName == "Fighter")
            {
                TargetLevels.AddRange( new int[]{4,4,6,6,8,8,10,10,12,12,14,14,16,16,19,19});
            }
            else if(pClass.ClassName == "Rogue")
            {
                TargetLevels.AddRange(new int[]{4,4,8,8,10,10,12,12,16,16,19,19});
            }
            else
            {
                TargetLevels.AddRange(new int[]{4,4,8,8,12,12,16,16,19,19});
            }

            Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            foreach(int val in TargetLevels)
            {
                Console.WriteLine(val);
            }
            // Empty bracket to be filled by comparing level to Target levels
            int ASITotal = 0;
            List<int> ASILevel = new List<int>();

            for(int i =0; i < TargetLevels.Count; i++)
            {
                ;
                if(TargetLevels[i] < Level)
                {
                    ASILevel.Add(1);
                }
            }
            // for each value pushed into ASILevel, Select random Stat and assign value increase
            foreach(int ASI in ASILevel)
            {
                Random rand = new Random();
                int skill = rand.Next(0,6);
                switch (skill)
                {
                    case 0:
                        if (Stats.Strength < 20)
                        {
                        Stats.Strength = Stats.Strength + ASI;
                        ASITotal = ASITotal + ASI;
                        break;
                        }
                        else
                            {
                                goto case 1;
                            }
                    case 1:
                        if (Stats.Dexterity < 20)
                        {
                        Stats.Dexterity = Stats.Dexterity + ASI;
                        ASITotal = ASITotal + ASI;
                        break;
                        }
                        else
                            {
                                goto case 2;
                            }
                    case 2:
                        if (Stats.Intelligence < 20)
                            {
                            Stats.Intelligence = Stats.Intelligence + ASI;
                            ASITotal = ASITotal + ASI;
                            break;
                            }
                            else
                                {
                                    goto case 3;
                                }
                    case 3:
                            if (Stats.Charisma < 20)
                            {
                                Stats.Charisma = Stats.Charisma + ASI;
                                ASITotal = ASITotal + ASI;
                                break;
                            }
                            else
                                {
                                    goto case 4;
                                }
                    case 4:
                        if (Stats.Constitution < 20)
                        {
                            Stats.Constitution = Stats.Constitution + ASI;
                            ASITotal = ASITotal + ASI;
                            break;
                        }
                        else
                            {
                                goto case 5;
                            }
                    case 5:
                        if (Stats.Wisdom < 20)
                            {
                            Stats.Wisdom = Stats.Wisdom + ASI;
                            ASITotal = ASITotal + ASI;
                            break;                                
                            }
                        else
                            {
                                goto case 0;
                            }
                }
            }
            return ASITotal;
        }

        public bool ProGen( int level, PlayerStat stats, int numOfPro, int[] skillList)
        {
            // Based on bard because of high count of pros and entire list selection
            // Modify for child class by changing available cases and modify rand.
            Random rand = new Random();
            int Level = level;
            PlayerStat Stats = stats;
            int NumOfPro = numOfPro;
            int[] SkillList = skillList;

            for(int i =0; i<NumOfPro; i++)
            {
                // Select will represent the number that is used to pic the skill that is proficient
                // Second number represents total number of cases
                // Will add 1 to NumOfPro and break if already true
                int num = rand.Next(0,SkillList.Length);
                int Select = SkillList[num];
                // pass in list for loop into switch 
                switch(Select)
                {
                    case 0:
                        if(Stats.AnimalHandlingB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        Stats.AnimalHandlingB = true;
                        break;

                    case 1:
                        if(Stats.AthleticsB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        Stats.AthleticsB = true;
                        break;

                    case 2:
                        if(Stats.AcrobaticsB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        Stats.AcrobaticsB = true;
                        break;

                    case 3:
                        if(Stats.DeceptionB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        Stats.DeceptionB = true;
                        break;

                    case 4:
                        if(Stats.HistoryB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        Stats.HistoryB = true;
                        break;

                    case 5:
                        if(Stats.ArcanaB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        Stats.ArcanaB = true;
                        break;

                    case 6:
                        if(Stats.InsightB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        Stats.InsightB = true;
                        break;

                    case 7:
                        if(Stats.IntimidationB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        Stats.IntimidationB = true;
                        break;

                    case 8:
                        if(Stats.InvestigationB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        Stats.InvestigationB = true;
                        break;

                    case 9:
                        if(Stats.MedicineB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        Stats.MedicineB = true;
                        break;

                    case 10:
                        if(Stats.NatureB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        Stats.NatureB = true;
                        break;

                    case 11:
                        if(Stats.PerceptionB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        Stats.PerceptionB = true;
                        break;

                    case 12:
                        if(Stats.PerformanceB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        Stats.PerformanceB = true;
                        break;

                    case 13:
                        if(Stats.PersuasionB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        Stats.PersuasionB = true;
                        break;

                    case 14:
                        if(Stats.ReligionB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        Stats.ReligionB = true;
                        break;

                    case 15:
                        if(Stats.SleightB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        Stats.SleightB = true;
                        break;

                    case 16:
                        if(Stats.StealthB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        Stats.StealthB = true;
                        break;

                    case 17:
                        if(Stats.SurvivalB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        Stats.SurvivalB = true;
                        break;
                }
            }
            return true;
        }

        public int HPCalc(int level, int HitDie, int ConMod)
        {
            Random rand = new Random();
            int total = HitDie + ConMod;
            for (int i = 1; i<level; i++)
                {
                    total += rand.Next(1, HitDie+1) + ConMod;
                }
            return total;
        }
    

    // CLASS SELECTION - PICKS CLASS AND UPDATES PLAYER CLASS AND PLAYER STATS
    //  TODO - Write the reamaining calls inside class selector
    // TODO - write the remaining funtcions for each class
    // TODO - organize SkillList for each class to reflect correct 
        public PlayerStat ClassSelector(int level, PlayerStat stats, PlayerClass playerClass)
        {
            int Level = level;
            PlayerStat Stats = stats;
            PlayerClass pClass = playerClass;
            Random rand = new Random();
            switch(rand.Next(0,3))
            {
                case 0:
                    Bard(Level, Stats, pClass);
                    string Name = "Bard";
                    pClass.ClassName = Name;
                    return Stats;
                case 1:
                    Cleric(Level, Stats, pClass);
                    Name = "Cleric";
                    pClass.ClassName = "Cleric";
                    return Stats;
                case 2:
                    Barbarian(Level, Stats, pClass);
                    Name = "Barbarian";
                    pClass.ClassName = "Barbarian";

                    return Stats;
                case 3:
                    Druid(Level, Stats, pClass);
                    Name = "Druid";
                    pClass.ClassName = "Barbarian";
                    return Stats;
                
            }
            return Stats;
        }

        public PlayerStat Barbarian(int level, PlayerStat stats, PlayerClass pClass)
        {
            ClassName = "Barbarian";
            int Level = level;
            PlayerStat Stats = stats;
            int NumOfPro = 2;
            // Skills: { "AnimalHandling", "Athletics", "Intimidation", "Nature", "Perception", "Survivial" };
            int[] SkillList = {0, 1, 7, 10, 11, 17}; 
            Proficiencies = ProGen( Level, Stats, NumOfPro, SkillList);
            // ASI assignment
            ASI = ASIGen(Level, Stats, pClass);
            // Hp Generation
            int Base = 12;
            int ConMod = Stats.ConMod;
            ClassHP = HPCalc(Level, Base, ConMod);
            Stats.HitPoints = Stats.HitPoints + ClassHP;
            return Stats; 
        }
        public PlayerStat Bard(int level, PlayerStat stats, PlayerClass pClass) 
        {
            ClassName = "Bard";
            int Level = level;
            PlayerStat Stats = stats;
            int NumOfPro = 3;
            // Skills: Choose two fro{ "Acrobatics", "AnimalHandling", "Arcana", "Athletics", "Deception", "History","Insight","Intimidation","Investigation", "Medicine", "Nature", "Perception", "Performance", "Persuasion", "Religion", "SleightofHand", "Stealth", "Survival"};
            int[] SkillList = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17}; 
            pClass.Proficiencies = ProGen( Level, Stats, NumOfPro, SkillList);
            // ASI assignment
            pClass.ASI = ASIGen(Level, Stats, pClass);
            // Hp Generation
            int Base = 8;
            int ConMod = Stats.ConMod;
            pClass.ClassHP = HPCalc(Level, Base, ConMod);
            Stats.HitPoints = Stats.HitPoints + ClassHP; 
            return Stats;
        }



        public PlayerStat Cleric(int level, PlayerStat stats, PlayerClass pClass)
        {
            int Level = level;
            PlayerStat Stats = stats;
            int NumOfPro = 2;
            // Skills:{"History", "Insight", "Medicine", "Religion"};
            int[] SkillList = {4, 6, 7, 9, 14}; 
            Proficiencies = ProGen( Level, Stats, NumOfPro, SkillList);
            // ASI assignment
            ASI = ASIGen(Level, Stats, pClass);
            // Hp Generation
            int Base = 8;
            int ConMod = Stats.ConMod;
            ClassHP = HPCalc(Level, Base, ConMod);
            Stats.HitPoints = Stats.HitPoints + ClassHP;
            return Stats; 
        }

        public PlayerStat Druid(int level, PlayerStat stats, PlayerClass pClass)
        {
            int Level = level;
            PlayerStat Stats = stats;
            int NumOfPro = 2;
            // Skills: Choose { "AnimalHandling", "Arcana", "Insight", "Medicine", "Nature", "Perception", "Religion", "Survival"};
            int[] SkillList = {0, 5, 6, 9, 10, 11, 14, 17}; 
            Proficiencies = ProGen( Level, Stats, NumOfPro, SkillList);
            // ASI assignment
            ASI = ASIGen(Level, Stats, pClass);
            // Hp Generation
            int Base = 8;
            int ConMod = Stats.ConMod;
            ClassHP = HPCalc(Level, Base, ConMod);
            Stats.HitPoints = Stats.HitPoints + ClassHP;
            return Stats; 
        }
        public PlayerStat Fighter(int level, PlayerStat stats, PlayerClass pClass)
        {
            int Level = level;
            PlayerStat Stats = stats;
            int NumOfPro = 2;
            // Skills: Choose{ "Acrobatics", "AnimalHandling","Athletics","History","Insight","Intimidation","Perception", "Survival"};
            int[] SkillList = {2, 0, 1, 4, 6, 7, 11, 17}; 
            Proficiencies = ProGen( Level, Stats, NumOfPro, SkillList);
            // ASI assignment
            ASI = ASIGen(Level, Stats, pClass);
            // Hp Generation
            int Base = 10;
            int ConMod = Stats.ConMod;
            ClassHP = HPCalc(Level, Base, ConMod);
            Stats.HitPoints = Stats.HitPoints + ClassHP;
            return Stats; 
        }
        public PlayerStat Monk(int level, PlayerStat stats, PlayerClass pClass)
        {
            int Level = level;
            PlayerStat Stats = stats;
            int NumOfPro = 2;
            // Skills: Choose  { "Acrobatics","Athletics","History","Insight","Religion", "SleightofHand", "Stealth"}
            int[] SkillList = {2, 1, 4, 6, 14, 15, 16}; 
            Proficiencies = ProGen( Level, Stats, NumOfPro, SkillList);
            // ASI assignment
            ASI = ASIGen(Level, Stats, pClass);
            // Hp Generation
            int Base = 8;
            int ConMod = Stats.ConMod;
            ClassHP = HPCalc(Level, Base, ConMod);
            Stats.HitPoints = Stats.HitPoints + ClassHP;
            return Stats; 
        }
        public PlayerStat Paladin(int level, PlayerStat stats, PlayerClass pClass)
        {
            int Level = level;
            PlayerStat Stats = stats;
            int NumOfPro = 2;
            // Skills: Choose { "Athletics","Insight","Intimidation","Medicine", "Persuasion", "Religion"};
            int[] SkillList = {1, 6, 7, 9, 13, 14}; 
            Proficiencies = ProGen( Level, Stats, NumOfPro, SkillList);
            // ASI assignment
            ASI = ASIGen(Level, Stats, pClass);
            // Hp Generation
            int Base = 10;
            int ConMod = Stats.ConMod;
            ClassHP = HPCalc(Level, Base, ConMod);
            Stats.HitPoints = Stats.HitPoints + ClassHP;
            return Stats; 
        }
        public PlayerStat Ranger(int level, PlayerStat stats, PlayerClass pClass)
        {
            int Level = level;
            PlayerStat Stats = stats;
            int NumOfPro = 3;
            // Skills: Choose {"AnimalHandling","Athletics","Insight","Investigation","Nature", "Perception", "Stealth", "Survival"};
            int[] SkillList = {0, 1, 6, 8, 10, 11, 16, 17}; 
            Proficiencies = ProGen( Level, Stats, NumOfPro, SkillList);
            // ASI assignment
            ASI = ASIGen(Level, Stats, pClass);
            // Hp Generation
            int Base = 10;
            int ConMod = Stats.ConMod;
            ClassHP = HPCalc(Level, Base, ConMod);
            Stats.HitPoints = Stats.HitPoints + ClassHP;
            return Stats; 
        }
        public PlayerStat Rogue(int level, PlayerStat stats, PlayerClass pClass)
        {
            int Level = level;
            PlayerStat Stats = stats;
            int NumOfPro = 4;
            // Skills: Choose { "Acrobatics", "Athletics", "Deception","Insight","Intimidation","Investigation", "Perception", "Performance", "Persuasion", "SleightofHand", "Stealth"};
            int[] SkillList = { 2, 1, 3, 6, 7, 8, 11, 12, 13, 15, 16 }; 
            Proficiencies = ProGen( Level, Stats, NumOfPro, SkillList);
            // ASI assignment
            ASI = ASIGen(Level, Stats, pClass);
            // Hp Generation
            int Base = 8;
            int ConMod = Stats.ConMod;
            ClassHP = HPCalc(Level, Base, ConMod);
            Stats.HitPoints = Stats.HitPoints + ClassHP;
            return Stats; 
        }
        public PlayerStat Sorcerer(int level, PlayerStat stats, PlayerClass pClass)
        {
            int Level = level;
            PlayerStat Stats = stats;
            int NumOfPro = 2;
            // Skills: Choose {"Arcana","Deception","Insight","Intimidation", "Persuasion", "Religion"};
            int[] SkillList = {5, 3, 6, 7, 13, 14}; 
            Proficiencies = ProGen( Level, Stats, NumOfPro, SkillList);
            // ASI assignment
            ASI = ASIGen(Level, Stats, pClass);
            // Hp Generation
            int Base = 6;
            int ConMod = Stats.ConMod;
            ClassHP = HPCalc(Level, Base, ConMod);
            Stats.HitPoints = Stats.HitPoints + ClassHP;
            return Stats; 
        }
        public PlayerStat Warlock(int level, PlayerStat stats, PlayerClass pClass)
        {
            int Level = level;
            PlayerStat Stats = stats;
            int NumOfPro = 2;
            // Skills: Choose { "Arcana", "Deception", "History", "Intimidation", "Investigation", "Nature",  "Religion"};
            int[] SkillList = { 5, 3, 4, 7, 8, 10, 14}; 
            Proficiencies = ProGen( Level, Stats, NumOfPro, SkillList);
            // ASI assignment
            ASI = ASIGen(Level, Stats, pClass);
            // Hp Generation
            int Base = 8;
            int ConMod = Stats.ConMod;
            ClassHP = HPCalc(Level, Base, ConMod);
            Stats.HitPoints = Stats.HitPoints + ClassHP;
            return Stats; 
        }
        public PlayerStat Wizard(int level, PlayerStat stats, PlayerClass pClass)
        {
            int Level = level;
            PlayerStat Stats = stats;
            int NumOfPro = 2;
            // Skills: Choose { "Arcana", "History", "Insight", "Investigation", "Medicine", "Religion"};
            int[] SkillList = { 5, 4, 6, 8, 9, 14}; 
            Proficiencies = ProGen( Level, Stats, NumOfPro, SkillList);
            // ASI assignment
            ASI = ASIGen(Level, Stats, pClass);
            // Hp Generation
            int Base = 6;
            int ConMod = Stats.ConMod;
            ClassHP = HPCalc(Level, Base, ConMod);
            Stats.HitPoints = Stats.HitPoints + ClassHP;
            return Stats; 
        }
        

        public static void BarbSubGen(PlayerClass pc)
        {
            Random rand = new Random();
            int subclassnum = rand.Next(0,2);
            switch(subclassnum)
                {
                    case 0:
                        pc.SubClassName = "Path of the Berserker";
                        break;

                    case 1:
                        pc.SubClassName = "Path of the Totem Warrior";
                        break;
                }
        }

        public static void BardSubGen(PlayerClass pc)
        {
            Random rand = new Random();
            int subclassnum = rand.Next(0,2);
            switch(subclassnum)
                {
                    case 0:
                        pc.SubClassName = "College of Lore";
                        break;

                    case 1:
                        pc.SubClassName = "College of Valor";
                        break;
                }
        }

        public static void ClericSubGen(PlayerClass pc)
        {
            Random rand = new Random();
            int subclassnum = rand.Next(0,7);
            //KnowledgePHB LifePHB LightPHB NaturePHB TempestPHB TrickeryPHB WarPHB
            switch(subclassnum)
                {
                    case 0:
                        pc.SubClassName = "Knowledge Domain";
                        break;

                    case 1:
                        pc.SubClassName = "Life Domain";
                        break;

                    case 2:
                        pc.SubClassName = "Light Domain";
                        break;
                }
        }
    }
}
    
