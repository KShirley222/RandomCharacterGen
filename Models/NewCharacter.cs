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
        public string playerNotes { get; set; }
        public string playerName { get; set; }
        public bool isSaved {get; set;} = false;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public NewCharacter(){}
        
        public NewCharacter(int level, User testUser)
        {
            Level = 1;
            user = testUser; 
            UserId = testUser.UserId;
            
            playerStat = new PlayerStat(1);
            playerStatId = playerStat.PlayerStatId;

            playerBG = new PlayerBG();
            playerBG.BGSelector(playerStat, playerBG);
            playerBGId = playerBG.PlayerBGId;

            playerRace = new PlayerRace();
            playerRace.RaceSelector(Level, playerStat, playerRace);
            playerRaceId = playerRace.PlayerRaceId;

            playerClass = new PlayerClass(Level, playerStat);
            playerClass.ClassSelector(Level, playerStat, playerClass);
            playerClassId = playerClass.PlayerClassId;

            playerStat.UpdatePro(playerStat);
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
            playerName = "";
            playerNotes = "";
            
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