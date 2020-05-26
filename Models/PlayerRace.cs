using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CharacterGenerator.Models
{
    public class PlayerRace
    {
        [Key]
        public int PlayerRaceId {get; set;}
        public string Race { get; set; }
        public string Subrace {get; set;}
        public int Speed { get; set; } = 30;
        public bool Flight { get; set; } = false;
        public int FlightSpeed { get; set; } = 0;
        public bool ASI { get; set; }
        public string RacialTraits1 {get; set;}
        public string RacialTraits2 {get; set;}
        public string RacialTraits3 {get; set;}
        public string RacialTraits4 {get; set;}
        public string RacialTraits5 {get; set;}
        public string SubRacialTraits1 {get; set;}
        public string SubRacialTraits2 {get; set;}
        public string SubRacialTraits3 {get; set;}
        public string RaceSize {get; set;} = "Medium";
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public PlayerRace()
        {
            
        }
        public PlayerRace(PlayerStat playerStat, PlayerRace playerRace)
        {
            
        }

        // Takes player Race obj and selects a specific Race and increases bonuses and detmines traits
        public PlayerStat RaceSelector(int Level, PlayerStat playerStat, PlayerRace playerRace)
        {
            Random rand = new Random();
            int num = rand.Next(0,9);
            switch(num)
            {
                case 0:
                    playerRace.Race = "DragonBorn";
                    DragonBorn(Level, playerStat, playerRace);
                    return playerStat;
                case 1:
                    playerRace.Race = "Dwarf";
                    Dwarf(Level, playerStat, playerRace);
                    return playerStat;
                case 2:
                    playerRace.Race = "Elf";
                    Elf(Level, playerStat, playerRace);
                    return playerStat;
                case 3:
                    playerRace.Race = "Gnome";
                    Gnome(Level, playerStat, playerRace);
                    return playerStat;
                case 4:
                    playerRace.Race = "Half Elf";
                    HalfElf(Level, playerStat, playerRace);
                    return playerStat;
                case 5:
                    playerRace.Race = "Half Orc";
                    HalfOrc(Level, playerStat, playerRace);
                    return playerStat;
                case 6:
                    playerRace.Race = "Halfling";
                    Halfling(Level, playerStat, playerRace);
                    return playerStat;
                case 7:
                    playerRace.Race = "Human";
                    Human(Level, playerStat, playerRace);
                    return playerStat;
                case 8:
                    playerRace.Race = "Tiefling";
                    Tiefling(Level, playerStat, playerRace);
                    return playerStat;
            }
            return playerStat;
        }


        // Dragonborn and Dragon Ancestry determine Dragonborn race
        public PlayerStat DragonBorn(int Level, PlayerStat playerStat, PlayerRace playerRace)
        {
            Random rand = new Random();
            int num = rand.Next(0,10);
            string[] DragonType = {"Black", "Blue", "Brass", "Bronze", "Copper", "Gold", "Green", "Red", "Silver", "White"};
            string[] DamageType = {"Acid", "Lightning", "Fire", "Lightning", "Acid", "Fire", "Poison", "Fire", "Cold", "Cold"};
            playerRace.RacialTraits1 = DamageType[num] + " Dragon Breath";
            playerStat.Strength += 2;
            playerStat.Charisma += 1;
            playerRace.ASI = true;
            playerRace.Subrace = DragonType[num];
            playerStat.LangDraconic = true;
            playerStat.LangUndercommon = true;
            return playerStat;
            
        }
        // public string DraconicAncestry(PlayerRace playerRace)
        // {
        //     Random rand = new Random();
        //     int num = rand.Next(0,10);
        //     string[] DragonType = {"Black", "Blue", "Brass", "Bronze", "Copper", "Gold", "Green", "Red", "Silver", "White"};
        //     string[] DamageType = {"Acid", "Lightning", "Fire", "Lightning", "Acid", "Fire", "Poison", "Fire", "Cold", "Cold"};
        //     playerRace.RacialTraits1 = DamageType[num] + " Dragon Breath";
        //     return DragonType[num];
        // }

        // Next race
        public PlayerStat Dwarf(int Level, PlayerStat playerStat, PlayerRace playerRace)
            {
                //base dwarf ASI
                playerStat.Constitution += 2;

                //Creating for subraces
                Random rand = new Random();
                int num = rand.Next(0,1);
                switch(num)
                    {
                        case 0:
                            playerRace.Subrace ="Hill";
                            playerStat.Wisdom +=1;
                            playerRace.SubRacialTraits1 = "Dwarven Toughness";
                            playerStat.HitPoints += Level;
                            break;
                        case 1: 
                            playerRace.Subrace = "Mountain";
                            playerStat.Strength += 2;
                            playerRace.SubRacialTraits1 = "Dwarven Armor Training";
                            break;
                    }
                playerRace.ASI = true;
                playerRace.RacialTraits1 = "Darkvision (60 ft)";
                playerRace.RacialTraits2 = "Stonecunning";
                playerRace.RacialTraits3 = "Dwarven Resilience";
                playerRace.RacialTraits4 = "Tool Proficiency";
                playerRace.RacialTraits5 = "Stonecunning";
                playerRace.Speed = 25;
                playerStat.LangDwarvish = true;
                return playerStat;
            }
            public PlayerStat Elf(int Level, PlayerStat playerStat, PlayerRace playerRace)
            {
                //base elf ASI
                playerStat.Dexterity += 2;
                //Creating for subraces
                Random rand = new Random();
                int num = rand.Next(0,1);
                switch(num)
                    {
                        case 0:
                            playerRace.Subrace ="High";
                            playerStat.Intelligence +=1;
                            playerRace.SubRacialTraits1 = "Cantrip (Wizard)";
                                //add Cantrip from the Wizard Spell list to the character at random
                            playerRace.SubRacialTraits2 = "Elf Weapon Training";
                            playerRace.SubRacialTraits3 = "Extra Language";
                            break;
                        case 1: 
                            playerRace.Subrace = "Wood";
                            playerStat.Wisdom += 1;
                            playerRace.SubRacialTraits1 = "Elf Weapon Training";
                            playerRace.SubRacialTraits2 = "Fleet of Foot";
                                playerRace.Speed = 35;
                            playerRace.SubRacialTraits3 = "Mask of the Wild";
                            break;
                    }
                playerRace.ASI = true;
                playerRace.RacialTraits1 = "Darkvision (60 ft)";
                playerRace.RacialTraits2 = "Fey Ancestry";
                playerRace.RacialTraits3 = "Trance";
                playerRace.RacialTraits4 = "Keen Senses";
                    //adding in Perception from playerstat, need to make sure that the selection doesn't double dip
                    playerStat.PerceptionB = true;
                playerStat.LangElvish = true;
                return playerStat;
            }
        public PlayerStat Gnome(int Level, PlayerStat playerStat, PlayerRace playerRace)
        {
            //base gnome ASI
            playerStat.Intelligence += 2;
            playerRace.Speed = 25;
            playerRace.RacialTraits1 = "Darkvision (60 ft)";
            playerRace.RacialTraits2 ="Gnome Cunning";
             Random rand = new Random();
                int num = rand.Next(1,2);
                switch(num)
                {
                    case 0:
                        playerRace.Subrace ="Forest";
                        playerStat.Dexterity += 1;
                        playerRace.SubRacialTraits1 ="Natural Illusionist";
                            //Add Minor Illusion cantrip
                        playerRace.SubRacialTraits2 ="Speak with Small Beasts";
                        break;
                    case 1:
                        playerRace.Subrace = "Rock";
                        playerStat.Constitution += 1;
                        playerRace.SubRacialTraits1 = "Artificer's Lore";
                        playerRace.SubRacialTraits2 = "Tinker";
                        break;
                }
                playerRace.RaceSize = "Small";
            playerRace.ASI = true;
            playerStat.LangGnomish = true;
            return playerStat;
        }
        public PlayerStat HalfElf(int Level, PlayerStat playerStat, PlayerRace playerRace)
        {
            playerStat.Charisma +=2;
            int[] randostats = {0,1,2,3,4};
            Randomize(randostats, randostats.Length);
            for (int i = 0; i < 2; i++)
                {
                    int var = randostats[i];
                    switch(var)
                        {
                            case 0:
                                playerStat.Strength+=1;
                            break;

                            case 1:
                                playerStat.Dexterity+=1;
                            break;

                            case 2:
                                playerStat.Constitution+=1;
                            break;

                            case 3:
                                playerStat.Wisdom+=1;
                            break;

                            case 4:
                                playerStat.Intelligence+=1;
                            break;
                        }
                }
            playerRace.RacialTraits1 ="Darkvision (60 ft)";
            playerRace.RacialTraits2 = "Fey Ancestry";
            playerRace.RacialTraits3 ="High-Elf Versitility"; //Adds two proficiences of any type

            int[] SkillList = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17};
            Randomize(SkillList, SkillList.Length);
            Random rand = new Random();
            int NumOfPro = 2;
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
                        if(playerStat.AnimalHandlingB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        playerStat.AnimalHandlingB = true;
                        break;

                    case 1:
                        if(playerStat.AthleticsB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        playerStat.AthleticsB = true;
                        break;

                    case 2:
                        if(playerStat.AcrobaticsB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        playerStat.AcrobaticsB = true;
                        break;

                    case 3:
                        if(playerStat.DeceptionB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        playerStat.DeceptionB = true;
                        break;

                    case 4:
                        if(playerStat.HistoryB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        playerStat.HistoryB = true;
                        break;

                    case 5:
                        if(playerStat.ArcanaB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        playerStat.ArcanaB = true;
                        break;

                    case 6:
                        if(playerStat.InsightB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        playerStat.InsightB = true;
                        break;

                    case 7:
                        if(playerStat.IntimidationB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        playerStat.IntimidationB = true;
                        break;

                    case 8:
                        if(playerStat.InvestigationB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        playerStat.InvestigationB = true;
                        break;

                    case 9:
                        if(playerStat.MedicineB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        playerStat.MedicineB = true;
                        break;

                    case 10:
                        if(playerStat.NatureB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        playerStat.NatureB = true;
                        break;

                    case 11:
                        if(playerStat.PerceptionB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        playerStat.PerceptionB = true;
                        break;

                    case 12:
                        if(playerStat.PerformanceB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        playerStat.PerformanceB = true;
                        break;

                    case 13:
                        if(playerStat.PersuasionB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        playerStat.PersuasionB = true;
                        break;

                    case 14:
                        if(playerStat.ReligionB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        playerStat.ReligionB = true;
                        break;

                    case 15:
                        if(playerStat.SleightB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        playerStat.SleightB = true;
                        break;

                    case 16:
                        if(playerStat.StealthB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        playerStat.StealthB = true;
                        break;

                    case 17:
                        if(playerStat.SurvivalB == true)
                        {
                            NumOfPro++;
                            break;
                        }
                        playerStat.SurvivalB = true;
                        break;
                };
            }
            playerStat.LangElvish = true;
            int[] langgen = {0,1,2,3,4,5,6,7,8,9,10,11,12,13};
            Randomize(langgen, langgen.Length);
            int num1 = langgen[0];
            switch(num1)
                {
                    case 0:
                        if (playerStat.LangDwarvish == true)
                            {
                                goto case 1;
                            }
                        else
                            {
                                playerStat.LangDwarvish = true;
                            }
                        break;

                        case 1:
                        if (playerStat.LangGiant == true)
                            {
                                goto case 2;
                            }
                        else
                            {
                                playerStat.LangGiant = true;
                            }
                        break;

                        case 2:
                            if (playerStat.LangGnomish == true)
                                {
                                    goto case 3;
                                }
                            else
                                {
                                    playerStat.LangGnomish = true;
                                }
                            break;

                        case 3:
                            if (playerStat.LangGoblin == true)
                                {
                                    goto case 4;
                                }
                            else
                                {
                                    playerStat.LangGoblin = true;
                                }
                            break;

                        case 4:
                            if (playerStat.LangHalfling == true)
                                {
                                    goto case 5;
                                }
                            else
                                {
                                    playerStat.LangHalfling = true;
                                }
                            break;

                        case 5:
                            if (playerStat.LangOrcish == true)
                                {
                                    goto case 6;
                                }
                            else
                                {
                                    playerStat.LangOrcish = true;
                                }
                            break;

                        case 6:
                            if (playerStat.LangAbyssal == true)
                                {
                                    goto case 7;
                                }
                            else
                                {
                                    playerStat.LangAbyssal = true;
                                }
                            break;
                        case 7:
                            if (playerStat.LangCelestial == true)
                                {
                                    goto case 8;
                                }
                            else
                                {
                                    playerStat.LangCelestial = true;
                                }
                            break;

                        case 8:
                            if (playerStat.LangDraconic == true)
                                {
                                    goto case 9;
                                }
                            else
                                {
                                    playerStat.LangDraconic = true;
                                }
                            break;

                        case 9:
                            if (playerStat.LangDeepSpeech == true)
                                {
                                    goto case 10;
                                }
                            else
                                {
                                    playerStat.LangDeepSpeech = true;
                                }
                            break;

                        case 10:
                            if (playerStat.LangInfernal == true)
                                {
                                    goto case 11;
                                }
                            else
                                {
                                    playerStat.LangInfernal = true;
                                }
                            break;

                        case 11:
                            if (playerStat.LangPrimordial == true)
                                {
                                    goto case 12;
                                }
                            else
                                {
                                    playerStat.LangPrimordial = true;
                                }
                            break;

                            case 12:
                            if (playerStat.LangSylvan == true)
                                {
                                    goto case 13;
                                }
                            else
                                {
                                    playerStat.LangSylvan = true;
                                }
                            break;

                            case 13:
                            if (playerStat.LangUndercommon == true)
                                {
                                    goto case 0;
                                }
                            else
                                {
                                    playerStat.LangUndercommon = true;
                                }
                            break;
                }
            return playerStat;
        }


        public PlayerStat HalfOrc(int Level, PlayerStat playerStat, PlayerRace playerRace)
        {
            playerStat.Strength +=2;
            playerStat.Constitution +=1;
            playerRace.RacialTraits1 ="Darkvision (60 ft)";
            playerRace.RacialTraits2 = "Menacing";
                playerStat.IntimidationB = true;
            playerRace.RacialTraits3 = "Relentless Endurance";
            playerRace.RacialTraits4 = "Savage Attacks";
            playerStat.LangOrcish = true;
            return playerStat;
        }

        public PlayerStat Halfling(int Level, PlayerStat playerStat, PlayerRace playerRace)
        {
            playerStat.Dexterity += 2;
            Random rand = new Random();
            int num = rand.Next(0,1);
            switch (num)
                {
                    case 0:
                        playerRace.Subrace = "Lightfoot";
                        playerRace.SubRacialTraits1 = "Naturally Stealthy";
                        playerStat.Charisma +=1;
                        break;
                    case 1:
                        playerRace.Subrace = "Stout";
                        playerRace.SubRacialTraits1 = "Stout Resilience";
                        playerStat.Constitution +=1;
                        break;
                }
            playerRace.ASI = true;
            playerRace.Speed = 25;
            playerRace.RacialTraits1 = "Lucky";
            playerRace.RacialTraits2 = "Brave";
            playerRace.RacialTraits3 = "Nimble";
            playerRace.RaceSize = "Small";
            playerStat.LangHalfling = true;
            return playerStat;
        }

        public PlayerStat Human(int Level, PlayerStat playerStat, PlayerRace playerRace)
        {
            playerStat.Strength +=1;
            playerStat.Dexterity +=1;
            playerStat.Constitution +=1;
            playerStat.Intelligence +=1;

            int[] langgen = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14};
            Randomize(langgen, langgen.Length);
            int num1 = langgen[0];
            switch(num1)
                {
                    case 0:
                        if (playerStat.LangDwarvish == true)
                            {
                                goto case 1;
                            }
                        else
                            {
                                playerStat.LangDwarvish = true;
                            }
                        break;

                        case 1:
                        if (playerStat.LangGiant == true)
                            {
                                goto case 2;
                            }
                        else
                            {
                                playerStat.LangGiant = true;
                            }
                        break;

                        case 2:
                            if (playerStat.LangGnomish == true)
                                {
                                    goto case 3;
                                }
                            else
                                {
                                    playerStat.LangGnomish = true;
                                }
                            break;

                        case 3:
                            if (playerStat.LangGoblin == true)
                                {
                                    goto case 4;
                                }
                            else
                                {
                                    playerStat.LangGoblin = true;
                                }
                            break;

                        case 4:
                            if (playerStat.LangHalfling == true)
                                {
                                    goto case 5;
                                }
                            else
                                {
                                    playerStat.LangHalfling = true;
                                }
                            break;

                        case 5:
                            if (playerStat.LangOrcish == true)
                                {
                                    goto case 6;
                                }
                            else
                                {
                                    playerStat.LangOrcish = true;
                                }
                            break;

                        case 6:
                            if (playerStat.LangAbyssal == true)
                                {
                                    goto case 7;
                                }
                            else
                                {
                                    playerStat.LangAbyssal = true;
                                }
                            break;
                        case 7:
                            if (playerStat.LangCelestial == true)
                                {
                                    goto case 8;
                                }
                            else
                                {
                                    playerStat.LangCelestial = true;
                                }
                            break;

                        case 8:
                            if (playerStat.LangDraconic == true)
                                {
                                    goto case 9;
                                }
                            else
                                {
                                    playerStat.LangDraconic = true;
                                }
                            break;

                        case 9:
                            if (playerStat.LangDeepSpeech == true)
                                {
                                    goto case 10;
                                }
                            else
                                {
                                    playerStat.LangDeepSpeech = true;
                                }
                            break;

                        case 10:
                            if (playerStat.LangInfernal == true)
                                {
                                    goto case 11;
                                }
                            else
                                {
                                    playerStat.LangInfernal = true;
                                }
                            break;

                        case 11:
                            if (playerStat.LangPrimordial == true)
                                {
                                    goto case 12;
                                }
                            else
                                {
                                    playerStat.LangPrimordial = true;
                                }
                            break;

                            case 12:
                            if (playerStat.LangSylvan == true)
                                {
                                    goto case 13;
                                }
                            else
                                {
                                    playerStat.LangSylvan = true;
                                }
                            break;

                            case 13:
                            if (playerStat.LangUndercommon == true)
                                {
                                    goto case 14;
                                }
                            else
                                {
                                    playerStat.LangUndercommon = true;
                                }
                            break;

                            case 14:
                            if (playerStat.LangElvish == true)
                                {
                                    goto case 0;
                                }
                            else
                                {
                                    playerStat.LangElvish = true;
                                }
                            break;
                }
            return playerStat;
        }

        public PlayerStat Tiefling(int Level, PlayerStat playerStat, PlayerRace playerRace)
        {
            playerStat.Intelligence +=1;
            playerStat.Charisma += 2;
            playerRace.ASI = true;
            playerRace.RacialTraits1 ="Darkvision (60 ft)";
            playerRace.RacialTraits2 = "Hellish Resistance";
            playerStat.LangInfernal = true;
            return playerStat;
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
