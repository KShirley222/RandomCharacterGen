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
        public PlayerClass(int one)
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

        Random rand = new Random();
        public PlayerStat ASIGen( int level, PlayerStat Stats, PlayerClass pClass)
        {
            int Level = level;
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
                if(TargetLevels[i] <= Level)
                {
                    ASILevel.Add(1);
                }
            }
            // for each value pushed into ASILevel, Select random Stat and assign value increase
            foreach(int ASI in ASILevel)
            {
                
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
            pClass.ASI = ASILevel.Count;
            return Stats;
        }

        public bool ProGen( int level, PlayerStat Stats, int numOfPro, int[] skillList)
        {
            // Based on bard because of high count of pros and entire list selection
            // Modify for child class by changing available cases and modify rand.
            
            int Level = level;
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
            
            int total = HitDie + ConMod;
            for (int i = 1; i<level; i++)
                {
                    total += rand.Next(1, HitDie+1) + ConMod;
                }
            return total;
        }
    

    // CLASS SELECTION - PICKS CLASS AND UPDATES PLAYER CLASS AND PLAYER STATS
        public PlayerStat ClassSelector(int level, PlayerStat Stats, PlayerClass playerClass)
        {
            Console.WriteLine("Within Class Selector, checking for Dwarven Toughness bonus");
            Console.WriteLine(Stats.HitPoints);
            int Level = level;
            PlayerClass pClass = playerClass;
            
            switch(rand.Next(0,12))
            {
                case 0:
                    Bard(Level, Stats, pClass);
                    pClass.ClassName = "Bard";
                    return Stats;
                case 1:
                    Cleric(Level, Stats, pClass);
                    pClass.ClassName = "Cleric";
                    return Stats;
                case 2:
                    Barbarian(Level, Stats, pClass);
                    pClass.ClassName = "Barbarian";
                    return Stats;
                case 3:
                    Druid(Level, Stats, pClass);
                    pClass.ClassName = "Druid";
                    return Stats;
                case 4:
                    Fighter(Level, Stats, pClass);
                    pClass.ClassName = "Fighter";
                    return Stats;
                case 5:
                    Monk(Level, Stats, pClass);
                    pClass.ClassName = "Monk";
                    return Stats;
                case 6:
                    Paladin(Level, Stats, pClass);
                    pClass.ClassName = "Paladin";
                    return Stats;
                case 7:
                    Ranger(Level, Stats, pClass);
                    pClass.ClassName = "Ranger";
                    return Stats;
                case 8:
                    Rogue(Level, Stats, pClass);
                    pClass.ClassName = "Rogue";
                    return Stats;
                case 9:
                    Sorcerer(Level, Stats, pClass);
                    pClass.ClassName = "Sorcerer";
                    return Stats;
                case 10:
                    Warlock(Level, Stats, pClass);
                    pClass.ClassName = "Warlock";
                    return Stats;
                case 11:
                    Wizard(Level, Stats, pClass);
                    pClass.ClassName = "Wizard";
                    return Stats;
            }
            Console.WriteLine("Finished with Class Selector");
            return Stats;
        }

        public PlayerStat SpecClassSelector(int level, PlayerStat Stats, PlayerClass playerClass, string selection)
        {
            int Level = level;
            PlayerClass pClass = playerClass;
            
            switch(selection)
            {
                case "Bard":
                    Bard(Level, Stats, pClass);
                    pClass.ClassName = "Bard";
                    return Stats;
                case "Cleric":
                    Cleric(Level, Stats, pClass);
                    pClass.ClassName = "Cleric";
                    return Stats;
                case "Barbarian":
                    Barbarian(Level, Stats, pClass);
                    pClass.ClassName = "Barbarian";
                    return Stats;
                case "Druid":
                    Druid(Level, Stats, pClass);
                    pClass.ClassName = "Druid";
                    return Stats;
                case "Fighter":
                    Fighter(Level, Stats, pClass);
                    pClass.ClassName = "Fighter";
                    return Stats;
                case "Monk":
                    Monk(Level, Stats, pClass);
                    pClass.ClassName = "Monk";
                    return Stats;
                case "Paladin":
                    Paladin(Level, Stats, pClass);
                    pClass.ClassName = "Paladin";
                    return Stats;
                case "Ranger":
                    Ranger(Level, Stats, pClass);
                    pClass.ClassName = "Ranger";
                    return Stats;
                case "Rogue":
                    Rogue(Level, Stats, pClass);
                    pClass.ClassName = "Rogue";
                    return Stats;
                case "Sorcerer":
                    Sorcerer(Level, Stats, pClass);
                    pClass.ClassName = "Sorcerer";
                    return Stats;
                case "Warlock":
                    Warlock(Level, Stats, pClass);
                    pClass.ClassName = "Warlock";
                    return Stats;
                case "Wizard":
                    Wizard(Level, Stats, pClass);
                    pClass.ClassName = "Wizard";
                    return Stats;
            }
            return Stats;
        }

        public PlayerStat Barbarian(int level, PlayerStat Stats, PlayerClass pClass)
        {
            ClassName = "Barbarian";
            int Level = level;
            int NumOfPro = 2;
            // Skills: { "AnimalHandling", "Athletics", "Intimidation", "Nature", "Perception", "Survivial" };
            int[] SkillList = {0, 1, 7, 10, 11, 17}; 
            Proficiencies = ProGen( Level, Stats, NumOfPro, SkillList);
            Stats.StrSaveB = true;
            Stats.ConSaveB = true;
            // ASI assignment
            Stats = ASIGen(Level, Stats, pClass);
            // Hp Generation
            int Base = 12;
            int ConMod = Stats.ConMod;
            ClassHP = HPCalc(Level, Base, ConMod);
            Stats.HitPoints += ClassHP;
            BarbSubGen(pClass, Stats);
            return Stats; 
        }
        public PlayerStat Bard(int level, PlayerStat Stats, PlayerClass pClass) 
        {
            ClassName = "Bard";
            int Level = level;
            int NumOfPro = 3;
            // Skills: Choose two fro{ "Acrobatics", "AnimalHandling", "Arcana", "Athletics", "Deception", "History","Insight","Intimidation","Investigation", "Medicine", "Nature", "Perception", "Performance", "Persuasion", "Religion", "SleightofHand", "Stealth", "Survival"};
            int[] SkillList = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17}; 
            pClass.Proficiencies = ProGen( Level, Stats, NumOfPro, SkillList);
            Stats.DexSaveB = true;
            Stats.ChaSaveB = true;
            // ASI assignment
            Stats = ASIGen(Level, Stats, pClass);
            // Hp Generation
            int Base = 8;
            int ConMod = Stats.ConMod;
            pClass.ClassHP = HPCalc(Level, Base, ConMod);
            Stats.HitPoints += ClassHP;
            BardSubGen(pClass, Stats); 
            return Stats;
        }



        public PlayerStat Cleric(int level, PlayerStat Stats, PlayerClass pClass)
        {
            int Level = level;
            int NumOfPro = 2;
            // Skills:{"History", "Insight", "Medicine", "Religion"};
            int[] SkillList = {4, 6, 7, 9, 14}; 
            Proficiencies = ProGen( Level, Stats, NumOfPro, SkillList);
            // ASI assignment
            Stats = ASIGen(Level, Stats, pClass);
            Stats.WisSaveB = true;
            Stats.ChaSaveB = true;
            // Hp Generation
            int Base = 8;
            int ConMod = Stats.ConMod;
            ClassHP = HPCalc(Level, Base, ConMod);
            Stats.HitPoints += ClassHP;
            ClericSubGen(pClass, Stats);
            return Stats; 
        }

        public PlayerStat Druid(int level, PlayerStat Stats, PlayerClass pClass)
        {
            int Level = level;
            int NumOfPro = 2;
            // Skills: Choose { "AnimalHandling", "Arcana", "Insight", "Medicine", "Nature", "Perception", "Religion", "Survival"};
            int[] SkillList = {0, 5, 6, 9, 10, 11, 14, 17}; 
            Proficiencies = ProGen( Level, Stats, NumOfPro, SkillList);
            // ASI assignment
            Stats = ASIGen(Level, Stats, pClass);
            Stats.IntSaveB = true;
            Stats.WisSaveB = true;
            // Hp Generation
            int Base = 8;
            int ConMod = Stats.ConMod;
            ClassHP = HPCalc(Level, Base, ConMod);
            Stats.HitPoints += ClassHP;
            DruidSubGen(pClass, Stats);
            return Stats; 
        }
        public PlayerStat Fighter(int level, PlayerStat Stats, PlayerClass pClass)
        {
            int Level = level;
            int NumOfPro = 2;
            // Skills: Choose{ "Acrobatics", "AnimalHandling","Athletics","History","Insight","Intimidation","Perception", "Survival"};
            int[] SkillList = {2, 0, 1, 4, 6, 7, 11, 17}; 
            Proficiencies = ProGen( Level, Stats, NumOfPro, SkillList);
            // ASI assignment
            Stats = ASIGen(Level, Stats, pClass);
            Stats.StrSaveB = true;
            Stats.ConSaveB = true;
            // Hp Generation
            int Base = 10;
            int ConMod = Stats.ConMod;
            ClassHP = HPCalc(Level, Base, ConMod);
            Stats.HitPoints += ClassHP;
            FighterSubGen( pClass, Stats);
            return Stats; 
        }
        public PlayerStat Monk(int level, PlayerStat Stats, PlayerClass pClass)
        {
            int Level = level;
            int NumOfPro = 2;
            // Skills: Choose  { "Acrobatics","Athletics","History","Insight","Religion", "SleightofHand", "Stealth"}
            int[] SkillList = {2, 1, 4, 6, 14, 15, 16}; 
            Proficiencies = ProGen( Level, Stats, NumOfPro, SkillList);
            // ASI assignment
            Stats = ASIGen(Level, Stats, pClass);
            Stats.StrSaveB = true;
            Stats.DexSaveB = true;
            // Hp Generation
            int Base = 8;
            int ConMod = Stats.ConMod;
            ClassHP = HPCalc(Level, Base, ConMod);
            Stats.HitPoints += ClassHP;
            MonkSubGen( pClass, Stats);
            return Stats; 
        }
        public PlayerStat Paladin(int level, PlayerStat Stats, PlayerClass pClass)
        {
            int Level = level;
            int NumOfPro = 2;
            // Skills: Choose { "Athletics","Insight","Intimidation","Medicine", "Persuasion", "Religion"};
            int[] SkillList = {1, 6, 7, 9, 13, 14}; 
            Proficiencies = ProGen( Level, Stats, NumOfPro, SkillList);
            // ASI assignment
            Stats = ASIGen(Level, Stats, pClass);
            Stats.WisSaveB = true;
            Stats.ChaSaveB = true;
            // Hp Generation
            int Base = 10;
            int ConMod = Stats.ConMod;
            ClassHP = HPCalc(Level, Base, ConMod);
            Stats.HitPoints += ClassHP;
            PaladinSubGen( pClass, Stats);
            return Stats; 
        }
        public PlayerStat Ranger(int level, PlayerStat Stats, PlayerClass pClass)
        {
            int Level = level;
            int NumOfPro = 3;
            // Skills: Choose {"AnimalHandling","Athletics","Insight","Investigation","Nature", "Perception", "Stealth", "Survival"};
            int[] SkillList = {0, 1, 6, 8, 10, 11, 16, 17}; 
            Proficiencies = ProGen( Level, Stats, NumOfPro, SkillList);
            // ASI assignment
            Stats = ASIGen(Level, Stats, pClass);
            Stats.StrSaveB = true;
            Stats.DexSaveB =true;
            // Hp Generation
            int Base = 10;
            int ConMod = Stats.ConMod;
            ClassHP = HPCalc(Level, Base, ConMod);
            Stats.HitPoints += ClassHP;
            RangerSubGen(pClass, Stats);
            return Stats; 
        }
        public PlayerStat Rogue(int level, PlayerStat Stats, PlayerClass pClass)
        {
            int Level = level;
            int NumOfPro = 4;
            // Skills: Choose { "Acrobatics", "Athletics", "Deception","Insight","Intimidation","Investigation", "Perception", "Performance", "Persuasion", "SleightofHand", "Stealth"};
            int[] SkillList = { 2, 1, 3, 6, 7, 8, 11, 12, 13, 15, 16 }; 
            Proficiencies = ProGen( Level, Stats, NumOfPro, SkillList);
            // ASI assignment
            Stats = ASIGen(Level, Stats, pClass);
            Stats.IntSaveB = true;
            Stats.DexSaveB =true;
            // Hp Generation
            int Base = 8;
            int ConMod = Stats.ConMod;
            ClassHP = HPCalc(Level, Base, ConMod);
            Stats.HitPoints += ClassHP;
            RogueSubGen( pClass, Stats);
            return Stats; 
        }
        public PlayerStat Sorcerer(int level, PlayerStat Stats, PlayerClass pClass)
        {
            int Level = level;
            int NumOfPro = 2;
            // Skills: Choose {"Arcana","Deception","Insight","Intimidation", "Persuasion", "Religion"};
            int[] SkillList = {5, 3, 6, 7, 13, 14}; 
            Proficiencies = ProGen( Level, Stats, NumOfPro, SkillList);
            // ASI assignment
            Stats = ASIGen(Level, Stats, pClass);
            Stats.ConSaveB = true;
            Stats.ChaSaveB =true;
            // Hp Generation
            int Base = 6;
            int ConMod = Stats.ConMod;
            ClassHP = HPCalc(Level, Base, ConMod);
            Stats.HitPoints += ClassHP;
            SorcSubGen( pClass, Stats);
            return Stats; 
        }
        public PlayerStat Warlock(int level, PlayerStat Stats, PlayerClass pClass)
        {
            int Level = level;
            int NumOfPro = 2;
            // Skills: Choose { "Arcana", "Deception", "History", "Intimidation", "Investigation", "Nature",  "Religion"};
            int[] SkillList = { 5, 3, 4, 7, 8, 10, 14}; 
            Proficiencies = ProGen( Level, Stats, NumOfPro, SkillList);
            // ASI assignment
            Stats = ASIGen(Level, Stats, pClass);
            Stats.WisSaveB = true;
            Stats.ChaSaveB =true;
            // Hp Generation
            int Base = 8;
            int ConMod = Stats.ConMod;
            ClassHP = HPCalc(Level, Base, ConMod);
            Stats.HitPoints += ClassHP;
            WarlockSubGen( pClass, Stats);
            return Stats; 
        }
        public PlayerStat Wizard(int level, PlayerStat Stats, PlayerClass pClass)
        {
            int Level = level;
            int NumOfPro = 2;
            // Skills: Choose { "Arcana", "History", "Insight", "Investigation", "Medicine", "Religion"};
            int[] SkillList = { 5, 4, 6, 8, 9, 14}; 
            Proficiencies = ProGen( Level, Stats, NumOfPro, SkillList);
            // ASI assignment
            Stats = ASIGen(Level, Stats, pClass);
            Stats.IntSaveB = true;
            Stats.WisSaveB =true;
            // Hp Generation
            int Base = 6;
            int ConMod = Stats.ConMod;
            ClassHP = HPCalc(Level, Base, ConMod);
            Stats.HitPoints += ClassHP;
            WizSubGen( pClass, Stats );
            return Stats; 
        }
        
        //passing in Playerclass to assign the subclassname, and the PlayerStat to assign features/spells once those are implemented in PlayerClass
        public void BarbSubGen(PlayerClass pc, PlayerStat ps)
        {
            
            int subclassnum = rand.Next(0,1);
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

        public void BardSubGen(PlayerClass pc, PlayerStat ps)
        {
            
            int subclassnum = rand.Next(0,1);
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

        public void ClericSubGen(PlayerClass pc, PlayerStat ps)
        {
            
            int subclassnum = rand.Next(1,2);
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

                    case 3:
                        pc.SubClassName = "Nature Domain";
                        break;

                    case 4: 
                        pc.SubClassName = "Tempest Domain";
                        break;

                    case 5:
                        pc.SubClassName = "Trickery Domain";
                        break;
                    
                    case 6:
                        pc.SubClassName = "War Domain";
                        break;
                }
        }
        public void DruidSubGen(PlayerClass pc, PlayerStat ps)
        {
            
            int subclassnum = 1;
            switch(subclassnum)
                {
                    case 0:
                        pc.SubClassName = "Circle of the Moon";
                        break;

                    case 1:
                        int randruid = rand.Next(0,7);
                        switch(randruid)
                        {
                            case 0:
                                pc.SubClassName = "Circle of the Land: Arctic";
                                break;
                            case 1:
                                pc.SubClassName = "Circle of the Land: Coast";
                                break;
                            case 2:
                                pc.SubClassName = "Circle of the Land: Desert";
                                break;
                            case 3:
                                pc.SubClassName = "Circle of the Land: Forest";
                                break;
                            case 4:
                                pc.SubClassName = "Circle of the Land: Grassland";
                                break;
                            case 5:
                                pc.SubClassName = "Circle of the Land: Mountain";
                                break;
                            case 6:
                                pc.SubClassName = "Circle of the Land: Swamp";
                                break;
                            case 7:
                                pc.SubClassName = "Circle of the Land: Underdark";
                                break;
                        }
                        break;
                }
        }

        public void FighterSubGen(PlayerClass pc,PlayerStat ps)
        {
            
            int subclassnum = rand.Next(0,1);
            switch(subclassnum)
            {
                case 0:
                    pc.SubClassName = "Champion";
                    break;
                case 1:
                    pc.SubClassName = "Battle Master";
                    break;
                case 2:
                    pc.SubClassName = "Eldritch Knight";
                    break;
            }
        }
        public void MonkSubGen (PlayerClass pc, PlayerStat ps)
        {
            
            int subclassnum = rand.Next(0,1);
            switch(subclassnum)
            {
                case 0:
                    pc.SubClassName = "Way of the Open Hand";
                    break;
                case 1:
                    pc.SubClassName = "Way of the Four Elements";
                    break;
                case 2:
                    pc.SubClassName = "Way of Shadow";
                    break;
            }
        }

        public void PaladinSubGen (PlayerClass pc, PlayerStat ps)
        {
            
            int subclassnum = rand.Next(0,1);
            switch(subclassnum)
            {
                case 0:
                    pc.SubClassName = "Oath of Devotion";
                    break;
                case 1:
                    pc.SubClassName = "Oath of Vengeance";
                    break;
                case 2:
                    pc.SubClassName = "Oath of the Ancients";
                    break;
            }
        }

        public void RangerSubGen (PlayerClass pc, PlayerStat ps)
        {
            
            int subclassnum = rand.Next(0,1);
            switch(subclassnum)
            {
                case 0:
                    pc.SubClassName = "Hunter";
                    break;
                case 1:
                    pc.SubClassName = "Beast Master";
                    break;
            }
        }

        public void RogueSubGen (PlayerClass pc, PlayerStat ps)
        {
            
            int subclassnum = rand.Next(0,1);
            switch(subclassnum)
            {
                case 0:
                    pc.SubClassName = "Thief";
                    break;
                case 1:
                    pc.SubClassName = "Assassin";
                    break;
                case 2:
                    pc.SubClassName = "Arcane Trickster";
                    break;
            }
        }

        public void SorcSubGen (PlayerClass pc, PlayerStat ps)
        {
            
            int subclassnum = rand.Next(0,1);
            switch(subclassnum)
            {
                case 0:
                    pc.SubClassName = "Draconic Bloodline";
                    break;
                case 1:
                    pc.SubClassName = "Wild Magic";
                    break;
            }
        }

        public void WarlockSubGen (PlayerClass pc, PlayerStat ps)
        {
            
            int subclassnum = rand.Next(2,3);
            switch(subclassnum)
            {
                case 0:
                    pc.SubClassName = "The Archfey";
                    break;
                case 1:
                    pc.SubClassName = "The Great Old One";
                    break;
                case 2:
                    pc.SubClassName = "The Fiend";
                    break;
            }
        }

        public void WizSubGen (PlayerClass pc, PlayerStat ps)
        {
            
            int subclassnum = rand.Next(4,5);
            switch(subclassnum)
            {
                case 0:
                    pc.SubClassName = "School of Abjuration";
                    break;
                case 1:
                    pc.SubClassName = "School of Conjuration";
                    break;
                case 2:
                    pc.SubClassName = "School of Divination";
                    break;
                case 3:
                    pc.SubClassName = "School of Enchantment";
                    break;
                case 4:
                    pc.SubClassName = "School of Evocation";
                    break;
                case 5:
                    pc.SubClassName = "School of Illusion";
                    break;
                case 6:
                    pc.SubClassName = "School of Necromancy";
                    break;
                case 7:
                    pc.SubClassName = "School of Transmutation";
                    break;
            }
        }
    }
}
    
