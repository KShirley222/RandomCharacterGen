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
            int num = rand.Next(0,15);
            switch(num)
            {
                case 0:
                    playerBG.Background = "Acolyte"; //2 Lang prof
                    playerStat.InsightB = true;
                    playerStat.ReligionB = true;
                    LangGenerator(2,playerStat);
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "Shelter of the Fainthful"; 
                    return playerStat;
                case 1:
                    playerBG.Background = "Charlatan"; // Prof in Disguise and Forgery Kit
                    playerStat.DeceptionB = true;
                    playerStat.SleightB = true;
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "False Identity";
                    return playerStat;
                case 2:
                    playerBG.Background = "Criminal / Spy"; //Prof in 1 gaming set, thieve's tools
                    playerStat.DeceptionB = true;
                    playerStat.StealthB = true;
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "Criminal Contact";
                    return playerStat;
                case 3:
                    playerBG.Background = "Entertainer / Gladiator"; //Prof in Disguise Kit, 1 musical instrument
                    playerStat.AcrobaticsB = true;
                    playerStat.PerformanceB = true;
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "By Popular Demand";
                    return playerStat;
                case 4:
                    playerBG.Background = "Folk Hero"; //Artisan's Tools, 1 type of land vehicle
                    playerStat.AnimalHandlingB = true;
                    playerStat.SurvivalB = true;
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "Rustic Hospitality";
                    return playerStat;
                case 5:
                    playerBG.Background = "Guild Artisan / Guild Merchant"; //1 type of artisan's tools, 1 lang
                    playerStat.InsightB = true;
                    playerStat.PersuasionB = true;
                    LangGenerator(1,playerStat);
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "Guild Membership";
                    return playerStat;
                case 6:
                    playerBG.Background = "Hermit"; //herbalism kit, 1 lang
                    playerStat.MedicineB = true;
                    playerStat.ReligionB= true;
                    LangGenerator(1,playerStat);
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "Discovery";
                    return playerStat;
                case 7:
                    playerBG.Background = "Knight"; //1 gaming set, 1 lang
                    playerStat.HistoryB = true;
                    playerStat.PersuasionB = true;
                    LangGenerator(1,playerStat);
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "Retainers";
                    return playerStat;
                case 8:
                    playerBG.Background = "Noble"; //1 gaming set, 1 lang
                    playerStat.HistoryB = true;
                    playerStat.PersuasionB = true;
                    LangGenerator(1,playerStat);
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "Position of Privilege";
                    return playerStat;
                case 9:
                    playerBG.Background = "Outlander"; // 1 instrument, 1 lang
                    playerStat.AthleticsB = true;
                    playerStat.SurvivalB = true;
                    LangGenerator(1,playerStat);
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "Wanderer";
                    return playerStat;
                case 10:
                    playerBG.Background = "Pirate"; //Nav Tools, 1 Water Vehicle
                    playerStat.AthleticsB = true;
                    playerStat.PerceptionB = true;
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "Bad Reputation";
                    return playerStat;
                case 11:
                    playerBG.Background = "Sage"; //2 lang
                    playerStat.ArcanaB = true;
                    playerStat.HistoryB = true;
                    LangGenerator(2,playerStat);
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "Researcher";
                    return playerStat;
                case 12:
                    playerBG.Background = "Sailor"; //Nav Tools, 1 Water Vehicle
                    playerStat.AthleticsB = true;
                    playerStat.PerceptionB = true;
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "Ship's Passage";
                    return playerStat;
                case 13:
                    playerBG.Background = "Soldier"; //Gaming set, land vehicles
                    playerStat.AthleticsB = true;
                    playerStat.IntimidationB = true;
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "Military Rank";
                    return playerStat;
                case 14:
                    playerBG.Background = "Urchin"; //disguise kit, thieve's tools
                    playerStat.SleightB = true;
                    playerStat.StealthB = true;
                    playerBG.Proficiencies = true;
                    playerBG.Feature = "City Secrets";
                    return playerStat;
            }
            return playerStat;
        }

        public static void LangGenerator(int num_of_lang, PlayerStat playerStat)
        {
            Random rand = new Random();
            for (int i = 0; i<num_of_lang; i++)
            {
                    int langnum = rand.Next(0,15);
                    switch(langnum)
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
            }
        }
    }
}