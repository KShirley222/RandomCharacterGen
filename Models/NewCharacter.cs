using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CharacterGenerator.Models
{
    public class NewCharacter
    {
        [Key]
        public int CharacterId { get; set; }
        // Consider creating another class to hold connection between user and character. then characters will spawn without logged in user
        public User user { get; set; }
        public int UserId { get; set; }
        public int playerClassId { get; set; }
        public PlayerClass playerClass { get; set; }
        public int playerRaceId { get; set; }
        public PlayerRace playerRace { get; set; }
        public int playerBGId { get; set; }
        public PlayerBG playerBG { get; set; }
        public int playerStatId { get; set; }
        public PlayerStat playerStat { get; set; }
        public List<SpellAssoc> SpellList { get; set; }
        public List<FeatureAssoc> FeaturesList { get; set; }
        public int Level { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public NewCharacter(int level)
        {
            Level = level;
        }
        public NewCharacter(int level, PlayerStat pStat,  PlayerRace pRace, PlayerClass pClass, PlayerBG pBG, User pUser)
        {
            Level = level;
            playerStat = pStat;
            playerStatId = pStat.PlayerStatId;
            playerClassId = pClass.PlayerClassId;
            playerClass = pClass;
            playerRaceId = pRace.PlayerRaceId;
            playerRace = pRace;
            playerBGId = pBG.PlayerBGId;
            playerBG = pBG;
            user = pUser;
            UserId = pUser.UserId;
            
        }
        public NewCharacter(int level, PlayerStat pStat,  PlayerRace pRace, PlayerClass pClass, PlayerBG pBG)
        {
            Level = level;
            playerStat = pStat;
            playerStatId = pStat.PlayerStatId;
            playerClassId = pClass.PlayerClassId;
            playerClass = pClass;
            playerRaceId = pRace.PlayerRaceId;
            playerRace = pRace;
            playerBGId = pBG.PlayerBGId;
            playerBG = pBG;
            UserId = 1;
            
        }
    }
}