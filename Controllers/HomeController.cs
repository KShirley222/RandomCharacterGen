using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CharacterGenerator.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

using Microsoft.EntityFrameworkCore;
using System.Dynamic;

using Newtonsoft.Json.Linq;

namespace CharacterGenerator.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;
        
        public HomeController(MyContext context)
        {
            _context = context;
        }
        

        [HttpGet("")]
        public IActionResult Index()
        {
            Random rand = new Random();
            // Check for features, if null populate DB
            Feature featTest = _context.Features.FirstOrDefault( f => f.FeatureId == 1);
            if( featTest == null)
            {
                Feature feats = new Feature();
                List<Feature> FeatureList = feats.GenerateFeatureList();
                foreach(Feature f in FeatureList)
                {
                    _context.Features.Add(f);
                    _context.SaveChanges();
                }
            }
            // Check for spells, if null populate DB
            Spell spellTest = _context.Spells.FirstOrDefault ( s => s.SpellId == 1);
            if( spellTest == null)
            {
                Spell spell = new Spell();
                List<Spell> SpellList = spell.GenerateSpellList();
                foreach( Spell s in SpellList)
                {
                    _context.Spells.Add(s);
                    _context.SaveChanges();
                }
            }
            // Check for base user, if null Populate DB
            User userCheck = _context.Users.FirstOrDefault( u => u.UserId == 1 );
            if( userCheck == null)
            {
                string email = "Test@test.com";
                string uName = "UnameTest";
                string PW = "vu942kjdaiduq934i3o4odsud9qw324";
                User testUser = new User(email, uName, PW);
                _context.Users.Add(testUser);
                _context.SaveChanges();
                userCheck = testUser;
            }
            // Create a function to generate 1 character for display if none in dB
            NewCharacter characterCheck = _context.NewCharacter.FirstOrDefault( c => c.CharacterId == 1);
            if( characterCheck == null )
            {
                NewCharacter test = new NewCharacter(1, userCheck);
                _context.PlayerStats.Add(test.playerStat); 
                _context.PlayerBGs.Add(test.playerBG); 
                _context.PlayerClasses.Add(test.playerClass);
                _context.PlayerRaces.Add(test.playerRace);
                _context.NewCharacter.Add(test);
                _context.SaveChanges();
                List<Feature> charFeatures = _context.Features.Where(f =>  ((f.FeatSource == test.playerClass.ClassName) && (f.FeatLevel <= test.Level)) || ((f.FeatSource == test.playerClass.SubClassName) && (f.FeatLevel <= test.Level))).ToList();
                foreach( Feature feat in charFeatures )
                    {
                        var Fassoc = new FeatureAssoc(test, feat); //id 1- 42
                        _context.Feature_Associations.Add(Fassoc);
                        _context.SaveChanges();
                    }
                
                // Get all associated features with characterID
                List<Feature> Feats = _context.NewCharacter
                    .Include(c => c.FeaturesList)
                    .ThenInclude(fa => fa.FeatureA)
                    .FirstOrDefault(c => c.CharacterId == test.CharacterId)
                    .FeaturesList.Select(f => f.FeatureA)
                    .OrderBy(f => f.FeatLevel)
                    .ToList();

                List<Spell> ClassAvailbleSpells = _context.Spells.Where(s => 
                              s.Source1 == test.playerClass.ClassName
                            ||s.Source2 == test.playerClass.ClassName
                            ||s.Source3 == test.playerClass.ClassName
                            ||s.Source4 == test.playerClass.ClassName
                            ||s.Source5 == test.playerClass.ClassName
                            ||s.Source6 == test.playerClass.ClassName
                            ||s.Source7 == test.playerClass.ClassName
                            ).ToList();
                List<Spell> NonClassSpells = _context.Spells.Where(s => 
                              s.Source1 != test.playerClass.ClassName
                            ||s.Source2 != test.playerClass.ClassName
                            ||s.Source3 != test.playerClass.ClassName
                            ||s.Source4 != test.playerClass.ClassName
                            ||s.Source5 != test.playerClass.ClassName
                            ||s.Source6 != test.playerClass.ClassName
                            ||s.Source7 != test.playerClass.ClassName
                            ).ToList(); 
                                      
                Spell FetchSpell = new Spell();
                List<Spell> Spells = FetchSpell.GetPossibleSpells(test, ClassAvailbleSpells, NonClassSpells);
                foreach(Spell s in Spells)
                {
                    SpellAssoc a = new SpellAssoc(test, s);
                    

                    if (test.playerClass.SubClassName == "Circle of the Land: Arctic" && s.SpellName == "Hold Person"||
                        test.playerClass.SubClassName == "Circle of the Land: Arctic" && s.SpellName == "Spike Growth"||
                        test.playerClass.SubClassName == "Circle of the Land: Arctic" && s.SpellName == "Sleet Storm"||
                        test.playerClass.SubClassName == "Circle of the Land: Arctic" && s.SpellName == "Slow"||
                        test.playerClass.SubClassName == "Circle of the Land: Arctic" && s.SpellName == "Freedom of Movement"||
                        test.playerClass.SubClassName == "Circle of the Land: Arctic" && s.SpellName == "Ice Storm"||
                        test.playerClass.SubClassName == "Circle of the Land: Arctic" && s.SpellName == "Commune with Nature"||
                        test.playerClass.SubClassName == "Circle of the Land: Arctic" && s.SpellName == "Cone of Cold"
                        ){
                            a.AlwaysPrepped = true;
                        };

                    if (test.playerClass.SubClassName == "Circle of the Land: Coast" && s.SpellName == "Mirror Image"||
                        test.playerClass.SubClassName == "Circle of the Land: Coast" && s.SpellName == "Misty Step"||
                        test.playerClass.SubClassName == "Circle of the Land: Coast" && s.SpellName == "Water Breathing"||
                        test.playerClass.SubClassName == "Circle of the Land: Coast" && s.SpellName == "Water Walk"||
                        test.playerClass.SubClassName == "Circle of the Land: Coast" && s.SpellName == "Freedom of Movement"||
                        test.playerClass.SubClassName == "Circle of the Land: Coast" && s.SpellName == "Control Water"||
                        test.playerClass.SubClassName == "Circle of the Land: Coast" && s.SpellName == "Conjure Elemental"||
                        test.playerClass.SubClassName == "Circle of the Land: Coast" && s.SpellName == "Scrying"
                        ){
                            a.AlwaysPrepped = true;
                        };

                    if (test.playerClass.SubClassName == "Circle of the Land: Desert" && s.SpellName == "Blur"||
                        test.playerClass.SubClassName == "Circle of the Land: Desert" && s.SpellName == "Silence"||
                        test.playerClass.SubClassName == "Circle of the Land: Desert" && s.SpellName == "Create Food and Water"||
                        test.playerClass.SubClassName == "Circle of the Land: Desert" && s.SpellName == "Protection from Energy"||
                        test.playerClass.SubClassName == "Circle of the Land: Desert" && s.SpellName == "Blight"||
                        test.playerClass.SubClassName == "Circle of the Land: Desert" && s.SpellName == "Hallucinatory Terrain"||
                        test.playerClass.SubClassName == "Circle of the Land: Desert" && s.SpellName == "Insect Plague"||
                        test.playerClass.SubClassName == "Circle of the Land: Desert" && s.SpellName == "Wall of Stone"
                        ){
                            a.AlwaysPrepped = true;
                        };

                    if (test.playerClass.SubClassName == "Circle of the Land: Forest" && s.SpellName == "Barkskin"||
                        test.playerClass.SubClassName == "Circle of the Land: Forest" && s.SpellName == "Spider Climb"||
                        test.playerClass.SubClassName == "Circle of the Land: Forest" && s.SpellName == "Call Lightning"||
                        test.playerClass.SubClassName == "Circle of the Land: Forest" && s.SpellName == "Plant Growth"||
                        test.playerClass.SubClassName == "Circle of the Land: Forest" && s.SpellName == "Freedom of Movement"||
                        test.playerClass.SubClassName == "Circle of the Land: Forest" && s.SpellName == "Divination"||
                        test.playerClass.SubClassName == "Circle of the Land: Forest" && s.SpellName == "Commune with Nature"||
                        test.playerClass.SubClassName == "Circle of the Land: Forest" && s.SpellName == "Tree Stride"
                        ){
                            a.AlwaysPrepped = true;
                        };
                    
                    if (test.playerClass.SubClassName == "Circle of the Land: Grassland" && s.SpellName == "Invisibilty"||
                        test.playerClass.SubClassName == "Circle of the Land: Grassland" && s.SpellName == "Pass Without Trace"||
                        test.playerClass.SubClassName == "Circle of the Land: Grassland" && s.SpellName == "Daylight"||
                        test.playerClass.SubClassName == "Circle of the Land: Grassland" && s.SpellName == "Haste"||
                        test.playerClass.SubClassName == "Circle of the Land: Grassland" && s.SpellName == "Freedom of Movement"||
                        test.playerClass.SubClassName == "Circle of the Land: Grassland" && s.SpellName == "Divination"||
                        test.playerClass.SubClassName == "Circle of the Land: Grassland" && s.SpellName == "Dream"||
                        test.playerClass.SubClassName == "Circle of the Land: Grassland" && s.SpellName == "Insect Plague"
                        ){
                            a.AlwaysPrepped = true;
                        };

                    if (test.playerClass.SubClassName == "Circle of the Land: Mountain" && s.SpellName == "Spider Climb"||
                        test.playerClass.SubClassName == "Circle of the Land: Mountain" && s.SpellName == "Spike Growth"||
                        test.playerClass.SubClassName == "Circle of the Land: Mountain" && s.SpellName == "Lightning Bolt"||
                        test.playerClass.SubClassName == "Circle of the Land: Mountain" && s.SpellName == "Meld into Stone"||
                        test.playerClass.SubClassName == "Circle of the Land: Mountain" && s.SpellName == "Stone Shape"||
                        test.playerClass.SubClassName == "Circle of the Land: Mountain" && s.SpellName == "Stoneskin"||
                        test.playerClass.SubClassName == "Circle of the Land: Mountain" && s.SpellName == "Passwall"||
                        test.playerClass.SubClassName == "Circle of the Land: Mountain" && s.SpellName == "Wall of Stone"
                        ){
                            a.AlwaysPrepped = true;
                        };

                    if (test.playerClass.SubClassName == "Circle of the Land: Swamp" && s.SpellName == "Acid Arrow"||
                        test.playerClass.SubClassName == "Circle of the Land: Swamp" && s.SpellName == "Darkness"||
                        test.playerClass.SubClassName == "Circle of the Land: Swamp" && s.SpellName == "Water Walk"||
                        test.playerClass.SubClassName == "Circle of the Land: Swamp" && s.SpellName == "Stinking Cloud"||
                        test.playerClass.SubClassName == "Circle of the Land: Swamp" && s.SpellName == "Freedom of Movement"||
                        test.playerClass.SubClassName == "Circle of the Land: Swamp" && s.SpellName == "Locate Creature"||
                        test.playerClass.SubClassName == "Circle of the Land: Swamp" && s.SpellName == "Insect Plague"||
                        test.playerClass.SubClassName == "Circle of the Land: Swamp" && s.SpellName == "Scrying"
                        ){
                            a.AlwaysPrepped = true;
                        };

                    if (test.playerClass.SubClassName == "Life Domain" && s.SpellName == "Bless"||
                        test.playerClass.SubClassName == "Life Domain" && s.SpellName == "Cure Wounds"||
                        test.playerClass.SubClassName == "Life Domain" && s.SpellName == "Lesser Restoration"||
                        test.playerClass.SubClassName == "Life Domain" && s.SpellName == "Spiritual Weapon"||
                        test.playerClass.SubClassName == "Life Domain" && s.SpellName == "Beacon of Hope"||
                        test.playerClass.SubClassName == "Life Domain" && s.SpellName == "Revivify"||
                        test.playerClass.SubClassName == "Life Domain" && s.SpellName == "Death Ward"||
                        test.playerClass.SubClassName == "Life Domain" && s.SpellName == "Guardian of Faith"||
                        test.playerClass.SubClassName == "Life Domain" && s.SpellName == "Mass Cure Wounds"||
                        test.playerClass.SubClassName == "Life Domain" && s.SpellName == "Raise Dead"
                        ){
                            a.AlwaysPrepped = true;
                        };

                    if (test.playerClass.SubClassName == "Oath of Devotion" && s.SpellName == "Protection from Evil & Good"||
                        test.playerClass.SubClassName == "Oath of Devotion" && s.SpellName == "Sanctuary"||
                        test.playerClass.SubClassName == "Oath of Devotion" && s.SpellName == "Lesser Restoration"||
                        test.playerClass.SubClassName == "Oath of Devotion" && s.SpellName == "Zone of Truth"||
                        test.playerClass.SubClassName == "Oath of Devotion" && s.SpellName == "Beacon of Hope"||
                        test.playerClass.SubClassName == "Oath of Devotion" && s.SpellName == "Dispel Magic"||
                        test.playerClass.SubClassName == "Oath of Devotion" && s.SpellName == "Freedom of Movement"||
                        test.playerClass.SubClassName == "Oath of Devotion" && s.SpellName == "Guardian of Faith"||
                        test.playerClass.SubClassName == "Oath of Devotion" && s.SpellName == "Commune"||
                        test.playerClass.SubClassName == "Oath of Devotion" && s.SpellName == "Flame Strike"
                        ){
                            a.AlwaysPrepped = true;
                        };

                    if (test.playerClass.SubClassName == "The Fiend" && s.SpellName == "Burning Hands"||
                        test.playerClass.SubClassName == "The Fiend" && s.SpellName == "Command"||
                        test.playerClass.SubClassName == "The Fiend" && s.SpellName == "Blindness/Deafness"||
                        test.playerClass.SubClassName == "The Fiend" && s.SpellName == "Scorching Ray"||
                        test.playerClass.SubClassName == "The Fiend" && s.SpellName == "Fireball"||
                        test.playerClass.SubClassName == "The Fiend" && s.SpellName == "Stinking Cloud"||
                        test.playerClass.SubClassName == "The Fiend" && s.SpellName == "Fire Shield"||
                        test.playerClass.SubClassName == "The Fiend" && s.SpellName == "Wall of Fire"||
                        test.playerClass.SubClassName == "The Fiend" && s.SpellName == "Flame Strike"||
                        test.playerClass.SubClassName == "The Fiend" && s.SpellName == "Hallow"
                        ){
                            a.AlwaysPrepped = true;
                        };

                        if (test.playerClass.ClassName == "Sorcerer" ||
                            test.playerClass.ClassName == "Warlock" ||
                            test.playerClass.ClassName == "Bard")
                            {
                                a.AlwaysPrepped = true;
                            }
                    
                    _context.Spell_Associations.Add(a);
                    _context.SaveChanges();
                }
                int passiveperception = 10+test.playerStat.Perception;

                // Dynamic model to provide all available objects and data
                dynamic MyModel = new ExpandoObject();
                MyModel.User = userCheck;
                MyModel.Login = new Login();
                MyModel.Character = test; 
                MyModel.Features = Feats;
                MyModel.Spells = Spells;
                MyModel.PassivePerception = passiveperception;
                

                return View("index", MyModel);
            } 

            // if DB is populated with spells, feats, 1 character and 1 user are already present
            else
            {
            // Home screen selects and shows previously generated characters. IF players would like to generate new characters or save them, they must login
            int? SessionId = HttpContext.Session.GetInt32("UserId");
            User SessionUser = _context.Users.FirstOrDefault( u => u.UserId == SessionId);
            // Get list of all Currently generated characters
            List<NewCharacter> CharacterList = _context.NewCharacter.ToList();
            // select random character and gather all stats, BG, Race and class
            int characterNum = rand.Next(1,CharacterList.Count);
            NewCharacter character = _context.NewCharacter.Include( c => c.playerRace).Include( c => c.playerClass).Include(c => c.playerStat).Include(c => c.playerBG).FirstOrDefault( c => c.CharacterId == characterNum);
            // LINQ query all Feature Associations
            List<Feature> Feats = _context.NewCharacter
                .Include(c => c.FeaturesList)
                .ThenInclude(fa => fa.FeatureA)
                .FirstOrDefault(c => c.CharacterId == character.CharacterId)
                .FeaturesList.Select(f => f.FeatureA)
                .OrderBy(f => f.FeatLevel)
                .ToList();

            List<Spell> Spells = _context.NewCharacter
                .Include(c => c.SpellList)
                .ThenInclude(sa => sa.SpellA)
                .FirstOrDefault(c => c.CharacterId == character.CharacterId)
                .SpellList.Select(s => s.SpellA)
                .OrderBy(f => f.SpellLevel)
                .ToList();

            // Pass Model User, Character, Feats
            //passiveperception, because it hates trying to do it on the front end
            int passiveperception = 10+character.playerStat.Perception;

            dynamic MyModel = new ExpandoObject();
            MyModel.User = SessionUser;
            MyModel.Login = new Login();
            MyModel.Character = character;
            MyModel.Features = Feats; 
            MyModel.Spells = Spells;
            MyModel.PassivePerception = passiveperception;
            
            
            
            return View("index", MyModel);
            }
        }
        


        // Seperate generation and saving
        // saving requires userid in session, if not prompt for login register should kick in
        // would be cool if pop up happened so that character doesnt get removed
        [HttpGet("/GenerateNew")]
        public IActionResult CharacterGenerator()
        {
            int? SessionId = HttpContext.Session.GetInt32("UserId");
            User SessionUser = _context.Users.FirstOrDefault( u => u.UserId == SessionId);
            if(SessionUser == null){
                return View("UserLogin");
            }

            Random rand = new Random();
            int Level = rand.Next(1,21);

            //creating input for creation and modification of base in other functions
            PlayerStat playerStat = new PlayerStat(Level);

            // Run player stat through background
            PlayerBG playerBG = new PlayerBG();
            // Returns player Stat VVV
            playerBG.BGSelector(playerStat, playerBG);
            
            // Determines player Race and then run stats
            PlayerRace playerRace = new PlayerRace();
            // Returns player Stat VVV
            playerRace.RaceSelector(Level, playerStat, playerRace);

            // Determine Player Class and run playerStat
            PlayerClass playerClass = new PlayerClass(Level, playerStat);
            // Returns player Stat VVV
            playerClass.ClassSelector(Level, playerStat, playerClass);


            // reruns numbers based on updated proficiencies and skill increases
            playerStat.UpdatePro(playerStat);

            // create connection to all character objects within the character
            NewCharacter newPlayer = new NewCharacter(Level, playerStat,playerRace,playerClass, playerBG, SessionUser);

            _context.PlayerStats.Add(playerStat); 
            _context.PlayerBGs.Add(playerBG); 
            _context.PlayerClasses.Add(playerClass);
            _context.PlayerRaces.Add(playerRace);
            _context.NewCharacter.Add(newPlayer);
            
            // _context.SaveChanges(); //Is this necessary?

            // Feat Generation
            List<Feature> charFeatures = _context.Features.Where(f =>  ((f.FeatSource == playerClass.ClassName) && (f.FeatLevel <= Level)) || ((f.FeatSource == playerClass.SubClassName) && (f.FeatLevel <= Level))).ToList();
            foreach( Feature feat in charFeatures )
            {
                var Fassoc = new FeatureAssoc(newPlayer, feat); 
                _context.Feature_Associations.Add(Fassoc);
                _context.SaveChanges();
            }

            List<Feature> Feats = _context.NewCharacter //Starting Construction from the character side
                        .Include(c => c.FeaturesList) // Including The Association Table List in construction
                        .ThenInclude(fa => fa.FeatureA) //Including items within the Association Table
                        .FirstOrDefault(c => c.CharacterId == newPlayer.CharacterId) // Selecting the character themselves
                        .FeaturesList.Select(f => f.FeatureA) // Selecting the Feats
                        .OrderBy(f => f.FeatLevel) // Ascending order for Feats
                        .ToList();
            
            List<Spell> ClassAvailbleSpells = _context.Spells.Where(s => 
                              s.Source1 == newPlayer.playerClass.ClassName
                            ||s.Source2 == newPlayer.playerClass.ClassName
                            ||s.Source3 == newPlayer.playerClass.ClassName
                            ||s.Source4 == newPlayer.playerClass.ClassName
                            ||s.Source5 == newPlayer.playerClass.ClassName
                            ||s.Source6 == newPlayer.playerClass.ClassName
                            ||s.Source7 == newPlayer.playerClass.ClassName
                            ).ToList();
                            
            List<Spell> NonClassSpells = _context.Spells.Where(s => 
                                s.Source1 != newPlayer.playerClass.ClassName
                            ||s.Source2 != newPlayer.playerClass.ClassName
                            ||s.Source3 != newPlayer.playerClass.ClassName
                            ||s.Source4 != newPlayer.playerClass.ClassName
                            ||s.Source5 != newPlayer.playerClass.ClassName
                            ||s.Source6 != newPlayer.playerClass.ClassName
                            ||s.Source7 != newPlayer.playerClass.ClassName
                            ).ToList(); 
                                      
            Spell FetchSpell = new Spell();
            List<Spell> availableSpells = FetchSpell.GetPossibleSpells(newPlayer, ClassAvailbleSpells, NonClassSpells);
            foreach(Spell s in availableSpells)
            {
                SpellAssoc a = new SpellAssoc(newPlayer, s);//always true
                 if (newPlayer.playerClass.SubClassName == "Circle of the Land: Arctic" && s.SpellName == "Hold Person"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Arctic" && s.SpellName == "Spike Growth"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Arctic" && s.SpellName == "Sleet Storm"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Arctic" && s.SpellName == "Slow"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Arctic" && s.SpellName == "Freedom of Movement"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Arctic" && s.SpellName == "Ice Storm"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Arctic" && s.SpellName == "Commune with Nature"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Arctic" && s.SpellName == "Cone of Cold"
                    ){
                        a.AlwaysPrepped = true;
                    };

                if (newPlayer.playerClass.SubClassName == "Circle of the Land: Coast" && s.SpellName == "Mirror Image"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Coast" && s.SpellName == "Misty Step"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Coast" && s.SpellName == "Water Breathing"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Coast" && s.SpellName == "Water Walk"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Coast" && s.SpellName == "Freedom of Movement"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Coast" && s.SpellName == "Control Water"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Coast" && s.SpellName == "Conjure Elemental"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Coast" && s.SpellName == "Scrying"
                    ){
                        a.AlwaysPrepped = true;
                    };

                if (newPlayer.playerClass.SubClassName == "Circle of the Land: Desert" && s.SpellName == "Blur"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Desert" && s.SpellName == "Silence"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Desert" && s.SpellName == "Create Food and Water"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Desert" && s.SpellName == "Protection from Energy"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Desert" && s.SpellName == "Blight"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Desert" && s.SpellName == "Hallucinatory Terrain"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Desert" && s.SpellName == "Insect Plague"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Desert" && s.SpellName == "Wall of Stone"
                    ){
                        a.AlwaysPrepped = true;
                    };

                if (newPlayer.playerClass.SubClassName == "Circle of the Land: Forest" && s.SpellName == "Barkskin"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Forest" && s.SpellName == "Spider Climb"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Forest" && s.SpellName == "Call Lightning"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Forest" && s.SpellName == "Plant Growth"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Forest" && s.SpellName == "Freedom of Movement"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Forest" && s.SpellName == "Divination"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Forest" && s.SpellName == "Commune with Nature"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Forest" && s.SpellName == "Tree Stride"
                    ){
                        a.AlwaysPrepped = true;
                    };
                
                if (newPlayer.playerClass.SubClassName == "Circle of the Land: Grassland" && s.SpellName == "Invisibilty"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Grassland" && s.SpellName == "Pass Without Trace"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Grassland" && s.SpellName == "Daylight"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Grassland" && s.SpellName == "Haste"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Grassland" && s.SpellName == "Freedom of Movement"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Grassland" && s.SpellName == "Divination"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Grassland" && s.SpellName == "Dream"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Grassland" && s.SpellName == "Insect Plague"
                    ){
                        a.AlwaysPrepped = true;
                    };

                if (newPlayer.playerClass.SubClassName == "Circle of the Land: Mountain" && s.SpellName == "Spider Climb"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Mountain" && s.SpellName == "Spike Growth"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Mountain" && s.SpellName == "Lightning Bolt"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Mountain" && s.SpellName == "Meld into Stone"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Mountain" && s.SpellName == "Stone Shape"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Mountain" && s.SpellName == "Stoneskin"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Mountain" && s.SpellName == "Passwall"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Mountain" && s.SpellName == "Wall of Stone"
                    ){
                        a.AlwaysPrepped = true;
                    };

                if (newPlayer.playerClass.SubClassName == "Circle of the Land: Swamp" && s.SpellName == "Acid Arrow"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Swamp" && s.SpellName == "Darkness"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Swamp" && s.SpellName == "Water Walk"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Swamp" && s.SpellName == "Stinking Cloud"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Swamp" && s.SpellName == "Freedom of Movement"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Swamp" && s.SpellName == "Locate Creature"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Swamp" && s.SpellName == "Insect Plague"||
                    newPlayer.playerClass.SubClassName == "Circle of the Land: Swamp" && s.SpellName == "Scrying"
                    ){
                        a.AlwaysPrepped = true;
                    };

                if (newPlayer.playerClass.SubClassName == "Life Domain" && s.SpellName == "Bless"||
                    newPlayer.playerClass.SubClassName == "Life Domain" && s.SpellName == "Cure Wounds"||
                    newPlayer.playerClass.SubClassName == "Life Domain" && s.SpellName == "Lesser Restoration"||
                    newPlayer.playerClass.SubClassName == "Life Domain" && s.SpellName == "Spiritual Weapon"||
                    newPlayer.playerClass.SubClassName == "Life Domain" && s.SpellName == "Beacon of Hope"||
                    newPlayer.playerClass.SubClassName == "Life Domain" && s.SpellName == "Revivify"||
                    newPlayer.playerClass.SubClassName == "Life Domain" && s.SpellName == "Death Ward"||
                    newPlayer.playerClass.SubClassName == "Life Domain" && s.SpellName == "Guardian of Faith"||
                    newPlayer.playerClass.SubClassName == "Life Domain" && s.SpellName == "Mass Cure Wounds"||
                    newPlayer.playerClass.SubClassName == "Life Domain" && s.SpellName == "Raise Dead"
                    ){
                        a.AlwaysPrepped = true;
                    };

                if (newPlayer.playerClass.SubClassName == "Oath of Devotion" && s.SpellName == "Protection from Evil and Good"||
                    newPlayer.playerClass.SubClassName == "Oath of Devotion" && s.SpellName == "Cure Wounds"||
                    newPlayer.playerClass.SubClassName == "Oath of Devotion" && s.SpellName == "Lesser Restoration"||
                    newPlayer.playerClass.SubClassName == "Oath of Devotion" && s.SpellName == "Spiritual Weapon"||
                    newPlayer.playerClass.SubClassName == "Oath of Devotion" && s.SpellName == "Beacon of Hope"||
                    newPlayer.playerClass.SubClassName == "Oath of Devotion" && s.SpellName == "Revivify"||
                    newPlayer.playerClass.SubClassName == "Oath of Devotion" && s.SpellName == "Death Ward"||
                    newPlayer.playerClass.SubClassName == "Oath of Devotion" && s.SpellName == "Guardian of Faith"||
                    newPlayer.playerClass.SubClassName == "Oath of Devotion" && s.SpellName == "Mass Cure Wounds"||
                    newPlayer.playerClass.SubClassName == "Oath of Devotion" && s.SpellName == "Raise Dead"
                    ){
                        a.AlwaysPrepped = true;
                    };

                if (newPlayer.playerClass.SubClassName == "The Fiend" && s.SpellName == "Burning Hands"||
                        newPlayer.playerClass.SubClassName == "The Fiend" && s.SpellName == "Command"||
                        newPlayer.playerClass.SubClassName == "The Fiend" && s.SpellName == "Blindness/Deafness"||
                        newPlayer.playerClass.SubClassName == "The Fiend" && s.SpellName == "Scorching Ray"||
                        newPlayer.playerClass.SubClassName == "The Fiend" && s.SpellName == "Fireball"||
                        newPlayer.playerClass.SubClassName == "The Fiend" && s.SpellName == "Stinking Cloud"||
                        newPlayer.playerClass.SubClassName == "The Fiend" && s.SpellName == "Fire Shield"||
                        newPlayer.playerClass.SubClassName == "The Fiend" && s.SpellName == "Wall of Fire"||
                        newPlayer.playerClass.SubClassName == "The Fiend" && s.SpellName == "Flame Strike"||
                        newPlayer.playerClass.SubClassName == "The Fiend" && s.SpellName == "Hallow"
                        ){
                            a.AlwaysPrepped = true;
                        };

                    if (newPlayer.playerClass.ClassName == "Sorcerer" ||
                        newPlayer.playerClass.ClassName == "Warlock" ||
                        newPlayer.playerClass.ClassName == "Bard")
                        {
                            a.AlwaysPrepped = true;
                        }
                    
                _context.Spell_Associations.Add(a);
                _context.SaveChanges();
            }

            List<Spell> Spells = _context.NewCharacter
                .Include(c => c.SpellList)
                .ThenInclude(sa => sa.SpellA)
                .FirstOrDefault(c => c.CharacterId == newPlayer.CharacterId)
                .SpellList.Select(s => s.SpellA)
                .OrderBy(f => f.SpellLevel)
                .ToList();

            _context.SaveChanges();

            int passiveperception = 10+newPlayer.playerStat.Perception;

            //Dynamic model with USer, Login, Character
            dynamic MyModel = new ExpandoObject();
            MyModel.User = SessionUser;
            MyModel.Login = new Login();
            MyModel.Character = newPlayer;
            MyModel.Features = Feats;
            MyModel.Spells = Spells;
            MyModel.PassivePerception = passiveperception;
            return View("Index", MyModel);
        }


        [HttpGet("/view/{ID}")]
        public IActionResult ViewCharacter(int ID)
        {

            NewCharacter character = _context.NewCharacter.Include( c => c.playerRace).Include( c => c.playerClass).Include(c => c.playerStat).Include(c => c.playerBG).FirstOrDefault( c => c.CharacterId == ID);

            int? SessionId = HttpContext.Session.GetInt32("UserId");
            ViewBag.SessionId = SessionId;
            User SessionUser = _context.Users.FirstOrDefault( u => u.UserId == SessionId);


            List<Feature> Feats = _context.NewCharacter
                    .Include(c => c.FeaturesList)
                    .ThenInclude(fa => fa.FeatureA)
                    .FirstOrDefault(c => c.CharacterId == character.CharacterId)
                    .FeaturesList.Select(f => f.FeatureA)
                    .OrderBy(f => f.FeatLevel)
                    .ToList();
            
            List<Spell> Spells = _context.NewCharacter
                .Include(c => c.SpellList)
                .ThenInclude(sa => sa.AlwaysPrepped)
                .Include(c => c.SpellList)
                .ThenInclude(sa => sa.Prepped)
                .Include(c => c.SpellList)
                .ThenInclude(sa => sa.SpellA)
                .FirstOrDefault(c => c.CharacterId == character.CharacterId)
                .SpellList.Select(s => s.SpellA)
                .OrderBy(f => f.SpellLevel)
                .ToList();

            dynamic MyModel = new ExpandoObject();
            MyModel.User = SessionUser;
            MyModel.Login = new Login();
            MyModel.Character = character;
            MyModel.Spells = Spells;
            MyModel.Features = Feats;

            return View("Index", MyModel);
        }
        
        // ======================================================================
        // User Login, Registration and Logout

        [HttpPost("/register")]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(_context.Users.Any( u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email is already in use.");
                    return View("UserLogin");
                }
                if(_context.Users.Any( u => u.UserName == newUser.UserName))
                {
                    ModelState.AddModelError("UserName", "User Name is already in use.");
                    return View("UserLogin");
                }
                
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword( newUser, newUser.Password);

                _context.Users.Add(newUser);
                _context.SaveChanges();

                HttpContext.Session.SetInt32("UserId", newUser.UserId);
                int id = newUser.UserId;
                return RedirectToAction("CharacterGenerator");
            }
            return View("UserLogin");
        }

        [HttpPost("/login")]
        public IActionResult Login(Login LoginUser)
        {
            if( LoginUser == null)
            {
                return View("UserLogin");
            }
            else if(ModelState.IsValid)
            {
                var userInDb = _context.Users.FirstOrDefault(u => u.Email == LoginUser.LoginEmail);
                if(userInDb == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("UserLogin");
                }
                else{
                    var hasher = new PasswordHasher<Login>();
                    var result = hasher.VerifyHashedPassword(LoginUser, userInDb.Password, LoginUser.LoginPassword);
                    if(result ==0)
                    {
                        ModelState.AddModelError("Email", "Invalid Email/Password");
                        return View("UserLogin");
                    }
                    else{
                        HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                        return RedirectToAction("CharacterGenerator");
                    }
                }
            }
            return View("UserLogin");
        }

        [HttpGet("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        // Profile Page and Character List
        [HttpGet("/profile/{ID}")]
        public IActionResult Profile(int ID)
        {
            int? SessionId = HttpContext.Session.GetInt32("UserId");
            if(SessionId == null)
                {
                    return View("UserLogin");
                }
            else if (ID == SessionId)
                {
                    User SessionUser = _context.Users.FirstOrDefault( u => u.UserId == ID);
                    List<NewCharacter> Characters = _context.NewCharacter.Where(c => c.UserId == SessionUser.UserId).Include( c => c.playerRace).Include( c => c.playerClass).Include(c => c.playerStat).Include(c => c.playerBG).ToList();
                    dynamic MyModel = new ExpandoObject();
                    MyModel.Characters = Characters;
                    MyModel.User = SessionUser; 
                    return View("profile", MyModel);
                }
            return View("UserLogin");
        }


        // Misc, delete?????
        [HttpGet("/test")]
        public IActionResult Test()
        {
            int? SessionId = HttpContext.Session.GetInt32("UserId");
            ViewBag.SessionId = SessionId;
            User SessionUser = _context.Users.FirstOrDefault( u => u.UserId == SessionId);
            return View ("test");
        }

        [HttpGet("/user")]
        public IActionResult UserLogin()
        {
            int? SessionId = HttpContext.Session.GetInt32("UserId");
            ViewBag.SessionId = SessionId;
            User SessionUser = _context.Users.FirstOrDefault( u => u.UserId == SessionId);
            return View();
        }

    }
}
