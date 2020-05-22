﻿using System;
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
            Feature featTest = _context.Features.FirstOrDefault( f => f.FeatureId == 1);
            if( featTest == null)
            {
                return RedirectToAction("BuildFeatureTable");
            }
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
                _context.PlayerBGs.Add(test.playerBG); //id 1
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
                dynamic MyModel = new ExpandoObject();
                MyModel.User = userCheck;
                MyModel.Login = new Login();
                MyModel.Character = test; 
                
                List<Feature> Feats = _context.NewCharacter
                    .Include(c => c.FeaturesList)
                    .ThenInclude(fa => fa.FeatureA)
                    .FirstOrDefault(c => c.CharacterId == test.CharacterId)
                    .FeaturesList.Select(f => f.FeatureA)
                    .OrderBy(f => f.FeatLevel)
                    .ToList();
            
                 MyModel.Features = Feats;


                return View("index", MyModel);
            } 
            else
            {


            // Home screen selects and shows previously generated characters. IF players would like to generate new characters or save them, they must login
            int? SessionId = HttpContext.Session.GetInt32("UserId");
            ViewBag.SessionId = SessionId;
            User SessionUser = _context.Users.FirstOrDefault( u => u.UserId == SessionId);
            if(SessionUser == null)
            {
                SessionUser = _context.Users.FirstOrDefault( u => u.UserId == 1);
            }
            Random rand = new Random();

            int Level = rand.Next(1,21);

            List<NewCharacter> CharacterList = _context.NewCharacter.ToList();
            int characterNum = rand.Next(1,CharacterList.Count);
            NewCharacter character = _context.NewCharacter.Include( c => c.playerRace).Include( c => c.playerClass).Include(c => c.playerStat).Include(c => c.playerBG).FirstOrDefault( c => c.CharacterId == characterNum);

          
            dynamic MyModel = new ExpandoObject();
            MyModel.User = SessionUser;
            MyModel.Login = new Login();
            MyModel.Character = character;

            List<Feature> Feats = _context.NewCharacter
                .Include(c => c.FeaturesList)
                .ThenInclude(fa => fa.FeatureA)
                .FirstOrDefault(c => c.CharacterId == character.CharacterId)
                .FeaturesList.Select(f => f.FeatureA)
                .OrderBy(f => f.FeatLevel)
                .ToList();
            
            MyModel.Features = Feats; 
            
            return View("index", MyModel);
            }
        }
        


        // Seperate generation and saving
        // saving requires userid in session, if not prompt for login register should kick in
        // would be cool if pop up happened so that character doesnt get removed
        [HttpGet("/class")]
        public IActionResult CharacterGenerator()
        {
            int? SessionId = HttpContext.Session.GetInt32("UserId");
            ViewBag.SessionId = SessionId;
            User SessionUser = _context.Users.FirstOrDefault( u => u.UserId == SessionId);
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


            // Skills/Spells
           

            // create connection to all character objects within the character
            NewCharacter newPlayer = new NewCharacter(Level, playerStat,playerRace,playerClass, playerBG, SessionUser);
            _context.PlayerStats.Add(playerStat); 
            _context.PlayerBGs.Add(playerBG); 
            _context.PlayerClasses.Add(playerClass);
            _context.PlayerRaces.Add(playerRace);
            _context.NewCharacter.Add(newPlayer);
            _context.SaveChanges();

            // Feat Generation
            List<Feature> charFeatures = _context.Features.Where(f =>  ((f.FeatSource == playerClass.ClassName) && (f.FeatLevel <= Level)) || ((f.FeatSource == playerClass.SubClassName) && (f.FeatLevel <= Level))).ToList();
            foreach( Feature feat in charFeatures )
            {
                var Fassoc = new FeatureAssoc(newPlayer, feat); 
                _context.Feature_Associations.Add(Fassoc);
                _context.SaveChanges();
            }


            //Dynamic model with USer, Login, Character
            dynamic MyModel = new ExpandoObject();
            MyModel.User = SessionUser;
            MyModel.Login = new Login();
            MyModel.Character = newPlayer;
            List<Feature> Feats = _context.NewCharacter //Starting Construction from the character side
                        .Include(c => c.FeaturesList) // Including The Association Table List in construction
                        .ThenInclude(fa => fa.FeatureA) //Including items within the Association Table
                        .FirstOrDefault(c => c.CharacterId == newPlayer.CharacterId) // Selecting the character themselves
                        .FeaturesList.Select(f => f.FeatureA) // Selecting the Feats
                        .OrderBy(f => f.FeatLevel) // Ascending order for Feats
                        .ToList();
            
            MyModel.Features = Feats;
            //  _context.Feature_Associations.Where( f => f.CharacterId == newPlayer.CharacterId).Include( f => f.FeatureA).ThenInclude( feat => feat.FeatureName).ToList();

            //LINQ query

            return View("Classes", MyModel);
        }

        // =============================================================================
        // Test for FrontEnd Design
        [HttpGet("/testhtml")]
        public IActionResult TestHtml()
        {
            dynamic MyModel = new ExpandoObject();
            MyModel.User = _context.Users.FirstOrDefault(u => u.UserId == 1);
            MyModel.Character = _context.NewCharacter.FirstOrDefault(c => c.CharacterId == 1); 
            MyModel.Login = new Login();
            
            return View("TestDisplay", MyModel);
        }

        // =============================================================================
        // Other Character Building Routes
        [HttpGet("create/{level}/{classname}/{race}")]
        public IActionResult Specific(int level, string classname, string race)
        {
            Character newchar = new Character(level, classname, race);
            newchar.SecondaryGeneration(newchar); 
            newchar.ThirdGeneration(newchar);
            newchar.FourthGeneration(newchar);
            newchar.FifthGeneration(newchar);
            _context.Characters.Add(newchar);
            _context.SaveChanges();
            return View("Index", newchar);
        }
        
        [HttpGet("/class/{lvl}/{cls}")]
        public IActionResult SpecCharGene(int lvl, string cls)
        {
            int? SessionId = HttpContext.Session.GetInt32("UserId");
            ViewBag.SessionId = SessionId;
            User SessionUser = _context.Users.FirstOrDefault( u => u.UserId == SessionId);
            Random rand = new Random();
            int Level = lvl;

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
            playerClass.SpecClassSelector(Level, playerStat, playerClass, cls);


            // reruns numbers based on updated proficiencies and skill increases
            playerStat.UpdatePro(playerStat);


            // Skills/Spells

            // if (Feature.FeatSource == NewCharacter.PlayerClass.ClassName || Feature.FeatSource == NewCharacter.PlayerClass.SubClassName)
            //     {
            //     if (Feature.FeatLevel <= NewCharacter.Level)
            //         {
            //         NewCharacter.FeatureList.Add(Feature)
            //         }
            //     }


            // create connection to all character objects within the character
            NewCharacter newPlayer = new NewCharacter(Level, playerStat,playerRace,playerClass, playerBG, SessionUser);
            _context.PlayerStats.Add(playerStat);
            _context.PlayerBGs.Add(playerBG);
            _context.PlayerClasses.Add(playerClass);
            _context.PlayerRaces.Add(playerRace);
            _context.NewCharacter.Add(newPlayer);
            _context.SaveChanges();

            //LINQ query

            
            
            return View("Classes",newPlayer);
        }

        [HttpPost("/class/save")]
        public IActionResult SaveCharacter(NewCharacter newPlayer)
        {
            int? SessionId = HttpContext.Session.GetInt32("UserId");
            if(SessionId == null)
                {
                    return View("/login");
                }
            else
                {
                    User SessionUser = _context.Users.FirstOrDefault( u => u.UserId == SessionId);
                    // _context.NewCharacter.userId = SessionId;
                    // _context.NewCharacter.User = SessionUser;
                    _context.SaveChanges();
                    return View("class", newPlayer);
                }

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
            if(ModelState.IsValid)
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
            return Redirect("/");
        }
        
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

        [HttpGet("/view/{ID}")]
        public IActionResult ViewCharacter(int ID)
        {

            NewCharacter character = _context.NewCharacter.Include( c => c.playerRace).Include( c => c.playerClass).Include(c => c.playerStat).Include(c => c.playerBG).FirstOrDefault( c => c.CharacterId == ID);

            int? SessionId = HttpContext.Session.GetInt32("UserId");
            ViewBag.SessionId = SessionId;
            User SessionUser = _context.Users.FirstOrDefault( u => u.UserId == SessionId);

            dynamic MyModel = new ExpandoObject();
            MyModel.User = SessionUser;
            MyModel.Login = new Login();
            MyModel.Character = character;
            List<Feature> Feats = _context.NewCharacter
                    .Include(c => c.FeaturesList)
                    .ThenInclude(fa => fa.FeatureA)
                    .FirstOrDefault(c => c.CharacterId == character.CharacterId)
                    .FeaturesList.Select(f => f.FeatureA)
                    .OrderBy(f => f.FeatLevel)
                    .ToList();
            
            MyModel.Features = Feats;
            return View("classes", MyModel);
        }
        // =============================================================================
        // Feature builder
        [HttpGet("/topsecret")]
        public IActionResult BuildFeatureTable(){
            var searchcheck = _context.Features.FirstOrDefault();
            if (searchcheck != null)
                {
                    return RedirectToAction("Index");
                }
            //Base Class Features
            //Subclass Features
    //Barbarian
            Feature Rage = new Feature("Barbarian", "Rage", 1);
            _context.Features.Add(Rage);
            Feature UnDef = new Feature("Barbarian", "Unarmored Defense (Barbarian)", 1);
            _context.Features.Add(UnDef);
            Feature Reck = new Feature("Barbarian", "Reckless Attack", 2);
            _context.Features.Add(Reck);
            Feature DanSen = new Feature ("Barbarian", "Danger Sense", 2);
            _context.Features.Add(DanSen);
            Feature EA = new Feature("Barbarian", "Extra Attack", 5);
            _context.Features.Add(DanSen);
            Feature fast = new Feature("Barbarian", "Fast Movement", 5);
            _context.Features.Add(EA);
            Feature fin = new Feature ("Barbarian", "Feral Instinct", 7);
            _context.Features.Add(fin);
            Feature crit = new Feature ("Barbarian", "Brutal Critical (1 Die)", 9);
            _context.Features.Add(crit);
            Feature rent = new Feature ("Barbarian", "Relentless Rage", 11);
            _context.Features.Add(rent);
            Feature bruh = new Feature ("Barbarian", "Brutal Critical (2 Dice)", 13);
            _context.Features.Add(bruh);
            Feature pers = new Feature ("Barbarian", "Persistent Rage", 15);
            _context.Features.Add(pers);
            Feature three = new Feature ("Barbarian", "Brutal Critical (3 Dice)", 17);
            _context.Features.Add(three);
            Feature indom = new Feature ("Barbarian", "Indomitable Might", 18);
            _context.Features.Add(indom);
            Feature champ = new Feature ("Barbarian", "Primal Champion", 20);
            _context.Features.Add(champ);
        //Berserker
            Feature ber1 = new Feature ("Path of the Berserker", "Frenzy", 3);
            _context.Features.Add(ber1);
            Feature ber2 = new Feature ("Path of the Berserker", "Mindless Rage", 6);
            _context.Features.Add(ber2);
            Feature ber3 = new Feature ("Path of the Berserker", "Intimidating Presence", 10);
            _context.Features.Add(ber3);
            Feature ber4 = new Feature ("Path of the Berserker", "Retaliation", 14);
            _context.Features.Add(ber4);
        //Totem
            Feature tot1 = new Feature ("Path of the Totem Warrior", "Spirit Seeker", 3);
            _context.Features.Add(tot1);
            Feature tot2 = new Feature ("Path of the Totem Warrior", "Totem Spirit", 3); //*Choices
            _context.Features.Add(tot2);
            Feature tot3 = new Feature ("Path of the Totem Warrior", "Aspect of the Beast", 6); //*Choices
            _context.Features.Add(tot3);
            Feature tot4 = new Feature ("Path of the Totem Warrior", "Spirit Walker", 10);
            _context.Features.Add(tot4);
            Feature tot5 = new Feature ("Path of the Totem Warrior", "Totemic Attunement", 14); //*Choices
            _context.Features.Add(tot5);
    //Bard
            Feature bard0 = new Feature ("Bard", "Bardic Inspiration (d6)", 1);
            _context.Features.Add(bard0);
            Feature bardspc = new Feature ("Bard", "Spellcasting (Bard)", 1);
            _context.Features.Add(bardspc);
            Feature bard2 = new Feature ("Bard", "Jack of All Trades", 2);
            _context.Features.Add(bard2);
            Feature bard22 = new Feature ("Bard", "Song of Rest (d6)", 2);
            _context.Features.Add(bard22);
            Feature bard3 = new Feature ("Bard", "Expertise (2 Skills)", 3);
            _context.Features.Add(bard3);
            Feature bard5 = new Feature ("Bard", "Bardic Inspiration (d8)", 5);
            _context.Features.Add(bard5);
            Feature bard55 = new Feature ("Bard", "Font of Inspiration", 5);
            _context.Features.Add(bard55);
            Feature bard6 = new Feature ("Bard", "Countercharm", 6);
            _context.Features.Add(bard6);
            Feature bard9 = new Feature ("Bard", "Song of Rest (d8)", 9);
            _context.Features.Add(bard9);
            Feature bard10_1 = new Feature("Bard", "Bardic Inspiration (d10)", 10);
            _context.Features.Add(bard10_1);
            Feature bard10_2 = new Feature("Bard", "Magical Secrets (Level 10)", 10);
            _context.Features.Add(bard10_2);
            Feature bard10_3 = new Feature("Bard", "Expertise (2 Skills)", 10);
            _context.Features.Add(bard10_3);
            Feature bard13 = new Feature("Bard", "Song of Rest (d10)", 13);
            _context.Features.Add(bard13);
            Feature bard14_1= new Feature("Bard", "Magical Secrets (Level 14)", 14);
            _context.Features.Add(bard14_1);
            Feature bard15 = new Feature("Bard", "Bardic Inspiration (d12)", 15);
            _context.Features.Add(bard15);
            Feature bard17 = new Feature("Bard", "Song of Rest (d12)", 17);
            _context.Features.Add(bard17);
            Feature bard18 = new Feature("Bard", "Magical Secrets (Level 18)", 18);
            _context.Features.Add(bard18);
            Feature bard20 = new Feature("Bard", "Superior Inspiration", 20);
            _context.Features.Add(bard20);
        //College of Lore
            Feature Lore1 = new Feature ("College of Lore", "Bonus Proficiencies (Any 3 Skills)", 3); //*Gains 3 additional proficiencies
            _context.Features.Add(Lore1);
            Feature Lore2 = new Feature ("College of Lore", "Cutting Words", 3);
            _context.Features.Add(Lore2);
            Feature Lore3 = new Feature ("College of Lore", "Additional Magical Secrets", 6); //*Gains 2 additional spells NOT KNOWN from any list, up to third level
            _context.Features.Add(Lore3);
            Feature Lore4 = new Feature ("College of Lore", "Peerless Skill", 14);
            _context.Features.Add(Lore4);
        //College of Valor
            Feature Valor1 = new Feature ("College of Valor", "Bonus Proficiencies (Medium Armor, Shields, Martial Weapons)", 3); //*Proficiencies is armor/weapons. Should we account for those as well?
            _context.Features.Add(Valor1);
            Feature Valor2 = new Feature ("College of Valor", "Combat Inspiration", 3);
            _context.Features.Add(Valor2);
            Feature Valor3 = new Feature ("College of Valor", "Extra Attack", 6);
            _context.Features.Add(Valor3);
            Feature Valor4 = new Feature ("College of Valor", "Battle Magic", 14);
            _context.Features.Add(Valor4);
    //Cleric
            Feature Cleric1 = new Feature("Cleric", "Spellcasting (Cleric)", 1);
            _context.Features.Add(Cleric1);
            Feature Cleric2 = new Feature("Cleric", "Channel Divinity (1/rest)", 2);
            _context.Features.Add(Cleric2);
            Feature Cleric5 = new Feature("Cleric", "Destroy Undead (CR 1/2)", 5);
            _context.Features.Add(Cleric5);
            Feature Cleric6 = new Feature("Cleric", "Channel Divinity (2/Rest)", 6);
            _context.Features.Add(Cleric6);
            Feature Cleric8 = new Feature("Cleric", "Destroy Undead (CR 1)", 8);
            _context.Features.Add(Cleric6);
            Feature Cleric10 = new Feature("Cleric", "Divine Intervention", 10);
            _context.Features.Add(Cleric10);
            Feature Cleric11 = new Feature("Cleric", "Destroy Undead (CR 2)", 11);
            _context.Features.Add(Cleric11);
            Feature Cleric14 = new Feature("Cleric", "Destroy Undead (CR 3)", 14);
            _context.Features.Add(Cleric14);
            Feature Cleric17 = new Feature("Cleric", "Destroy Undead (CR 4)", 17);
            _context.Features.Add(Cleric17);
            Feature Cleric18 = new Feature("Cleric", "Channel Divinity (3/rest", 18);
            _context.Features.Add(Cleric18);
            Feature Cleric20 = new Feature("Cleric", "Divine Intervention Improvement", 20);
            _context.Features.Add(Cleric20);
        //Knowledge Domain
            Feature Know1 = new Feature ("Knowledge Domain", "Blessings of Knowledge", 1);
            _context.Features.Add(Know1);
            Feature Know2 = new Feature ("Knowledge Domain", "Channel Divinity: Knowledge of the Ages", 2);
            _context.Features.Add(Know2);
            Feature Know3 = new Feature ("Knowledge Domain", "Channel Divinity: Read Thoughts", 6);
            _context.Features.Add(Know3);
            Feature Know4 = new Feature ("Knowledge Domain", "Potent Spellcasting", 8);
            _context.Features.Add(Know4);
            Feature Know5 = new Feature ("Knowledge Domain", "Visions of the Past", 17);
            _context.Features.Add(Know5);
        //Life Domain
            Feature Life1 = new Feature ("Life Domain", "Bonus Proficiency (Heavy Armor)", 1);
            _context.Features.Add(Life1);
            Feature Life2 = new Feature ("Life Domain", "Disciple of Life", 1);
            _context.Features.Add(Life2);
            Feature Life3 = new Feature ("Life Domain", "Channel Divinity: Preserve Life", 2);
            _context.Features.Add(Life3);
            Feature Life4 = new Feature ("Life Domain", "Blessed Healer", 6);
            _context.Features.Add(Life4);
            Feature Life5 = new Feature ("Life Domain", "Divine Strike", 8);
            _context.Features.Add(Life5);
            Feature Life6 = new Feature ("Life Domain", "Supreme Healing", 17);
            _context.Features.Add(Life6);
        //Light Domain
            Feature Light1 = new Feature ("Light Domain", "Bonus Cantrip (Light Cantrip)", 1);
            _context.Features.Add(Light1);
            Feature Light2 = new Feature ("Light Domain", "Warding Flare", 1);
            _context.Features.Add(Light2);
            Feature Light3 = new Feature ("Light Domain", "Channel Divinity: Radiance of the Dawn", 2);
            _context.Features.Add(Light3);
            Feature Light4 = new Feature ("Light Domain", "Improved Flare", 6);
            _context.Features.Add(Light4);
            Feature Light5 = new Feature ("Light Domain", "Potent Spellcasting", 8);
            _context.Features.Add(Light5);
            Feature Light6 = new Feature ("Light Domain", "Corona of Light", 17);
            _context.Features.Add(Light6);
        //Nature Domain
            Feature Nature1 = new Feature ("Nature Domain", "Acolyte of Nature", 1);
            _context.Features.Add(Nature1);
            Feature Nature2 = new Feature ("Nature Domain", "Bonus Proficiency (Heavy Armor)", 1);
            _context.Features.Add(Nature2);
            Feature Nature3 = new Feature ("Nature Domain", "Channel Divinity: Charm Animals and Plants", 2);
            _context.Features.Add(Nature3);
            Feature Nature4 = new Feature ("Nature Domain", "Dampen Elements", 6);
            _context.Features.Add(Nature4);
            Feature Nature5 = new Feature ("Nature Domain", "Divine Strike", 8);
            _context.Features.Add(Nature5);
            Feature Nature6 = new Feature ("Nature Domain", "Master of Nature", 17);
            _context.Features.Add(Nature6);
        //Tempest Domain
            Feature Tempest1 = new Feature ("Tempest Domain", "Bonus Proficiency (Martial Weapons, Heavy Armor)", 1);
            _context.Features.Add(Tempest1);
            Feature Tempest2 = new Feature ("Tempest Domain", "Wrath of the Storm", 1);
            _context.Features.Add(Tempest2);
            Feature Tempest3 = new Feature ("Tempest Domain", "Channel Divinity: Destructive Wrath", 2);
            _context.Features.Add(Tempest3);
            Feature Tempest4 = new Feature ("Tempest Domain", "Thunderbolt Strike", 6);
            _context.Features.Add(Tempest4);
            Feature Tempest5 = new Feature ("Tempest Domain", "Divine Strike", 8);
            _context.Features.Add(Tempest5);
            Feature Tempest6 = new Feature ("Tempest Domain", "Stormborn", 17);
            _context.Features.Add(Tempest6);
        //Trickery Domain
            Feature Trickery1 = new Feature ("Trickery Domain", "Blessing of the Trickster", 1);
            _context.Features.Add(Trickery1);
            Feature Trickery2 = new Feature ("Trickery Domain", "Channel Divinity: Invoke Duplicity", 2);
            _context.Features.Add(Trickery2);
            Feature Trickery3 = new Feature ("Trickery Domain", "Channel Divinity: Cloak of Shadows", 6);
            _context.Features.Add(Trickery3);
            Feature Trickery4 = new Feature ("Trickery Domain", "Divine Strike", 8);
            _context.Features.Add(Trickery4);
            Feature Trickery5 = new Feature ("Trickery Domain", "Improved Duplicity", 17);
            _context.Features.Add(Trickery5);
        //War Domain
            Feature War1 = new Feature ("War Domain", "Bonus Proficiency (Martial Weapons, Heavy Armor)", 1);
            _context.Features.Add(War1);
            Feature War2 = new Feature ("War Domain", "War Priest", 1);
            _context.Features.Add(War2);
            Feature War3 = new Feature ("War Domain", "Channel Divinity: Guided Strike", 2);
            _context.Features.Add(War3);
            Feature War4 = new Feature ("War Domain", "Channel Divinity: War God's Blessing", 6);
            _context.Features.Add(War4);
            Feature War5 = new Feature ("War Domain", "Divine Strike", 8);
            _context.Features.Add(War5);
            Feature War6 = new Feature ("War Domain", "Avatar of Battle", 17);
            _context.Features.Add(War6);
    //Druid
            Feature Druid1 = new Feature("Druid", "Spellcasting (Druid)", 1);
            _context.Features.Add(Druid1);
            Feature Druid1_1 = new Feature("Druid", "Druidic", 1);
            _context.Features.Add(Druid1_1);
            Feature Druid2 = new Feature("Druid", "Wild Shape", 2);
            _context.Features.Add(Druid2);
            Feature Druid4 = new Feature("Druid", "Wild Shape Improvement (Level 4)", 4);
            _context.Features.Add(Druid4);
            Feature Druid8 = new Feature("Druid", "Wild Shape Improvement(Level 8)", 8);
            _context.Features.Add(Druid8);
            Feature Druid18 = new Feature("Druid", "Timeless Body", 18);
            _context.Features.Add(Druid18);
            Feature Druid18_1 = new Feature("Druid", "Beast Spells", 18);
            _context.Features.Add(Druid18_1);
            Feature Druid20 = new Feature("Druid", "Archdruid", 20);
            _context.Features.Add(Druid20);
        //Circle of the Land - *If we can do a Linq Query for Contains on Circle of the Land, we can add all of these, I think. Then, when adding spells, we can do a similar function for the Land keywords (e.g. Arctic, Grassland, etc.)
            Feature Land1 = new Feature ("Circle of the Land", "Bonus Cantrip (Any Druid Cantrip)", 2);
            _context.Features.Add(Land1);
            Feature Land2 = new Feature ("Circle of the Land", "Natural Recovery", 2);
            _context.Features.Add(Land2);
            Feature Land3 = new Feature ("Circle of the Land", "Land's Stride", 6);
            _context.Features.Add(Land3);
            Feature Land4 = new Feature ("Circle of the Land", "Nature's Ward", 10);
            _context.Features.Add(Land4);
            Feature Land5 = new Feature ("Circle of the Land", "Nature's Sanctuary", 14);
            _context.Features.Add(Land5);
        //Circle of the Moon
            Feature Moon1 = new Feature ("Circle of the Moon", "Combat Wild Shape", 2);
            _context.Features.Add(Moon1);
            Feature Moon2 = new Feature ("Circle of the Moon", "Circle Forms", 2);
            _context.Features.Add(Moon2);
            Feature Moon3 = new Feature ("Circle of the Moon", "Primal Strike", 6);
            _context.Features.Add(Moon3);
            Feature Moon4 = new Feature ("Circle of the Moon", "Elemental Wild Shape", 10);
            _context.Features.Add(Moon4);
            Feature Moon5 = new Feature ("Circle of the Moon", "Thousand Forms", 14);
            _context.Features.Add(Moon5);
    //Fighter
            Feature Fighter1 = new Feature("Fighter", "Fighting Style", 1);
            _context.Features.Add(Fighter1); //Need to implement randomization for Fighting Style choice
            Feature Fighter1_1 = new Feature("Fighter", "Second Wind", 1);
            _context.Features.Add(Fighter1_1);
            Feature Fighter2 = new Feature("Fighter", "Action Surge", 2);
            _context.Features.Add(Fighter2);
            Feature Fighter5 = new Feature("Fighter", "Extra Attack", 5);
            _context.Features.Add(Fighter5);
            Feature Fighter9 = new Feature("Fighter", "Indomitable (1 Use)", 9);
            _context.Features.Add(Fighter9);
            Feature Fighter11 = new Feature("Fighter", "Extra Attack (2)", 11);
            _context.Features.Add(Fighter11);
            Feature Fighter13 = new Feature("Fighter", "Indomitable (2 Use)", 13);
            _context.Features.Add(Fighter13);
            Feature Fighter17 = new Feature("Fighter", "Action Surge (2 Uses)", 17);
            _context.Features.Add(Fighter17);
            Feature Fighter17_1 = new Feature("Fighter", "Indomitable (3 Uses)", 17);
            _context.Features.Add(Fighter17_1);
            Feature Fighter20 = new Feature("Fighter", "Extra Attack (3)", 20);
            _context.Features.Add(Fighter20);
        //Champion
            Feature champ1 = new Feature("Champion", "Improved Critical", 3);
            _context.Features.Add(champ1);
            Feature champ2 = new Feature("Champion", "Remarkable Athlete", 7);
            _context.Features.Add(champ2);
            Feature champ3 = new Feature("Champion", "Additional Fighting Style", 10);
            _context.Features.Add(champ3);
            Feature champ4 = new Feature("Champion", "Superior Critical Critical", 15);
            _context.Features.Add(champ4);
            Feature champ5 = new Feature("Champion", "Survivor", 18);
            _context.Features.Add(champ5);
        //Battle Master
            Feature bm1 = new Feature("Battle Master", "Combat Superiority", 3);
            _context.Features.Add(bm1);
            Feature bm2 = new Feature("Battle Master", "Combat Student of War", 3);
            _context.Features.Add(bm2);
            Feature bm3 = new Feature("Battle Master", "Student of War", 7);
            _context.Features.Add(bm3);
            Feature bm4 = new Feature("Battle Master", "Improved Combat Superiority", 10);
            _context.Features.Add(bm4);
            Feature bm5 = new Feature("Battle Master", "Relentless", 15);
            _context.Features.Add(bm5);
        //Eldritch Knight
            Feature ek1 = new Feature("Eldritch Knight", "Spellcasting (Eldritch Knight)", 3);
            _context.Features.Add(ek1);
            Feature ek2 = new Feature("Eldritch Knight", "War Magic", 7);
            _context.Features.Add(ek2);
            Feature ek3 = new Feature("Eldritch Knight", "Eldritch Strike", 10);
            _context.Features.Add(ek3);
            Feature ek4 = new Feature("Eldritch Knight", "Arcane Charge", 15);
            _context.Features.Add(ek4);
            Feature ek5 = new Feature("Eldritch Knight", "Improved War Magic", 18);
            _context.Features.Add(ek5);
    //Monk
            Feature Monk1 = new Feature("Monk", "Unarmored Defense (Monk)", 1);
            _context.Features.Add(Monk1);
            Feature Monk1_1 = new Feature("Monk", "Martial Arts", 1);
            _context.Features.Add(Monk1_1);
            Feature Monk2 = new Feature("Monk", "Ki", 2);
            _context.Features.Add(Monk2);
            Feature Monk2_2 = new Feature("Monk", "Unarmored Movement", 2);
            _context.Features.Add(Monk2_2);
            Feature Monk3 = new Feature("Monk", "Deflect Missiles", 3);
            _context.Features.Add(Monk3);
            Feature Monk4 = new Feature("Monk", "Slow Fall", 4);
            _context.Features.Add(Monk4);
            Feature Monk5 = new Feature("Monk", "Extra Attack", 5);
            _context.Features.Add(Monk5);
            Feature Monk5_5 = new Feature("Monk", "Stunning Strike", 5);
            _context.Features.Add(Monk5_5);
            Feature Monk6 = new Feature("Monk", "Ki-Empowered Strikes", 6);
            _context.Features.Add(Monk6);
            Feature Monk7 = new Feature("Monk", "Evasion", 7);
            _context.Features.Add(Monk7);
            Feature Monk7_1 = new Feature("Monk", "Stillness of Mind", 7);
            _context.Features.Add(Monk7_1);
            Feature Monk9 = new Feature("Monk", "Unarmored Movement Improvement", 9);
            _context.Features.Add(Monk9);
            Feature Monk10 = new Feature("Monk", "Purity of Body", 10);
            _context.Features.Add(Monk10);
            Feature Monk13 = new Feature("Monk", "Tongue of the Sun and Moon", 1);
            _context.Features.Add(Monk13);
            Feature Monk14 = new Feature("Monk", "Diamond Soul", 14);
            _context.Features.Add(Monk1);
            Feature Monk15 = new Feature ("Monk", "Timeless Body", 15);
            _context.Features.Add(Monk15);
            Feature Monk18 = new Feature ("Monk", "Empty Body", 18);
            _context.Features.Add(Monk18);
            Feature Monk20 = new Feature ("Monk", "Perfect Self", 20);
            _context.Features.Add(Monk20);
        //Way of the Open Hand
            Feature openhand1 = new Feature ("Way of the Open Hand", "Open Hand Technique", 3);
            _context.Features.Add(openhand1);
            Feature openhand2 = new Feature ("Way of the Open Hand", "Wholeness of Body", 6);
            _context.Features.Add(openhand2);
            Feature openhand3 = new Feature ("Way of the Open Hand", "Tranquility", 11);
            _context.Features.Add(openhand3);
            Feature openhand4 = new Feature ("Way of the Open Hand", "Quivering Palm", 17);
            _context.Features.Add(openhand4);
        //Way of the Four Elements
            Feature avatar1 = new Feature ("Way of the Four Elements", "Disciple of the Elements", 3);
            _context.Features.Add(avatar1); //*Requires detail, goes into a number of potential options
        //Way of Shadow
            Feature shadow1 = new Feature ("Way of Shadow", "Shadow Arts", 3);
            _context.Features.Add(shadow1);
            Feature shadow2 = new Feature ("Way of Shadow", "Shadow Step", 6);
            _context.Features.Add(shadow2);
            Feature shadow3 = new Feature ("Way of Shadow", "Cloak of Shadows", 11);
            _context.Features.Add(shadow3);
            Feature shadow4 = new Feature ("Way of Shadow", "Opportunist", 17);
            _context.Features.Add(shadow4);
    //Paladin
            Feature Pally0 = new Feature ("Paladin", "Divine Sense", 1);
            _context.Features.Add(Pally0);
            Feature Pally1 = new Feature ("Paladin", "Lay on Hands", 1);
            _context.Features.Add(Pally1);
            Feature Pally2 = new Feature ("Paladin", "Fighting Style", 2);
            _context.Features.Add(Pally2);
            Feature Pally2_2 = new Feature ("Paladin", "Spellcasting (Paladin)", 2);
            _context.Features.Add(Pally2_2);
            Feature Pally2_3 = new Feature ("Paladin", "Divine Smite", 2);
            _context.Features.Add(Pally2_3);
            Feature Pally5 = new Feature ("Paladin", "Divine Smite", 5);
            _context.Features.Add(Pally5);
            Feature Pally10 = new Feature ("Paladin", "Aura of Courage", 10);
            _context.Features.Add(Pally10);
            Feature Pally11 = new Feature ("Paladin", "Improved Divine Smite", 11);
            _context.Features.Add(Pally11);
            Feature Pally14 = new Feature ("Paladin", "Cleansing Touch", 14);
            _context.Features.Add(Pally14);
            Feature Pally18 = new Feature ("Paladin", "Aura Improvements", 18);
            _context.Features.Add(Pally18);
        //Oath of Devotion
            Feature OoD1 = new Feature ("Oath of Devotion", "Channel Divinity: Sacred Weapon/Turn the Unholy", 3);
            _context.Features.Add(OoD1);
            Feature OoD2 = new Feature ("Oath of Devotion", "Aura of Devotion", 7);
            _context.Features.Add(OoD2);
            Feature OoD3 = new Feature ("Oath of Devotion", "Purity of Spirit", 15);
            _context.Features.Add(OoD3);
            Feature OoD4 = new Feature ("Oath of Devotion", "Holy Nimbus", 20);
            _context.Features.Add(OoD4);
        //Oath of Vengeance
            Feature OoV1 = new Feature ("Oath of Vengeance", "Channel Divinity: Abjure Enemy/Vow of Enmity", 3);
            _context.Features.Add(OoV1);
            Feature OoV2 = new Feature ("Oath of Vengeance", "Relentless Avenger", 7);
            _context.Features.Add(OoV2);
            Feature OoV3 = new Feature ("Oath of Vengeance", "Soul of Vengeance", 15);
            _context.Features.Add(OoV3);
            Feature OoV4 = new Feature ("Oath of Vengeance", "Avenging Angel", 20);
            _context.Features.Add(OoV4);
        //Oath of the Ancients
            Feature OotA1 = new Feature ("Oath of the Ancients", "Channel Divinity: Nature's Wrath/Turn the Faithless", 3);
            _context.Features.Add(OotA1);
            Feature OotA2 = new Feature ("Oath of the Ancients", "Aura of Warding", 7);
            _context.Features.Add(OotA2);
            Feature OotA3 = new Feature ("Oath of the Ancients", "Undying Sentinel", 15);
            _context.Features.Add(OotA3);
            Feature OotA4 = new Feature ("Oath of the Ancients", "Elder Champion", 20);
            _context.Features.Add(OotA4);
    //Ranger
            Feature rngr1 = new Feature ("Ranger", "Favored Enemy", 1);
            _context.Features.Add(rngr1); //Need to expand on this, as it covers a number of possible options
            Feature rngr1_1 = new Feature ("Ranger", "Natural Explorer", 1);
            _context.Features.Add(rngr1_1);
            Feature rngr2 = new Feature ("Ranger", "Fighting Style", 2);
            _context.Features.Add(rngr2); //Reqs expansion, similar to other Fighting Style options
            Feature rngr2_2 = new Feature ("Ranger", "Spellcasting (Ranger)", 2);
            _context.Features.Add(rngr2_2);
            Feature rngr3 = new Feature ("Ranger", "Primeval Awareness", 3);
            _context.Features.Add(rngr3);
            Feature rngr5 = new Feature ("Ranger", "Extra Attack", 5);
            _context.Features.Add(rngr5);
            Feature rngr6 = new Feature ("Ranger", "Favored Enemy and Natural Explorer Improvements", 6);
            _context.Features.Add(rngr6);
            Feature rngr10 = new Feature ("Ranger", "Natural Explorer Improvement", 10);
            _context.Features.Add(rngr10);
            Feature rngr10_1 = new Feature ("Ranger", "Hide in Plain Sight", 10);
            _context.Features.Add(rngr10_1);
            Feature rngr14 = new Feature ("Ranger", "Favored Enemy Improvement (Level 14)", 14);
            _context.Features.Add(rngr14);
            Feature rngr14_1 = new Feature ("Ranger", "Vanish", 14);
            _context.Features.Add(rngr14_1);
            Feature rngr18 = new Feature ("Ranger", "Feral Senses", 18);
            _context.Features.Add(rngr18);
            Feature rngr20 = new Feature ("Ranger", "Foe Slayer", 20);
            _context.Features.Add(rngr20);
        //Hunter
            Feature hunt1 = new Feature ("Hunter", "Hunter's Prey", 3); //*Choice
            _context.Features.Add(hunt1);
            Feature hunt2 = new Feature ("Hunter", "Defensive Tactics", 7); //*Choice
            _context.Features.Add(hunt2);
            Feature hunt3 = new Feature ("Hunter", "Multiattack", 11); //*Choice\
            _context.Features.Add(hunt3);
            Feature hunt4 = new Feature ("Hunter", "Superior Hunter's Defense", 15); //*Choice
            _context.Features.Add(hunt4);
        //Beast Master
            Feature BM1 = new Feature ("Beast Master", "Ranger's Companion", 3);
            _context.Features.Add(BM1);
            Feature BM2 = new Feature ("Beast Master", "Exceptional Training", 7);
            _context.Features.Add(BM2);
            Feature BM3 = new Feature ("Beast Master", "Bestial Fury", 11);
            _context.Features.Add(BM3);
            Feature BM4 = new Feature ("Beast Master", "Share Spells", 15);
            _context.Features.Add(BM4);
    //Rogue
            Feature rg1 = new Feature ("Rogue", "Expertise", 1);
            _context.Features.Add(rg1);
            Feature rg1_1 = new Feature ("Rogue", "Sneak Attack", 1);
            _context.Features.Add(rg1_1);
            Feature rg1_2 = new Feature ("Rogue", "Thieves Cant", 1);
            _context.Features.Add(rg1_2);
            Feature rg2 = new Feature ("Rogue", "Cunning Action", 2);
            _context.Features.Add(rg2);
            Feature rg5 = new Feature ("Rogue", "Uncanny Dodge", 5);
            _context.Features.Add(rg5);
            Feature rg6 = new Feature ("Rogue", "Expertise", 6);
            _context.Features.Add(rg6);
            Feature rg7 = new Feature ("Rogue", "Evasion", 7);
            _context.Features.Add(rg7);
            Feature rg11 = new Feature ("Rogue", "Reliable Talent", 11);
            _context.Features.Add(rg11);
            Feature rg14 = new Feature ("Rogue", "Blindsense", 14);
            _context.Features.Add(rg14);
            Feature rg15 = new Feature ("Rogue", "Slippery Mind", 15);
            _context.Features.Add(rg15);
            Feature rg18 = new Feature ("Rogue", "Elusive", 18);
            _context.Features.Add(rg18);
            Feature rg20 = new Feature ("Rogue", "Stroke of Luck", 20);
            _context.Features.Add(rg20);
        //Thief
            Feature thf1 = new Feature ("Thief", "Fast Hands", 3);
            _context.Features.Add(thf1);
            Feature thf2 = new Feature ("Thief", "Second-Story Work", 3);
            _context.Features.Add(thf2);
            Feature thf3 = new Feature ("Thief", "Supreme Sneak", 9);
            _context.Features.Add(thf3);
            Feature thf4 = new Feature ("Thief", "Use Magic Device", 13);
            _context.Features.Add(thf4);
            Feature thf5 = new Feature ("Thief", "Thief's Reflexes", 17);
            _context.Features.Add(thf5);
        //Assassin
            Feature assassin1 = new Feature ("Assassin", "Bonus Proficiencies (Disguise Kit, Poisoner's Kit)", 3);
            _context.Features.Add(assassin1);
            Feature assassin2 = new Feature ("Assassin", "Assassinate", 3);
            _context.Features.Add(assassin2);
            Feature assassin3 = new Feature ("Assassin", "Infiltration Expertise", 9);
            _context.Features.Add(assassin3);
            Feature assassin4 = new Feature ("Assassin", "Imposter", 13);
            _context.Features.Add(assassin4);
            Feature assassin5 = new Feature ("Assassin", "Death Strike", 17);
            _context.Features.Add(assassin5);
        //Arcane Trickster
            Feature AT1 = new Feature ("Arcane Trickster", "Spellcasting (Arcane Trickster)", 3);
            _context.Features.Add(AT1);
            Feature AT2 = new Feature ("Arcane Trickster", "Mage Hand Legerdemain", 3);
            _context.Features.Add(AT2);
            Feature AT3 = new Feature ("Arcane Trickster", "Magical Ambush", 9);
            _context.Features.Add(AT3);
            Feature AT4 = new Feature ("Arcane Trickster", "Versatile Trickster", 13);
            _context.Features.Add(AT4);
            Feature AT5 = new Feature ("Arcane Trickster", "Spell Thief", 17);
            _context.Features.Add(AT5);
    //Sorcerer
            Feature sorc1 = new Feature ("Sorcerer", "Spellcasting (Sorcerer)", 1);
            _context.Features.Add(sorc1);
            Feature sorc2 = new Feature ("Sorcerer", "Font of Magic", 2);
            _context.Features.Add(sorc2);
            Feature sorc3 = new Feature ("Sorcerer", "Metamagic (Level 3)", 3); //Have to generate options
            _context.Features.Add(sorc3);
            Feature sorc10 = new Feature ("Sorcerer", "Metamagic (Level 10)", 10);
            _context.Features.Add(sorc10);
            Feature sorc17 = new Feature ("Sorcerer", "Metamagic (Level 17)", 17);
            _context.Features.Add(sorc17);
            Feature sorc20 = new Feature ("Sorcerer", "Sorcerous Restoration", 20);
            _context.Features.Add(sorc20);
        //Draconic Bloodline
            Feature DB1 = new Feature ("Draconic Bloodline", "Dragon Ancestor", 1);//*Choice
            _context.Features.Add(DB1);
            Feature DB2 = new Feature ("Draconic Bloodline", "Draconic Resilience", 1);
            _context.Features.Add(DB2);
            Feature DB3 = new Feature ("Draconic Bloodline", "Elemental Affinity", 6); //Affected by Dragon Ancestor Choice
            _context.Features.Add(DB3);
            Feature DB4 = new Feature ("Draconic Bloodline", "Dragon Wings", 14);
            _context.Features.Add(DB4);
            Feature DB5 = new Feature ("Draconic Bloodline", "Draconic Presence", 18);
            _context.Features.Add(DB5);
        //Wild Magic
            Feature WM1 = new Feature ("Wild Magic", "Wild Magic Surge", 1);
            _context.Features.Add(WM1);
            Feature WM2 = new Feature ("Wild Magic", "Tides of Chaos", 1);
            _context.Features.Add(WM2);
            Feature WM3 = new Feature ("Wild Magic", "Bend Luck", 6);
            _context.Features.Add(WM3);
            Feature WM4 = new Feature ("Wild Magic", "Controlled Chaos", 14);
            _context.Features.Add(WM4);
            Feature WM5 = new Feature ("Wild Magic", "Spell Bombardment", 18);
            _context.Features.Add(WM5);
    //Warlock
            Feature lock0 = new Feature ("Warlock", "Pact Magic", 1);
            _context.Features.Add(lock0);
            Feature lock2 = new Feature ("Warlock", "Eldritch Invocations", 2); //Need to expand based on Invocations
            _context.Features.Add(lock2);
            Feature lock3 = new Feature ("Warlock", "Pact Boon", 3); // Need to expand on this to generate options, probably list each pact boon under this and when Pact boon is recieved, add one of the options to the list
            _context.Features.Add(lock3);
            Feature lock11 = new Feature ("Warlock", "Mystic Arcanum (Level 6)", 11);
            _context.Features.Add(lock11);
            Feature lock13 = new Feature ("Warlock", "Mystic Arcanum (Level 7)", 13);
            _context.Features.Add(lock13);
            Feature lock15 = new Feature ("Warlock", "Mystic Arcanum (Level 8)", 15);
            _context.Features.Add(lock15);
            Feature lock17 = new Feature ("Warlock", "Mystic Arcanum (Level 9)", 17);
            _context.Features.Add(lock17);
            Feature lock20 = new Feature ("Warlock", "Eldritch Master", 20);
            _context.Features.Add(lock20);
        //The Archfey
            Feature af1 = new Feature ("The Archfey", "Fey Presence", 1);
            _context.Features.Add(af1);
            Feature af2 = new Feature ("The Archfey", "Misty Escape", 6);
            _context.Features.Add(af2);
            Feature af3 = new Feature ("The Archfey", "Beguiling Defenses", 10);
            _context.Features.Add(af3);
            Feature af4 = new Feature ("The Archfey", "Dark Delirium", 14);
            _context.Features.Add(af4);
        //The Great Old One
            Feature GOO1 = new Feature ("The Great Old One", "Awakened Mind", 1);
            _context.Features.Add(GOO1);
            Feature GOO2 = new Feature ("The Great Old One", "Entropic Ward", 6);
            _context.Features.Add(GOO2);
            Feature GOO3 = new Feature ("The Great Old One", "Thought Shield", 10);
            _context.Features.Add(GOO3);
            Feature GOO4 = new Feature ("The Great Old One", "Create Thrall", 14);
            _context.Features.Add(GOO4);
        //The Fiend
            Feature fnd1 = new Feature ("The Fiend", "Dark One's Blessing", 1);
            _context.Features.Add(fnd1);
            Feature fnd2 = new Feature ("The Fiend", "Dark One's Own Luck", 6);
            _context.Features.Add(fnd2);
            Feature fnd3 = new Feature ("The Fiend", "Fiendish Resilience", 10);
            _context.Features.Add(fnd3);
            Feature fnd4 = new Feature ("The Fiend", "Hurl Through Hell", 14);
            _context.Features.Add(fnd4);
    //Wizard
            Feature Wiz1 = new Feature ("Wizard", "Spellcasting (Wizard)", 1);
            _context.Features.Add(Wiz1);
            Feature Wiz1_1 = new Feature ("Wizard", "Arcane Recovery", 1);
            _context.Features.Add(Wiz1_1);
            Feature Wiz18 = new Feature ("Wizard", "Spell Mastery", 18);
            _context.Features.Add(Wiz18);
            Feature Wiz20 = new Feature ("Wizard", "Signature Spell", 20);
            _context.Features.Add(Wiz20);
        //School of Abjuration
            Feature abj1 = new Feature ("School of Abjuration", "Abjuration Savant", 2);
            _context.Features.Add(abj1);
            Feature abj2 = new Feature ("School of Abjuration", "Arcane Ward", 2);
            _context.Features.Add(abj2);
            Feature abj3 = new Feature ("School of Abjuration", "Projected Ward", 6);
            _context.Features.Add(abj3);
            Feature abj4 = new Feature ("School of Abjuration", "Improved Abjuration", 10);
            _context.Features.Add(abj4);
            Feature abj5 = new Feature ("School of Abjuration", "Spell Resistance", 14);
            _context.Features.Add(abj5);
        //School of Conjuration
            Feature conj1 = new Feature ("School of Conjuration", "Conjuration Savant", 2);
            _context.Features.Add(conj1);
            Feature conj2 = new Feature ("School of Conjuration", "Minor Conjuration", 2);
            _context.Features.Add(conj2);
            Feature conj3 = new Feature ("School of Conjuration", "Benign Transposition", 6);
            _context.Features.Add(conj3);
            Feature conj4 = new Feature ("School of Conjuration", "Focused Conjuration", 10);
            _context.Features.Add(conj4);
            Feature conj5 = new Feature ("School of Conjuration", "Durable Summons", 14);
            _context.Features.Add(conj5);
        //School of Divination
            Feature div1 = new Feature ("School of Divination", "Divination Savant", 2);
            _context.Features.Add(div1);
            Feature div2 = new Feature ("School of Divination", "Portent", 2);
            _context.Features.Add(div2);
            Feature div3 = new Feature ("School of Divination", "Expert Divination", 6);
            _context.Features.Add(div3);
            Feature div4 = new Feature ("School of Divination", "The Third Eye", 10);
            _context.Features.Add(div4);
            Feature div5 = new Feature ("School of Divination", "Greater Portent", 14);
            _context.Features.Add(div5);
        //School of Enchantment
            Feature enc1 = new Feature ("School of Enchantment", "Enchantment Savant", 2);
            _context.Features.Add(enc1);
            Feature enc2 = new Feature ("School of Enchantment", "Hypnotic Gaze", 2);
            _context.Features.Add(enc2);
            Feature enc3 = new Feature ("School of Enchantment", "Instinctive Charm", 6);
            _context.Features.Add(enc3);
            Feature enc4 = new Feature ("School of Enchantment", "Split Enchantment", 10);
            _context.Features.Add(enc4);
            Feature enc5 = new Feature ("School of Enchantment", "Alter Memories", 14);
            _context.Features.Add(enc5);
        //School of Evocation
            Feature evo1 = new Feature ("School of Evocation", "Evocation Savant", 2);
            _context.Features.Add(evo1);
            Feature evo2 = new Feature ("School of Evocation", "Sculpt Spells", 2);
            _context.Features.Add(evo2);
            Feature evo3 = new Feature ("School of Evocation", "Potent Cantrip", 6);
            _context.Features.Add(evo3);
            Feature evo4 = new Feature ("School of Evocation", "Empowered Evocation", 10);
            _context.Features.Add(evo4);
            Feature evo5 = new Feature ("School of Evocation", "Overchannel", 14);
            _context.Features.Add(evo5);
        //School of Illusion
            Feature illu1 = new Feature ("School of Illusion", "Illusion Savant", 2);
            _context.Features.Add(illu1);
            Feature illu2 = new Feature ("School of Illusion", "Improved Minor Illusion", 2);
            _context.Features.Add(illu2);
            Feature illu3 = new Feature ("School of Illusion", "Malleable Illusion", 6);
            _context.Features.Add(illu3);
            Feature illu4 = new Feature ("School of Illusion", "Illusory Self", 10);
            _context.Features.Add(illu4);
            Feature illu5 = new Feature ("School of Illusion", "Illusory Reality", 14);
            _context.Features.Add(illu5);
        //School of Necromancy
            Feature necro1 = new Feature ("School of Necromancy", "Necromancy Savant", 2);
            _context.Features.Add(necro1);
            Feature necro2 = new Feature ("School of Necromancy", "Grim Harvest", 2);
            _context.Features.Add(necro2);
            Feature necro3 = new Feature ("School of Necromancy", "Undead Thralls", 6);
            _context.Features.Add(necro3);
            Feature necro4 = new Feature ("School of Necromancy", "Inured to Death", 10);
            _context.Features.Add(necro4);
            Feature necro5 = new Feature ("School of Necromancy", "Command Undead", 14);
            _context.Features.Add(necro5);
        //School of Transmutation
            Feature trans1 = new Feature ("School of Transmutation", "Transmutation Savant", 2);
            _context.Features.Add(trans1);
            Feature trans2 = new Feature ("School of Transmutation", "Minor Alchemy", 2);
            _context.Features.Add(trans2);
            Feature trans3 = new Feature ("School of Transmutation", "Transmuter's Stone", 6);
            _context.Features.Add(trans3);
            Feature trans4 = new Feature ("School of Transmutation", "Shapechanger", 10);
            _context.Features.Add(trans4);
            Feature trans5 = new Feature ("School of Transmutation", "Master Transmuter", 14);
            _context.Features.Add(trans5);

        _context.SaveChanges();

        return RedirectToAction ("Index");
        }

        [HttpGet("/topsecretspells")]
        public IActionResult GetSpells()
        {
            generatespells();
            // APITesting();
            //Issue seems to be in conversion from type Task<SpellHelperModel> to other instances. Looked into it in Spellprocessor, should now be returning a list of some sort
            return View ("BlankTestPage");
        }
        public void generatespells()
        {
            string Wizard = "Wizard";
            string Bard = "Bard";
            string Druid = "Druid";
            string Ranger = "Ranger";
            string Paladin = "Paladin";
            string Cleric = "Cleric";
            string Sorcerer = "Sorcerer";
            string Warlock = "Warlock";
            

            Spell ASplash = new Spell(0,"Acid Splash", "Wizard");
            _context.Spells.Add(ASplash);
            Spell CTouch = new Spell(0, "Chill Touch", "Sorcerer", "Warlock", "Wizard");
            _context.Spells.Add(CTouch);
            Spell DLight = new Spell(0, "Dancing Lights", "Sorcerer", "Bard", "Wizard");
            _context.Spells.Add(DLight);
            Spell Dcraft = new Spell(0, "Druidcraft", "Druid");
            _context.Spells.Add(Dcraft);
            Spell eblst = new Spell(0, "Eldritch Blast", "Warlock");
            _context.Spells.Add(eblst);
            Spell fbolt = new Spell(0, "Firebolt", "Sorcerer", "Wizard");
            _context.Spells.Add(fbolt);
            Spell gdnc = new Spell(0, "Guidance", "Cleric", "Druid");
            _context.Spells.Add(gdnc);
            Spell lght = new Spell(0, "Light", "Bard", "Cleric", "Sorcerer");
            _context.Spells.Add(lght);
            Spell mhand = new Spell(0, "Mage Hand", "Bard", "Sorcerer", "Warlock", "Wizard");
            _context.Spells.Add(mhand);
            Spell mend = new Spell(0, "Mending", "Bard", "Druid", "Sorcerer", "Wizard");
            _context.Spells.Add(mend);
            Spell mess = new Spell(0, "Message", "Bard","Sorcerer");
            _context.Spells.Add(mess);
            Spell miill = new Spell(0, "Minor Illusion", "Bard", "Sorcerer", "Warlock", "Wizard");
            _context.Spells.Add(miill);
            Spell pspray = new Spell(0, "Poison Spray", "Druid", "Sorcerer", "Warlock", "Wizard");
            _context.Spells.Add(pspray);
            Spell presti = new Spell(0, "Prestidigitation", "Bard", "Sorcerer", "Warlock", "Wizard");
            _context.Spells.Add(presti);
            Spell prodflame = new Spell(0, "Produce Flame", "Druid");
            _context.Spells.Add(prodflame);
            Spell ryfrst = new Spell(0, "Ray of Frost", "Sorcerer", "Wizard");
            _context.Spells.Add(ryfrst);
            Spell restist = new Spell(0, "Resistance", "Cleric:", "Druid");
            _context.Spells.Add(restist);
            Spell sflame = new Spell(0, "Sacred Flame", "Cleric");
            _context.Spells.Add(sflame);
            Spell shill = new Spell(0, "Shillelagh", "Druid");
            _context.Spells.Add(shill);
            Spell sgrasp = new Spell(0, "Shocking Grasp", "Sorcerer", "Wizard");
            _context.Spells.Add(sgrasp);
            Spell spare = new Spell(0, "Spare the Dying", "Cleric");
            _context.Spells.Add(spare);
            Spell Thaum = new Spell(0, "Thaumaturgy", "Cleric");
            _context.Spells.Add(Thaum);
            Spell TrueStrike = new Spell(0, "True Strike", "Bard", "Sorcerer", "Warlock", "Wizard");
            _context.Spells.Add(TrueStrike);
            Spell vmock = new Spell(0, "Viscious Mockery", "Bard");
            _context.Spells.Add(vmock);

        //Level 1
            Spell alrm = new Spell(1, "Alarm", "Ranger", "Wizard");
            _context.Spells.Add(alrm);
            Spell anfriend = new Spell(1, "Animal Friendship", "Bard", "Druid", "Ranger");
            _context.Spells.Add(anfriend);
            Spell bane = new Spell(1, "Bane", "Bard", "Cleric");
            _context.Spells.Add(bane);
            Spell bless = new Spell(1, "Bless", "Cleric", "Paladin");
            _context.Spells.Add(bless);
            Spell bhands = new Spell(1, "Burning Hands", "Sorcerer", "Wizard");
            _context.Spells.Add(bhands);
            Spell chrmprsn = new Spell(1, "Charm Person", "Bard", "Druid", "Sorcerer", "Warlock", "Wizard");
            _context.Spells.Add(chrmprsn);
            Spell clrspry = new Spell(1, "Color Spray", "Sorcerer", "Wizard");
            _context.Spells.Add(clrspry);
            Spell cmmnd = new Spell(1, "Command", "Cleric", "Paladin");
            _context.Spells.Add(cmmnd);
            Spell complang = new Spell(1, "Comprehend Languages", "Bard", "Sorcerer", "Warlock", "Wizard");
            _context.Spells.Add(complang);
            Spell corwater = new Spell(1, "Comprehend Languages", "Cleric", "Druid");
            _context.Spells.Add(corwater);
            Spell cwounds = new Spell(1, "Cure Wounds", "Bard", "Cleric", "Druid");
            _context.Spells.Add(cwounds);
            Spell deg = new Spell(1, "Detect Evil and Good", "Cleric", "Paladin");
            _context.Spells.Add(deg);
            Spell dmag = new Spell(1, "Detect Magic", "Bard", "Cleric", "Druid", "Paladin", "Ranger", "Sorcerer", "Wizard");
            _context.Spells.Add(dmag);
            Spell dpoi = new Spell(1, "Detect Poison and Disease", "Cleric", "Druid", "Paladin", "Ranger");
            _context.Spells.Add(dpoi);
            Spell disguise = new Spell(1, "Disguise Self", "Bard", "Sorcerer", "Wizard");
            _context.Spells.Add(disguise);
            Spell dfav = new Spell(1, "Divine Favor","Paladin");
            _context.Spells.Add(dfav);
            Spell entang = new Spell(1, "Entangle", "Druid");
            _context.Spells.Add(entang);
            Spell exret = new Spell(1, "Expeditious Retreat", "Sorcerer", "Warlock", "Wizard");
            _context.Spells.Add(exret);
            Spell ffire = new Spell(1, "Faerie Fire", "Bard", "Druid");
            _context.Spells.Add(ffire);
            Spell flife = new Spell(1, "False Life", "Sorcerer", "Wizard");
            _context.Spells.Add(flife);
            Spell ffall = new Spell(1, "Feather Fall", "Bard", "Sorcerer", "Wizard");
            _context.Spells.Add(ffall);
            Spell findfam = new Spell(1, "Find Familiar", "Wizard");
            _context.Spells.Add(findfam);
            Spell fdisc = new Spell(1, "Floating Disc", "Wizard");
            _context.Spells.Add(fdisc);
            Spell fogc = new Spell(1, "Fog Cloud", "Druid", "Ranger", "Sorcerer", "Wizard");
            _context.Spells.Add(fogc);
            Spell gberry = new Spell(1, "Goodberry", "Druid", "Ranger");
            _context.Spells.Add(gberry);
            Spell grease = new Spell(1, "Grease", "Wizard");
            _context.Spells.Add(grease);
            Spell gbolt = new Spell(1, "Guiding Bolt", "Cleric");
            _context.Spells.Add(gbolt);
            Spell hword = new Spell(1, "Healing Word", "Bard", "Cleric", "Druid");
            _context.Spells.Add(hword);
            Spell rebuke = new Spell(1, "Hellish Rebuke", "Warlock");
            _context.Spells.Add(rebuke);
            Spell heroism = new Spell(1, "Heroism", "Bard", "Paladin");
            _context.Spells.Add(heroism);
            Spell hidlaugh = new Spell(1, "Hideous Laughter", "Bard", "Wizard");
            _context.Spells.Add(hidlaugh);
            Spell hmark = new Spell(1, "Hunter's Mark", "Ranger");
            _context.Spells.Add(hmark);
            Spell identify = new Spell(1, "Identify", "Bard", "Wizard");
            _context.Spells.Add(identify);
            Spell iscript = new Spell(1, "Illusory Script", "Bard", "Wizard");
            _context.Spells.Add(iscript);
            Spell iwounds = new Spell(1, "Inflict Wounds", "Cleric");
            _context.Spells.Add(iwounds);
            Spell jamp = new Spell(1, "Jump", "Druid", "Ranger", "Sorcerer", "Wizard");
            _context.Spells.Add(jamp);
            Spell lstrider = new Spell(1, "Longstrider", "Bard", "Druid", "Ranger", "Wizard");
            _context.Spells.Add(lstrider);
            Spell marmor = new Spell(1, "Mage Armor", "Sorcerer", "Wizard");
            _context.Spells.Add(marmor);
            Spell mmisl = new Spell(1, "Magic Missile", "Sorcerer", "Wizard");
            _context.Spells.Add(mmisl);
            Spell peg = new Spell(1, "Protection from Evil and Good", "Cleric", "Paladin", "Warlock", "Wizard");
            _context.Spells.Add(peg);
            Spell pfd = new Spell(1, "Purify Food and Drink", "Cleric", "Druid", "Paladin");
            _context.Spells.Add(pfd);
            Spell sanc = new Spell(1, "Sanctuary", "Cleric");
            _context.Spells.Add(sanc);
            Spell shld = new Spell(1, "Shield", "Sorcerer", "Wizard");
            _context.Spells.Add(shld);
            Spell shldf = new Spell(1, "Shield of Faith", "Cleric", "Paladin");
            _context.Spells.Add(shldf);
            Spell silimg = new Spell(1, "Silent Image", "Bard", "Sorcerer", "Wizard");
            _context.Spells.Add(silimg);
            Spell slp = new Spell(1, "Sleep", "Bard", "Sorcerer", "Wizard");
            _context.Spells.Add(slp);
            Spell spkwans = new Spell(1, "Speak With Animals", "Bard", "Druid", "Ranger");
            _context.Spells.Add(spkwans);
            Spell twave = new Spell(1, "Thunderwave", "Bard", "Druid","Sorcerer", "Wizard");
            _context.Spells.Add(twave);
            Spell userv = new Spell(1, "Unseen Servant", "Bard", "Warlock", "Wizard");
            _context.Spells.Add(userv);

        //level 2
            Spell aarrow = new Spell(2, "Acid Arrow", "Wizard");
            _context.Spells.Add(aarrow);
            Spell aid = new Spell(2, "Aid", "Cleric", "Paladin");
            _context.Spells.Add(aid);
            Spell aself = new Spell(2, "Alter Self", "Sorcerer", "Wizard");
            _context.Spells.Add(aself);
            Spell anmess = new Spell(2, "Animal Messenger", "Bard", "Druid", "Ranger");
            _context.Spells.Add(anmess);
            Spell alock = new Spell(2, "Arcane Lock", "Wizard");
            _context.Spells.Add(alock);
            Spell ama = new Spell(2, "Arcanist's Magic Aura", "Wizard");
            _context.Spells.Add(ama);
            Spell augury = new Spell(2, "Augury", "Cleric");
            _context.Spells.Add(augury);
            Spell brkskn = new Spell(2, "Bark Skin", "Druid", "Ranger");
            _context.Spells.Add(brkskn);
            Spell dlindeaf = new Spell(2, "Blindness/Deafness", "Bard", "Cleric", "Sorcerer", "Wizard");
            _context.Spells.Add(dlindeaf);
            Spell blur = new Spell(2, "Blur", "Sorcerer", "Wizard");
            _context.Spells.Add(blur);
            Spell brandin = new Spell(2, "Branding Smite", "Paladin");
            _context.Spells.Add(brandin);
            Spell calme = new Spell(2, "Calm Emotions", "Bard", "Cleric");
            _context.Spells.Add(calme);
            Spell conflame = new Spell(2, "Continual Flame", "Cleric", "Wizard");
            _context.Spells.Add(conflame);
            Spell dorkness = new Spell(2, "Darkness", "Sorcerer", "Warlock", "Wizard");
            _context.Spells.Add(dorkness);
            Spell dorkv = new Spell(2, "Darkvision", "Druid", "Ranger", "Sorcerer", "Wizard");
            _context.Spells.Add(dorkv);
            Spell dthots = new Spell(2, "Detect Thoughts", "Bard", "Sorcerer", "Wizard");
            _context.Spells.Add(dthots);
            Spell ebail = new Spell(2, "Enhance Ability", "Bard", "Cleric", "Druid", "Sorcerer");
            _context.Spells.Add(ebail);
            Spell enlreduc = new Spell(2, "Enlarge/Reduce", "Sorcerer", "Wizard");
            _context.Spells.Add(enlreduc);
            Spell enthrall = new Spell(2, "Enthrall", "Bard", "Warlock");
            _context.Spells.Add(enthrall);
            Spell fsteed = new Spell(2, "Find Steed", "Paladin");
            _context.Spells.Add(fsteed);
            Spell ftraps = new Spell(2, "Find Traps","Cleric", "Druid", "Ranger");
            _context.Spells.Add(ftraps);
            Spell fblade = new Spell(2, "Flame Blade", "Druid");
            _context.Spells.Add(fblade);
            Spell fsphere = new Spell(2, "Flaming Sphere", "Druid", "Wizard");
            _context.Spells.Add(fsphere);
            Spell grepo = new Spell(2, "Gentle Repose", "Cleric", "Wizard");
            _context.Spells.Add(grepo);
            Spell gwind = new Spell(2, "Gust of Wind", "Druid", "Sorcerer", "Wizard");
            _context.Spells.Add(gwind);
            Spell hmetal = new Spell(2, "Heat Metal", "Bard", "Druid");
            _context.Spells.Add(hmetal);
            Spell hperson = new Spell(2, "Hold Person", "Bard", "Cleric", "Druid", "Sorcerer", "Warlock", "Wizard");
            _context.Spells.Add(hperson);
            Spell invis = new Spell(2, "Invisibility", "Bard", "Sorcerer", "Warlock", "Wizard");
            _context.Spells.Add(invis);
            Spell knck = new Spell(2, "Knock", "Bard", "Sorcerer", "Wizard");
            _context.Spells.Add(knck);
            Spell lsrrstr = new Spell(2, "Lesser Restoration", "Bard", "Cleric", "Druid", "Paladin", "Ranger");
            _context.Spells.Add(lsrrstr);
            Spell levi = new Spell(2, "Levitate", "Sorcerer", "Wizard");
            _context.Spells.Add(levi);
            Spell locanpla = new Spell(2, "Locate Animals or Plants", "Druid","Ranger");
            _context.Spells.Add(locanpla);
            Spell locaob = new Spell(2, "Locate Object", "Bard","Cleric", "Druid", "Paladin", "Ranger", "Wizard");
            _context.Spells.Add(locaob);
            Spell mmouth = new Spell(2, "Magic Mouth", "Bard", "Wizard");
            _context.Spells.Add(mmouth);
            Spell mwep = new Spell(2, "Magic Weapon", "Paladin", "Wizard");
            _context.Spells.Add(mwep);
            Spell mimage = new Spell(2, "Mirror Image", "Sorcerer","Warlock", "Wizard");
            _context.Spells.Add(mimage);
            Spell mistep = new Spell(2, "Misty Step", "Sorcerer", "Warlock", "Wizard");
            _context.Spells.Add(mistep);
            Spell mbeam = new Spell(2, "Moonbeam", "Druid");
            _context.Spells.Add(mbeam);
            Spell pwt = new Spell(2, "Pass Without Trace", "Druid", "Ranger");
            _context.Spells.Add(pwt);
            Spell prayer = new Spell(2, "Prayer of Healing", "Cleric");
            _context.Spells.Add(prayer);
            Spell poiprot = new Spell(2, "Protection from Poison", "Cleric", "Druid", "Paladin", "Ranger");
            _context.Spells.Add(poiprot);
            Spell roe = new Spell(2, "Ray of Enfeeblement", "Warlock", "Wizard");
            _context.Spells.Add(roe);
            Spell ropetrick = new Spell(2, "Rope Trick", "Wizard");
            _context.Spells.Add(ropetrick);
            Spell sray = new Spell(2, "Scorching Ray", "Sorcerer", "Wizard");
            _context.Spells.Add(sray);
            Spell sinvis = new Spell(2, "See Invisibility", "Bard", "Sorcerer", "Wizard");
            _context.Spells.Add(sinvis);
            Spell shat = new Spell(2, "Shatter", "Bard", "Sorcerer", "Warlock", "Wizard");
            _context.Spells.Add(shat);
            Spell silen = new Spell(2, "Silence", "Bard", "Cleric", "Ranger");
            _context.Spells.Add(silen);
            Spell spclimb = new Spell(2, "Spider Climb", "Sorcerer","Warlock", "Wizard");
            _context.Spells.Add(spclimb);
            Spell sgrowth = new Spell(2, "Spike Growth", "Druid");
            _context.Spells.Add(sgrowth);
            Spell swep = new Spell(2, "Spiritual Weapon", "Cleric");
            _context.Spells.Add(swep);
            Spell sug = new Spell(2, "Suggestion", "Bard", "Sorcerer", "Warlock", "Wizard");
            _context.Spells.Add(sug);
            Spell wardbond = new Spell(2, "Warding Bond", "Cleric");
            _context.Spells.Add(wardbond);
            Spell web = new Spell(2, "Web", "Sorcerer", "Wizard");
            _context.Spells.Add(web);
            Spell zoneoftruth = new Spell(2, "Zone of Truth","Bard", "Cleric", "Paladin");
            _context.Spells.Add(zoneoftruth);
            
        // Level 3
            Spell animateDead = new Spell(3, "Animate Dead", "Wizard", "Cleric");
            _context.Spells.Add(animateDead);
            Spell beaconHope = new Spell(3, "Beacon of Hope", "Cleric");
            _context.Spells.Add(beaconHope);
            Spell bestowCurse = new Spell(3, "Bestow Curse", "Bard", "Cleric", "Wizard");
            _context.Spells.Add(bestowCurse);
            Spell blink = new Spell(3, "Blink", "Wizard", "Sorcerer");
            _context.Spells.Add(blink);
            Spell callLighting = new Spell(3, "Call Lightning", "Druid");
            _context.Spells.Add(callLighting);
            Spell clairvoyance = new Spell(3, "Clairvoyance", "Bard", "Cleric", "Sorcerer", "Wizard");
            _context.Spells.Add(clairvoyance);
            Spell conjureAnimals = new Spell(3, "Conjure Animals", "Druid", "Ranger");
            _context.Spells.Add(conjureAnimals);
            Spell counterspell = new Spell(3, "Counterspell", "Warlock", "Sorcerer", "Wizard");
            _context.Spells.Add(counterspell);
            Spell createFood = new Spell(3, "Create Food and Water", "Palandin", "Cleric", "Sorcerer", "Wizard");
            _context.Spells.Add(createFood);
            Spell daylight = new Spell(3, "Daylight", "Druid", "Cleric", "Paladin", "Ranger", "Sorcerer");
            _context.Spells.Add(daylight);
            Spell dispelMagic = new Spell(3, "Dispel Magic", "Bard", "Cleric", "Druid", "Paladin", "Sorcerer", "Warlock", "Wizard");
            _context.Spells.Add(dispelMagic);
            Spell fear = new Spell(3, "Fear", "Bard", "Warlock", "Sorcerer", "Wizard");
            _context.Spells.Add(fear);
            Spell fireball = new Spell(3, "Fireball","Sorcerer", "Wizard");
            _context.Spells.Add(fireball);
            Spell fly = new Spell(3, "Warlock", "Sorcerer", "Wizard");
            _context.Spells.Add(fly);
            Spell gaseousForm = new Spell(3, "Gaseous Form", "Warlock", "Sorcerer", "Wizard");
            _context.Spells.Add(gaseousForm);
            Spell glyphWard = new Spell(3, "Glyph of Warding", "Bard", "Cleric", "Wizard");
            _context.Spells.Add(glyphWard);
            Spell haste = new Spell(3, "Haste", "Sorcerer", "Wizard");
            _context.Spells.Add(haste);
            Spell hypnoticPattern = new Spell(3, "Hypnotic Pattern", "Bard", "Warlock", "Sorcerer", "Wizard");
            _context.Spells.Add(hypnoticPattern);
            Spell lightningBolt = new Spell(3, "Lightning Bolt", "Bard", "Warlock", "Sorcerer", "Wizard");
            _context.Spells.Add(lightningBolt);
            Spell magicCircle = new Spell(3, "Magic Circle", "Cleric", "Warlock", "Paladin", "Wizard");
            _context.Spells.Add(magicCircle);
            Spell majorImage = new Spell(3, "Major Image", "Bard", "Sorcerer", "Warlock", "Wizard");
            _context.Spells.Add(majorImage);
            Spell massHealing = new Spell(3, "Mass Healing", "Cleric");
            _context.Spells.Add(massHealing);
            Spell meldStone = new Spell(3, "Meld into Stone", "Cleric", "Druid");
            _context.Spells.Add(meldStone);
            Spell nondetection = new Spell(3, "Nondetection", "Bard", "Rangers", "Wizards");
            _context.Spells.Add(nondetection);
            Spell phantomSeed = new Spell(3, "Phantom Seed", "Wizard");
            _context.Spells.Add(phantomSeed);
            Spell plantGrowth = new Spell(3, "Plant Growth", "Bard", "Druid", "Ranger");
            _context.Spells.Add(plantGrowth);
            Spell protectionEnergy = new Spell(3, "Protection From Energy", "Cleric", "Druid", "Ranger", "Sorcerer", "Wizard");
            _context.Spells.Add(protectionEnergy);
            Spell removeCurse = new Spell(3, "Remove Curse", "Cleric", "Paladin", "Warlock", "Wizard");
            _context.Spells.Add(protectionEnergy);
            Spell revivify = new Spell(3, "Revivify", "Cleric", "Paladin");
            _context.Spells.Add(revivify);
            Spell sending = new Spell(3, "Sending", "Cleric", "Bard", "Wizard");
            _context.Spells.Add(sending);
            Spell sleetStorm = new Spell(3, "Sleet Storm", "Druic", "Sorcerer", "Wizard");
            _context.Spells.Add(sleetStorm);
            Spell slow = new Spell(3, "Slow", "Srocerer", "Wizard");
            _context.Spells.Add(slow);
            Spell speakDead = new Spell(3, "Speak with Dead", "Cleric", "Bard");
            _context.Spells.Add(speakDead);
            Spell speakPlants = new Spell(3, "Speak with Plants", "Druid", "Ranger");
            _context.Spells.Add(speakPlants);
            Spell spiritGaurdians = new Spell(3, "Spirit Gaurdians", "Cleric");
            _context.Spells.Add(spiritGaurdians);
            Spell stikingCloud = new Spell(3, "Stinking Cloud", "Sorcerer", "Wizard" );
            _context.Spells.Add(stikingCloud);
            Spell tinyHut = new Spell(3, "Tiny Hut", "Wizard", "Bard" );
            _context.Spells.Add(tinyHut);
            Spell tongues = new Spell(3, "Tongues", "Cleric", "Warlock", "Sorcerer", "Wizard", "Bard" );
            _context.Spells.Add(tongues);
            Spell vampricTouch = new Spell(3, "Vampric Touch", "Warlock", "Wizard" );
            _context.Spells.Add(vampricTouch);
            Spell waterBreathing = new Spell(3, "Water Breathing", "Druid", "Ranger", "Sorcerer", "Wizard" );
            _context.Spells.Add(waterBreathing);
            Spell waterWalking = new Spell(3, "Cleric", "Druid", "Ranger", "Sorcerer" );
            _context.Spells.Add(waterWalking);
            Spell windWall = new Spell(3, "Wind Wall", "Druid", "Ranger");
            _context.Spells.Add(windWall);
            
        // Level 4
            Spell arceye = new Spell(4, "Arcane Eye", "Wizard");
            _context.Spells.Add(arceye);
            Spell banish = new Spell(4, "Banishment", "Cleric", "Paladin", "Sorcerer", "Warlock", "Wizard");
            _context.Spells.Add(banish);
            Spell blktent = new Spell(4, "Black Tentacles", "Wizard");
            _context.Spells.Add(blktent);
            Spell blight = new Spell(4, "Blight", "Druid", "Sorcerer", "Warlock", "Wizard");
            _context.Spells.Add(blight);
            Spell compul = new Spell(4, "Compulsion", "Bard");
            _context.Spells.Add(compul);
            Spell confus = new Spell(4, "Confusion", "Bard", "Druid", "Sorcerer", "Wizard");
            _context.Spells.Add(confus);
            Spell ConjureME = new Spell(4, "Conjure Minor Elementals", "Druid", "Wizard");
            _context.Spells.Add(ConjureME);
            Spell ConjureWB = new Spell(4, "Conjure Woodland Beings", "Druid", "Ranger");
            _context.Spells.Add(ConjureWB);
            Spell cwater = new Spell(4, "Control Water", "Cleric", "Druid", "Wizard");
            _context.Spells.Add(cwater);
            Spell DWard = new Spell(4, "Death Ward", "Cleric", "Paladin");
            _context.Spells.Add(DWard);
            Spell DDoor = new Spell(4, "Dimension Door", "Bard","Sorcerer", "Warlock", "Wizard");
            _context.Spells.Add(DDoor);
            Spell divin = new Spell(4, "Divination", "Cleric");
            _context.Spells.Add(divin);
            Spell dombeast = new Spell(4, "Dominate Beast", "Druid", "Sorcerer");
            _context.Spells.Add(dombeast);
            Spell fab = new Spell(4, "Fabricate", "Wizard");
            _context.Spells.Add(fab);
            Spell fhound = new Spell(4, "Faithel Hound", "Wizard");
            _context.Spells.Add(fhound);
            Spell fshield = new Spell(4, "Fire Shield", "Wizard");
            _context.Spells.Add(fshield);
            Spell freemov = new Spell(4, "Freedom of Movement", "Bard", "Cleric", "Druid", "Ranger");
            _context.Spells.Add(freemov);
            Spell giins = new Spell(4, "Giant Insect", "Druid");
            _context.Spells.Add(giins);
            Spell ginvis = new Spell(4, "Greater Invisibility", "Bard", "Sorcerer","Wizard");
            _context.Spells.Add(ginvis);
            Spell gofaith = new Spell(4, "Guardian of Faith", "Cleric");
            _context.Spells.Add(gofaith);
            Spell halluter = new Spell(4, "Hallucinatory Terrain", "Bard","Warlock", "Wizard");
            _context.Spells.Add(halluter);
            Spell locacreat = new Spell(4, "Locate Creature", "Bard","Cleric","Druid", "Paladin","Ranger", "Wizard");
            _context.Spells.Add(locacreat);
            Spell Phankill = new Spell(4, "Phantasmal Killer","Wizard");
            _context.Spells.Add(Phankill);
            Spell polym = new Spell(4, "Polymorph", "Bard", "Druid", "Sorcerer", "Wizard");
            _context.Spells.Add(polym);
            Spell privsanc = new Spell(4, "Private Sanctum", "Wizard");
            _context.Spells.Add(privsanc);
            Spell rsphere = new Spell(4, "Resilient", "Wizard");
            _context.Spells.Add(rsphere);
            Spell schest = new Spell(4, "Secret Chest", "Wizard");
            _context.Spells.Add(schest);
            Spell stonesha = new Spell(4, "Stone Shape", "Cleric", "Druid", "Wizard");
            _context.Spells.Add(stonesha);
            Spell Stoneskin = new Spell(4, "Stoneskin", "Druid", "Ranger", "Sorcerer", "Wizard");
            _context.Spells.Add(Stoneskin);
            Spell wallofiya = new Spell(4, "Wall of Fire", "Druid","Sorcerer", "Wizard");
            _context.Spells.Add(wallofiya);

            // 5th Level
            Spell anobj = new Spell(5, "Animate Objects", "Bard","Sorcerer", "Wizard");
            _context.Spells.Add(anobj);
            Spell anlifshl = new Spell(5, "Antilife Shell", "Druid");
            _context.Spells.Add(anlifshl);
            Spell archand = new Spell(5, "Arcane Hand", "Wizard");
            _context.Spells.Add(archand);
            Spell awaken = new Spell(5, "Awaken", "Bard", "Druid");
            _context.Spells.Add(awaken);
            Spell cloudkill = new Spell(5, "Cloudkill", "Sorcerer", "Wizard");
            _context.Spells.Add(cloudkill);
            Spell commune = new Spell(5, "Commune", "Cleric");
            _context.Spells.Add(commune);
            Spell communeNature = new Spell(5, "Commune with Nature", "Druid", "Ranger");
            _context.Spells.Add(communeNature);
            Spell coneOfCold = new Spell(5, "Cone of Cold", "Sorcerer", "Wizard");
            _context.Spells.Add(coneOfCold);
            Spell conjureElemnetal = new Spell(5, "Conjure Elemental", "Druid", "Wizard");
            _context.Spells.Add(conjureElemnetal);
            Spell contactPlane = new Spell(5, "Contact Other Plane", "Warlock", "Wizard");
            _context.Spells.Add(contactPlane);
            Spell contagion = new Spell(5, "Contagion", "Cleric", "Druid");
            _context.Spells.Add(contagion);
            Spell creation = new Spell(5, "Creation", "Sorcerer", "Wizard");
            _context.Spells.Add(creation);
            Spell dispelEvilGood = new Spell(5, "Dispel Evil and Good", "Cleric", "Paladin");
            _context.Spells.Add(dispelEvilGood);
            Spell dominatePerson = new Spell(5, "Dominate Person", "Sorcerer", "Bard", "Wizard");
            _context.Spells.Add(dominatePerson);
            Spell dream = new Spell(5, "Dream", Wizard, Warlock, "Bard");
            _context.Spells.Add(dream); 
            Spell flameStrike = new Spell(5, "Flame Strike", Cleric);
            _context.Spells.Add(flameStrike);
            Spell geas = new Spell(5, "Geas", Bard, Cleric, Druid, Paladin, Wizard);
            _context.Spells.Add(geas);
            Spell greaterResto = new Spell(5, "Greater Restoration", Bard, Cleric, Druid);
            _context.Spells.Add(greaterResto);
            Spell hallow = new Spell(5, "Hallow", Cleric);
            _context.Spells.Add(hallow);
            Spell holdMonster = new Spell(5, "Hold Monster", Bard, Sorcerer, Warlock, Wizard);
            _context.Spells.Add(holdMonster);
            Spell insectPlague = new Spell(5, "Insect Plague", Sorcerer, Cleric, Druid);
            _context.Spells.Add(insectPlague);
            Spell legendLore = new Spell(5, "Legend Lore", Bard, Cleric, Wizard);
            _context.Spells.Add(legendLore);
            Spell massCure = new Spell(5, "Mass Cure Wounds", Bard, Cleric, Druid);
            _context.Spells.Add(massCure);
            Spell mislead = new Spell(5, "Mislead", Bard, Wizard);
            _context.Spells.Add(mislead);
            Spell modMemory = new Spell(5, "Modify Memory", Bard, Wizard);
            _context.Spells.Add(modMemory);
            Spell passwall = new Spell(5, "Passwall", Wizard);
            _context.Spells.Add(passwall);
            Spell planarBinding= new Spell(5, "Planar Binding", Bard, Cleric, Druid, Wizard);
            _context.Spells.Add(planarBinding);
            Spell raiseDead = new Spell(5, "Raise Dead", Bard, Cleric, Paladin);
            _context.Spells.Add(raiseDead);
            Spell reincarnate = new Spell(5, "Reincarnate", Druid);
            _context.Spells.Add(reincarnate);
            Spell scrying = new Spell(5, "Scrying", Bard, Cleric, Druid, Warlock, Wizard);
            _context.Spells.Add(scrying);
            Spell seeming = new Spell(5, "Seeming", Bard, Sorcerer, Wizard);
            _context.Spells.Add(seeming);
            Spell telekinesis = new Spell(5, "Telekinesis", Sorcerer, Wizard);
            _context.Spells.Add(telekinesis);
            Spell telepathicBond = new Spell(5, "Telepathic Bond", Wizard);
            _context.Spells.Add(telepathicBond);
            Spell teleportationCir = new Spell(5, "Teleportation Circle", Bard, Sorcerer, Wizard);
            _context.Spells.Add(teleportationCir);
            Spell treeStride = new Spell(5, "Tree Stride", Ranger, Druid);
            _context.Spells.Add(treeStride);
            Spell wallForce = new Spell(5, "Wall of Force", Wizard);
            _context.Spells.Add(wallForce);
            Spell wallStone = new Spell(5, "Wall of Stone", Wizard, Sorcerer, Druid);
            _context.Spells.Add(wallStone);

            // Level 6
            
            
            
        }
    }
}
