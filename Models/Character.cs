using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CharacterGenerator.Models
{
    public class Character
    {
        // Base Inputs and character stats
        // Maybe have a stat object that calculates all proficencies and saves after bonuses for level and inputs.
        // Included Level as a seperate item to allow it to be used in other calculations 
        [Key]
        public int CharID {get; set;}
        public string PlayerClass { get; set; }
        public string PlayerRace { get; set; }
        public string PlayerBG { get; set; }
        // public string PlayerStats { get; set; }
        public int Level { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma {get; set;}

        // Skill modifiers
        public int Acrobatics { get; set; }
        public int AnimalHandling { get; set; }
        public int Arcana { get; set; }
        public int Athletics { get; set; }
        public int Deception { get; set; }
        public int History { get; set; }
        public int Insight { get; set; }
        public int Intimidation { get; set; }
        public int Investigation { get; set; }
        public int Medicine { get; set; }
        public int Nature { get; set; }
        public int Perception { get; set; }
        public int Performance { get; set; }
        public int Persuasion { get; set; }
        public int Religion { get; set; }
        public int SleightofHand { get; set; }
        public int Stealth { get; set; }
        public int Survival { get; set; }
        public int ASI { get; set; }

        //Need to add in Modifiers based on stats

        public int StrMod {get{
            return ModFunction(Strength);
        }}
        public int DexMod {get{
            return ModFunction(Dexterity);
            
        }}
        public int ConMod {get{
            return ModFunction(Constitution);
        }}
        public int IntMod {get{
            return ModFunction(Intelligence);
        }}
        public int WisMod {get{
            return ModFunction(Wisdom);
        }}
        public int ChaMod {get{
            return ModFunction(Charisma);
        }}

        //Secondary Constructor Items

        public int HitPoints {get;set;}
        public int ProficiencyBonus {get;set;}
        // public List<string> Features {get; set;}
        // public List<string> SpellList {get;set;}
        // public int Lvl1SpellSlots {get; set;}
        // public int Lvl2SpellSlots {get; set;}
        // public int Lvl3SpellSlots {get; set;}
        // public int Lvl4SpellSlots {get; set;}
        // public int Lvl5SpellSlots {get; set;}
        // public int Lvl6SpellSlots {get; set;}
        // public int Lvl7SpellSlots {get; set;}
        // public int Lvl8SpellSlots {get; set;}
        // public int Lvl9SpellSlots {get; set;}
        // // public List<string> Skills {get; set;}
        // // public List<string> Saves {get; set;}
        // public int SpellSaveDC {get; set;}
        // public int SpellsKnown {get; set;}
        // public string SpellLvlKnown {get; set;}
        // public string Subclass {get; set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        //Base Constructor builds the Character.
        //We can create (a) secondary constructor(s) that passes in the character and fills in additional information such as spells, spell slots, and more. 
        public Character(int level)
        {
            Level = level;
            HitPoints = 0;
            PlayerClass = FunctionPlayerClass();
            PlayerRace = FunctionPlayerRace();
            PlayerBG = FunctionPlayerBackground();
            ProficiencyBonus = ProficiencyBonusGen(level);
            int[] stats = { 10, 12, 15, 13, 14, 8};
            int [] shuffleStats = Randomize(stats, 6);
            Strength = shuffleStats[0];
            Dexterity = shuffleStats[1];
            Constitution = shuffleStats[2];
            Intelligence = shuffleStats[3];
            Wisdom = shuffleStats[4];
            Charisma = shuffleStats[5];
            Acrobatics = 0;
            AnimalHandling = 0;
            Arcana = 0;
            Athletics = 0;
            Deception = 0;
            History = 0;
            Intimidation = 0;
            Investigation = 0;
            Medicine = 0;
            Nature = 0;
            Perception = 0;
            Performance = 0;
            Persuasion = 0;
            Religion = 0;
            SleightofHand = 0;
            Stealth = 0;
            Survival = 0;
            
        }
        public Character(int level, string playerclass, string race)
        {
            Level = level;
            HitPoints = 0;
            PlayerClass = playerclass;
            PlayerRace = race;
            PlayerBG = FunctionPlayerBackground();
            ProficiencyBonus = ProficiencyBonusGen(level);
            int[] stats = { 10, 12, 15, 13, 14, 8};
            int [] shuffleStats = Randomize(stats, 6);
            Strength = shuffleStats[0];
            Dexterity = shuffleStats[1];
            Constitution = shuffleStats[2];
            Intelligence = shuffleStats[3];
            Wisdom = shuffleStats[4];
            Charisma = shuffleStats[5];
            Acrobatics = 0;
            AnimalHandling = 0;
            Arcana = 0;
            Athletics = 0;
            Deception = 0;
            History = 0;
            Intimidation = 0;
            Investigation = 0;
            Medicine = 0;
            Nature = 0;
            Perception = 0;
            Performance = 0;
            Persuasion = 0;
            Religion = 0;
            SleightofHand = 0;
            Stealth = 0;
            Survival = 0;
            ASI = 0;
        }

        public string FunctionPlayerClass()
        {
            
            Random rand = new Random();
            int randValue = rand.Next(0,12);
            string[] classlist = {
                "Barbarian",
                "Bard",
                "Cleric",
                "Druid",
                "Fighter",
                "Monk",
                "Paladin",
                "Ranger",
                "Rogue",
                "Sorcerer",
                "Warlock",
                "Wizard"
            };
            string curClass = classlist[randValue];
            // return string random from array
            return curClass;
        }
        
        public string FunctionPlayerRace()
        {
            Random rand = new Random();
            int randValue = rand.Next(0,9);
            string[] RaceList = 
            {
                "Dragonborn",
                "Dwarf",
                "Elf",
                "Gnome",
                "Half-Elf",
                "Halfling",
                "Half-Orc",
                "Human", 
                "Tiefling"    
            };
            string curRace = RaceList[randValue];
            return curRace;
        }

        public int ModFunction(int mod)
        {
            int Modifer = Math.Abs((mod-10)/2);
            return Modifer;
        }

        //Thank you GFG, this should help out immensely.
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
        public static string[] RandomizeString(string[] arr, int n) 
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
                string temp = arr[i]; 
                arr[i] = arr[j]; 
                arr[j] = temp; 
            } 
            
            // Prints the random array 
        return arr;
        }

        public string FunctionPlayerBackground()
        {
            Random rand = new Random();
            int randValue = rand.Next(0,18);
            string[] BGList = 
            {
                "Acolyte",
                "Charlatan",
                "Criminal",
                "Spy",
                "Entertainer",
                "Folk Hero",
                "Gladiator",
                "Guild Artisan",
                "Guild Merchant",
                "Hermit",
                "Knight",
                "Noble",
                "Outlander",
                "Pirate",
                "Sage",
                "Sailor",
                "Soldier",
                "Urchin"
            };
            string curBG = BGList[randValue];
            return curBG;
        }

        //The following Secondary Functions should help us construct the character past the initial stage.
        public void SecondaryGeneration(Character newchar)
        {
            if (newchar.PlayerClass == "Barbarian")
            {
                BarbGen(newchar);
            }
            else if (newchar.PlayerClass == "Bard")
            {
                BardGen(newchar);
            }
            else if (newchar.PlayerClass == "Cleric")
            {
                ClericGen(newchar);
            }
            else if (newchar.PlayerClass == "Druid")
            {
                DruidGen(newchar);
            }
            else if (newchar.PlayerClass == "Fighter")
            {
                FighterGen(newchar);
            }
            else if (newchar.PlayerClass == "Monk")
            {
                MonkGen(newchar);
            }
            else if (newchar.PlayerClass == "Paladin")
            {
                PaladinGen(newchar);
            }
            else if (newchar.PlayerClass == "Ranger")
            {
                RangerGen(newchar);
            }
            else if (newchar.PlayerClass == "Rogue")
            {
                RogueGen(newchar);
            }
            else if (newchar.PlayerClass == "Sorcerer")
            {
                SorcGen(newchar);
            }
            else if (newchar.PlayerClass == "Warlock")
            {
                WarlockGen(newchar);
            }
            else if (newchar.PlayerClass == "Wizard")
            {
                WizardGen(newchar);
            }
        }
        
        public void SkillsGen(Character newchar, string[] proficiences, int lengthofpro, int numberofpros )
        {
            string[] randompro = RandomizeString(proficiences, lengthofpro);
            for (int i = 0; i<numberofpros; i++)
                {
                    if(randompro[i] == "Acrobatics")
                        {
                            newchar.Acrobatics = ProficiencyBonus;
                        }
                    else if(randompro[i] == "AnimalHandling")
                        {
                            newchar.AnimalHandling = ProficiencyBonus;
                        }
                    else if(randompro[i] == "Arcana")
                        {
                            newchar.Arcana = ProficiencyBonus;
                        }
                    else if(randompro[i] == "Athletics")
                        {
                            newchar.Athletics = ProficiencyBonus;
                        }
                    else if(randompro[i] == "Deception")
                        {
                            newchar.Deception = ProficiencyBonus;
                        }
                     else if(randompro[i] == "History")
                        {
                            newchar.History = ProficiencyBonus;
                        }
                    else if(randompro[i] == "Insight")
                        {
                            newchar.Insight = ProficiencyBonus;
                        }
                     else if(randompro[i] == "Intimidation")
                        {
                            newchar.Intimidation = ProficiencyBonus;
                        }
                    else if(randompro[i] == "Medicine")
                        {
                            newchar.Medicine = ProficiencyBonus;
                        }
                    else if(randompro[i] == "Nature")
                        {
                            newchar.Nature = ProficiencyBonus;
                        }
                    else if(randompro[i] == "Perception")
                        {
                            newchar.Perception = ProficiencyBonus;
                        }
                    else if(randompro[i] == "Performance")
                        {
                            newchar.Performance = ProficiencyBonus;
                        }
                    else if(randompro[i] == "Persuasion")
                        {
                            newchar.Persuasion = ProficiencyBonus;
                        }
                    else if(randompro[i] == "Religion")
                        {
                            newchar.Religion = ProficiencyBonus;
                        }
                    else if(randompro[i] == "SleightofHand")
                        {
                            newchar.SleightofHand = ProficiencyBonus;
                        }
                    else if(randompro[i] == "Stealth")
                        {
                            newchar.Stealth = ProficiencyBonus;
                        }
                    else if(randompro[i] == "Survival")
                        {
                            newchar.Survival = ProficiencyBonus;
                        }
                    }
        }
        //From there, we can fill in based on character level, IE if (newchar.Level == #Number){set attributes accordingly}
        public void BarbGen(Character newchar)
        {
            Random rand  = new Random();
            // hitpoint
            int BarbHP = 12 + ((newchar.Level * rand.Next(1,12)+(newchar.ConMod * newchar.Level)));
            newchar.HitPoints = BarbHP;
            // 
            string[]  proficiencies = { "AnimalHandling", "Athletics", "Intimidation", "Nature", "Perception", "Survivial" };
            int numberofpros = 2;
            SkillsGen(newchar, proficiencies, proficiencies.Length, numberofpros);
            // BAse stat increases for level
            // string[] statNames = {"STR", "DEX", "CON", "WIS", "INT", "CHA"};
            if(newchar.Level >= 19)
            {
                newchar.ASI = 10;
            }
            else if(newchar.Level >= 16 && newchar.Level < 19)
            {
                ASI = 8;
            }
            else if(newchar.Level >= 12 && newchar.Level < 16)
            {
                ASI = 6;
            }
            else if(newchar.Level >= 8 && newchar.Level < 12)
            {
                ASI = 4;
            }
            else if(newchar.Level >= 4 && newchar.Level <8)
            {
                ASI = 2;
            }
            // for( int i= 0; i < ASI; i++)
            // {
            //     RandomizeString(statNames, 6);
            //     if(statNames = 0)
            // }
        }
        public void BardGen(Character newchar)
        {
            Random rand  = new Random();
            // hitpoint
            int BardHP = 8 + ((newchar.Level * rand.Next(1,8)+(newchar.ConMod * newchar.Level)));
            newchar.HitPoints = BardHP;

            // Proficiencies class addition 
            string[]  proficiencies = { "Acrobatics", "AnimalHandling", "Arcana", "Athletics", "Deception", "History","Insight","Intimidation","Investigation", "Medicine", "Nature", "Perception", "Performance", "Persuasion", "Religion", "SleightofHand", "Stealth", "Survival"};
            int numberofpros = 3;
            SkillsGen(newchar, proficiencies, proficiencies.Length, numberofpros);
        }
        public void ClericGen(Character newchar)
        {
            Random rand  = new Random();
            // hitpoint
            int ClericHP = 8 + ((newchar.Level * rand.Next(1,8)+(newchar.ConMod * newchar.Level)));
            newchar.HitPoints = ClericHP;

            string[]  proficiencies = {"History", "Insight", "Medicine", "Religion"};
            int numberofpros = 2;
            SkillsGen(newchar, proficiencies, proficiencies.Length, numberofpros);

        }
        public void DruidGen(Character newchar)
        {
            Random rand  = new Random();
            // hitpoint
            int DruidHP = 8 + ((newchar.Level * rand.Next(1,8)+(newchar.ConMod * newchar.Level)));
            newchar.HitPoints = DruidHP;
            string[]  proficiencies = { "AnimalHandling", "Arcana", "Insight", "Medicine", "Nature", "Perception", "Religion", "Survival"};
            int numberofpros = 2;
            SkillsGen(newchar, proficiencies, proficiencies.Length, numberofpros);
        }
        public void FighterGen(Character newchar)
        {
            Random rand  = new Random();
            // hitpoint
            int FighterHP = 10 + ((newchar.Level * rand.Next(1,11)+(newchar.ConMod * newchar.Level)));
            newchar.HitPoints = FighterHP;

            string[]  proficiencies = { "Acrobatics", "AnimalHandling","Athletics","History","Insight","Intimidation","Perception", "Survival"};
            int numberofpros = 2;
            SkillsGen(newchar, proficiencies, proficiencies.Length, numberofpros);
        }
        public void MonkGen(Character newchar)
        {
            Random rand  = new Random();
            // hitpoint
            int MonkHP = 8 + ((newchar.Level * rand.Next(1,9)+(newchar.ConMod * newchar.Level)));
            newchar.HitPoints = MonkHP;

            string[]  proficiencies = { "Acrobatics","Athletics","History","Insight","Religion", "SleightofHand", "Stealth"};
            int numberofpros = 2;
            SkillsGen(newchar, proficiencies, proficiencies.Length, numberofpros);
        }
        public void PaladinGen(Character newchar)
        {
            Random rand  = new Random();
            // hitpoint
            int PaladinHP = 10 + ((newchar.Level * rand.Next(1,11)+(newchar.ConMod * newchar.Level)));
            newchar.HitPoints = PaladinHP;

            string[]  proficiencies = { "Athletics","Insight","Intimidation","Medicine", "Persuasion", "Religion"};
            int numberofpros = 2;
            SkillsGen(newchar, proficiencies, proficiencies.Length, numberofpros);
        }
        public void RangerGen(Character newchar)
        {
            Random rand  = new Random();
            // hitpoint
            int RangerHP = 10 + ((newchar.Level * rand.Next(1,11)+(newchar.ConMod * newchar.Level)));
            newchar.HitPoints = RangerHP;

            string[]  proficiencies = {"AnimalHandling","Athletics","Insight","Investigation","Nature", "Perception", "Stealth", "Survival"};
            int numberofpros = 3;
            SkillsGen(newchar, proficiencies, proficiencies.Length, numberofpros);
        }
        public void RogueGen(Character newchar)
        {
            Random rand  = new Random();
            // hitpoint
            int RogueHP = 8 + ((newchar.Level * rand.Next(1,9)+(newchar.ConMod * newchar.Level)));
            newchar.HitPoints = RogueHP;
            string[]  proficiencies = { "Acrobatics", "Athletics", "Deception","Insight","Intimidation","Investigation", "Perception", "Performance", "Persuasion", "SleightofHand", "Stealth"};
            int numberofpros = 4;
            SkillsGen(newchar, proficiencies, proficiencies.Length, numberofpros);
        }
        public void SorcGen(Character newchar)
        {
            Random rand  = new Random();
            // hitpoint
            int SorcHP = 6 + ((newchar.Level * rand.Next(1,7)+(newchar.ConMod * newchar.Level)));
            newchar.HitPoints = SorcHP;

            string[]  proficiencies = {"Arcana","Deception","Insight","Intimidation", "Persuasion", "Religion"};
            int numberofpros = 2;
            SkillsGen(newchar, proficiencies, proficiencies.Length, numberofpros);
        }
        public void WarlockGen(Character newchar)
        {
            Random rand  = new Random();
            // hitpoint
            int WarlockHP = 8 + ((newchar.Level * rand.Next(1,9)+(newchar.ConMod * newchar.Level)));
            newchar.HitPoints = WarlockHP;
            string[]  proficiencies = { "Arcana", "Deception", "History", "Intimidation", "Investigation", "Nature",  "Religion"};
            int numberofpros = 2;
            SkillsGen(newchar, proficiencies, proficiencies.Length, numberofpros);
        }
        public void WizardGen(Character newchar)
        {
            Random rand  = new Random();
            // hitpoint
            int WizHP = 6 + ((newchar.Level * rand.Next(1,7)+(newchar.ConMod * newchar.Level)));
            newchar.HitPoints = WizHP;
            // abilities (spells/skills/feautres)
            // Profiency bonus generation
            // proficincies
            string[]  proficiencies = { "Arcana", "History", "Insight", "Investigation", "Medicine", "Religion"};
            int numberofpros = 2;
            SkillsGen(newchar, proficiencies, proficiencies.Length, numberofpros);

        }
        //For spells, at least at this point in time, best to give them a number of spells know, how many spells they are allowed to swap, and a number of spells at each level they are allowed to know.

        public void ThirdGeneration(Character newchar)
        {
            if (newchar.PlayerRace == "Dragonborn")
            {
                
                DragonbornGen(newchar);
            }
            else if (newchar.PlayerRace == "Dwarf")
            {
                
                DwarfGen(newchar);
            }
            else if (newchar.PlayerRace == "Elf")
            {
                ElfGen(newchar);
            }
            else if (newchar.PlayerRace == "Gnome")
            {
                GnomeGen(newchar);
            }
            else if (newchar.PlayerRace == "Half-Elf")
            {
                HalfElfGen(newchar);
            }
            else if (newchar.PlayerRace == "Halfling")
            {
                HalflingGen(newchar);
            }
            else if (newchar.PlayerRace == "Half-Orc")
            {
                HalfOrcGen(newchar);
            }
            else if (newchar.PlayerRace == "Human")
            {
                HumanGen(newchar);
            }
            else if (newchar.PlayerRace == "Tiefling")
            {
                TieflingGen(newchar);
            }
        }

        public void DragonbornGen(Character newchar)
        {
            newchar.Strength = newchar.Strength + 2;
            newchar.Charisma = newchar.Charisma + 1;

            // Spe
        }
        public void DwarfGen(Character newchar)
        {
            newchar.Constitution = newchar.Constitution + 2;
            // speed
        }
        public void ElfGen(Character newchar)
        {
            newchar.Dexterity = newchar.Dexterity + 2;
        }
        public void GnomeGen(Character newchar)
        {
            newchar.Intelligence = newchar.Intelligence + 2;
        }
        public void HalfElfGen(Character newchar)
        {
            Random rand  = new Random();
            newchar.Charisma = newchar.Charisma + 2;
            string[]  skills = { "Strength", "Charisma", "Intelligence", "Dexterity", "Wisdom", "Constitution" };
            RandomizeString(skills, 6);
            int numberofskills = 1;
            for (int i = 0;  i< numberofskills; i++)
                {
                    if(skills[i] == "Strength")
                        {
                            newchar.Strength = newchar.Strength + 1;
                        }
                    else if (skills[i] == "Charisma")
                        {
                            newchar.Charisma = newchar.Charisma + 1;
                        }
                    else if (skills[i] == "Intelligence")
                        {
                            newchar.Intelligence = newchar.Intelligence + 1;
                        }
                    else if (skills[i] == "Dexterity")
                        {
                            newchar.Dexterity = newchar.Dexterity + 1;
                        }
                    else if (skills[i] == "Wisdom")
                        {
                            newchar.Wisdom = newchar.Wisdom + 1;
                        }
                    else if (skills[i] == "Constitution")
                        {
                            newchar.Constitution = newchar.Constitution + 1;
                        }
                }
            
        }
        public void HalflingGen(Character newchar)
        {
            newchar.Dexterity = newchar.Dexterity + 2;
        }
        public void HalfOrcGen(Character newchar)
        {
            newchar.Strength = newchar.Strength + 1;
            newchar.Constitution = newchar.Constitution +1;
        }
        public void HumanGen(Character newchar)
        {
            newchar.Strength = newchar.Strength + 1;
            newchar.Constitution = newchar.Constitution +1;
            newchar.Intelligence = newchar.Intelligence + 1;
            newchar.Dexterity = newchar.Dexterity + 1;
            newchar.Charisma = newchar.Charisma + 1;
            newchar.Wisdom = newchar.Wisdom + 1;
        }
        public void TieflingGen(Character newchar)
        {
            newchar.Charisma = newchar.Charisma + 2;
            newchar.Intelligence = newchar.Intelligence + 1;
        }

        public int ProficiencyBonusGen(int level)
        {
            if( level >= 17)
            {
                return 6;
            }
            if( level >= 13 && level < 17)
            {
                return 5;
            }
            if( level >= 9 && level < 13)
            {
                return 4;
            }
            if( level >= 5 && level < 9)
            {
                return 3;
            }
            else
            {
                return 2;
            }

        }
        public void FourthGeneration(Character newchar)
        {
            if (newchar.PlayerBG == "Acolyte")
            {
                newchar.Insight = ProficiencyBonus;
                newchar.Religion = ProficiencyBonus;
            }
            else if (newchar.PlayerBG == "Charlatan")
            {
                newchar.Deception = ProficiencyBonus;
                newchar.SleightofHand = ProficiencyBonus;
            }
            else if (newchar.PlayerBG == "Criminal")
            {
               newchar.Deception = ProficiencyBonus;
               newchar.Stealth = ProficiencyBonus;
            }
            else if (newchar.PlayerBG == "Spy")
            {
                newchar.Deception = ProficiencyBonus;
               newchar.Stealth = ProficiencyBonus;
            }
            else if (newchar.PlayerBG == "Entertainer")
            {
                newchar.Acrobatics = ProficiencyBonus;
                newchar.Performance = ProficiencyBonus;
            }
            else if (newchar.PlayerBG == "Folk Hero")
            {
                newchar.AnimalHandling = ProficiencyBonus;
                newchar.Survival = ProficiencyBonus;
            }
            else if (newchar.PlayerBG == "Gladiator")
            {
                newchar.Acrobatics = ProficiencyBonus;
                newchar.Performance = ProficiencyBonus;
            }
            else if (newchar.PlayerBG == "Guild Artisan")
            {
                newchar.Insight = ProficiencyBonus;
                newchar.Persuasion = ProficiencyBonus;
            }
            else if (newchar.PlayerBG == "Knight")
            {
                newchar.History = ProficiencyBonus;
                newchar.Persuasion = ProficiencyBonus;
            }
            else if (newchar.PlayerBG == "Guild Merchant")
            {
                newchar.Insight = ProficiencyBonus;
                newchar.Persuasion = ProficiencyBonus;
            }
            else if (newchar.PlayerBG == "Hermit")
            {
                newchar.Medicine = ProficiencyBonus;
                newchar.Religion = ProficiencyBonus;
            }
            else if (newchar.PlayerBG == "Noble")
            {
                newchar.History = ProficiencyBonus;
                newchar.Persuasion = ProficiencyBonus;
            }
            else if (newchar.PlayerBG == "Outlander")
            {
                newchar.Athletics = ProficiencyBonus;
                newchar.Survival = ProficiencyBonus;
            }
            else if (newchar.PlayerBG == "Pirate")
            {
                newchar.Athletics = ProficiencyBonus;
                newchar.Perception = ProficiencyBonus;
            }
            else if (newchar.PlayerBG == "Sage")
            {
                newchar.Arcana = ProficiencyBonus;
                newchar.History = ProficiencyBonus;
            }
            else if (newchar.PlayerBG == "Salior")
            {
                newchar.Athletics = ProficiencyBonus;
                newchar.Perception = ProficiencyBonus;
            }
            else if (newchar.PlayerBG == "Soldier")
            {
                newchar.Athletics = ProficiencyBonus;
                newchar.Intimidation = ProficiencyBonus;
            }
            else if (newchar.PlayerBG == "Urchin")
            {
                newchar.SleightofHand = ProficiencyBonus;
                newchar.Stealth = ProficiencyBonus;
            }
        }

        public void FifthGeneration(Character newchar)
        {
            newchar.Acrobatics = newchar.Acrobatics + newchar.DexMod;
            newchar.AnimalHandling = newchar.AnimalHandling + newchar.WisMod;
            newchar.Arcana = newchar.Arcana + newchar.IntMod;
            newchar.Athletics = newchar.Athletics + newchar.StrMod;
            newchar.Deception = newchar.Deception + newchar.ChaMod;
            newchar.History = newchar.History + newchar.IntMod;
            newchar.Intimidation = newchar.Intimidation + newchar.ChaMod;
            newchar.Investigation = newchar.Investigation + newchar.IntMod;
            newchar.Medicine = newchar.Medicine + newchar.WisMod;
            newchar.Nature = newchar.Nature + newchar.IntMod;
            newchar.Perception = newchar.Perception + newchar.WisMod;
            newchar.Performance = newchar.Performance+ newchar.ChaMod;
            newchar.Persuasion = newchar.Persuasion + newchar.ChaMod;
            newchar.Religion = newchar.Religion + newchar.IntMod;
            newchar.SleightofHand = newchar.SleightofHand + newchar.DexMod;
            newchar.Stealth = newchar.Stealth + newchar.DexMod;
            newchar.Survival = newchar.Survival + newchar.WisMod;
        }
    }
}