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

            ViewBag.character = character;

            dynamic MyModel = new ExpandoObject();
            MyModel.User = SessionUser;
            MyModel.Login = new Login();
            MyModel.Character = character; 

            return View("index", MyModel);
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

            //Dynamic model with USer, Login, Character
            dynamic MyModel = new ExpandoObject();
            MyModel.User = SessionUser;
            MyModel.Login = new Login();
            MyModel.Character = newPlayer; 

            //LINQ query

            return View("Classes", MyModel);
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
            return View("classes", MyModel);
        }
        // =============================================================================
        // Feature builder
        
        public void BuildFeatureTable(){

            //Base Class Features
            //Subclass Features
    //Barbarian
            Feature Rage = new Feature("Barbarian", "Rage", 1);
            Feature UnDef = new Feature("Barbarian", "Unarmored Defense (Barbarian)", 1);
            Feature Reck = new Feature("Barbarian", "Reckless Attack", 2);
            Feature DanSen = new Feature ("Barbarian", "Danger Sense", 2);
            Feature EA = new Feature("Barbarian", "Extra Attack", 5);
            Feature fast = new Feature("Barbarian", "Fast Movement", 5);
            Feature fin = new Feature ("Barbarian", "Feral Instinct", 7);
            Feature crit = new Feature ("Barbarian", "Brutal Critical (1 Die)", 9);
            Feature rent = new Feature ("Barbarian", "Relentless Rage", 11);
            Feature bruh = new Feature ("Barbarian", "Brutal Critical (2 Dice)", 13);
            Feature pers = new Feature ("Barbarian", "Persistent Rage", 15);
            Feature three = new Feature ("Barbarian", "Brutal Critical (3 Dice)", 17);
            Feature indom = new Feature ("Barbarian", "Indomitable Might", 18);
            Feature champ = new Feature ("Barbarian", "Primal Champion", 20);
        //Totem
        //Berserker
    //Bard
            Feature bard0 = new Feature ("Bard", "Bardic Inspiration (d6)", 1);
            Feature bardspc = new Feature ("Bard", "Spellcasting (Bard)", 1);
            Feature bard2 = new Feature ("Bard", "Jack of All Trades", 2);
            Feature bard22 = new Feature ("Bard", "Song of Rest (d6)", 2);
            Feature bard3 = new Feature ("Bard", "Expertise", 3);
            Feature bard5 = new Feature ("Bard", "Bardic Inspiration (d8)", 5);
            Feature bard55 = new Feature ("Bard", "Font of Inspiration", 5);
            Feature bard6 = new Feature ("Bard", "Countercharm", 6);
            Feature bard9 = new Feature ("Bard", "Song of Rest (d8)", 9);
            Feature bard10_1 = new Feature("Bard", "Bardic Inspiration (d10)", 10);
            Feature bard10_2 = new Feature("Bard", "Magical Secrets (Level 10)", 10);
            Feature bard10_3 = new Feature("Bard", "Expertise", 10);
            Feature bard13 = new Feature("Bard", "Song of Rest (d10)", 13);
            Feature bard14_1= new Feature("Bard", "Magical Secrets (Level 14)", 14);
            Feature bard15 = new Feature("Bard", "Bardic Inspiration (d12)", 15);
            Feature bard17 = new Feature("Bard", "Song of Rest (d12)", 17);
            Feature bard18 = new Feature("Bard", "Magical Secrets (Level 18)", 18);
            Feature bard20 = new Feature("Bard", "Superior Inspiration", 20);
    //Cleric
            Feature Cleric1 = new Feature("Cleric", "Spellcasting (Cleric)", 1);
            Feature Cleric2 = new Feature("Cleric", "Channel Divinity (1/rest)", 2);
            Feature Cleric5 = new Feature("Cleric", "Destroy Undead (CR 1/2)", 5);
            Feature Cleric6 = new Feature("Cleric", "Channel Divinity (2/Rest)", 6);
            Feature Cleric8 = new Feature("Cleric", "Destroy Undead (CR 1)", 8);
            Feature Cleric10 = new Feature("Cleric", "Divine Intervention", 10);
            Feature Cleric11 = new Feature("Cleric", "Destroy Undead (CR 2)", 11);
            Feature Cleric14 = new Feature("Cleric", "Destroy Undead (CR 3)", 14);
            Feature Cleric17 = new Feature("Cleric", "Destroy Undead (CR 4)", 17);
            Feature Cleric18 = new Feature("Cleric", "Channel Divinity (3/rest", 18);
            Feature Cleric20 = new Feature("Cleric", "Divine Intervention Improvement", 20);
    //Druid
            Feature Druid1 = new Feature("Druid", "Spellcasting (Druid)", 1);
            Feature Druid1_1 = new Feature("Druid", "Druidic", 1);
            Feature Druid2 = new Feature("Druid", "Wild Shape", 2);
            Feature Druid4 = new Feature("Druid", "Wild Shape Improvement (Level 4)", 4);
            Feature Druid8 = new Feature("Druid", "Wild Shape Improvement(Level 8)", 8);
            Feature Druid18 = new Feature("Druid", "Timeless Body", 18);
            Feature Druid18_1 = new Feature("Druid", "Beast Spells", 18);
            Feature Druid20 = new Feature("Druid", "Archdruid", 20);
    //Fighter
            Feature Fighter1 = new Feature("Fighter", "Fighting Style", 1); //Need to implement randomization for Fighting Style choice
            Feature Fighter1_1 = new Feature("Fighter", "Second Wind", 1);
            Feature Fighter2 = new Feature("Fighter", "Action Surge", 2);
            Feature Fighter5 = new Feature("Fighter", "Extra Attack", 5);
            Feature Fighter9 = new Feature("Fighter", "Indomitable (1 Use)", 9);
            Feature Fighter11 = new Feature("Fighter", "Extra Attack (2)", 11);
            Feature Fighter13 = new Feature("Fighter", "Indomitable (2 Use)", 13);
            Feature Fighter17 = new Feature("Fighter", "Action Surge (2 Uses)", 17);
            Feature Fighter17_1 = new Feature("Fighter", "Indomitable (3 Uses)", 17);
            Feature Fighter20 = new Feature("Fighter", "Extra Attack (3)", 20);
    //Monk
            Feature Monk1 = new Feature("Monk", "Unarmored Defense (Monk)", 1);
            Feature Monk1_1 = new Feature("Monk", "Martial Arts", 1);
            Feature Monk2 = new Feature("Monk", "Ki", 2);
            Feature Monk2_2 = new Feature("Monk", "Unarmored Movement", 2);
            Feature Monk3 = new Feature("Monk", "Deflect Missiles", 3);
            Feature Monk4 = new Feature("Monk", "Slow Fall", 4);
            Feature Monk5 = new Feature("Monk", "Extra Attack", 5);
            Feature Monk5_5 = new Feature("Monk", "Stunning Strike", 5);
            Feature Monk6 = new Feature("Monk", "Ki-Empowered Strikes", 6);
            Feature Monk7 = new Feature("Monk", "Evasion", 7);
            Feature Monk7_1 = new Feature("Monk", "Stillness of Mind", 7);
            Feature Monk9 = new Feature("Monk", "Unarmored Movement Improvement", 9);
            Feature Monk10 = new Feature("Monk", "Purity of Body", 10);
            Feature Monk13 = new Feature("Monk", "Tongue of the Sun and Moon", 1);
            Feature Monk14 = new Feature("Monk", "Diamond Soul", 14);
            Feature Monk15 = new Feature ("Monk", "Timeless Body", 15);
            Feature Monk18 = new Feature ("Monk", "Empty Body", 18);
            Feature Monk20 = new Feature ("Monk", "Perfect Self", 20);
    //Paladin
            Feature Pally0 = new Feature ("Paladin", "Divine Sense", 1);
            Feature Pally1 = new Feature ("Paladin", "Lay on Hands", 1);
            Feature Pally2 = new Feature ("Paladin", "Fighting Style", 2);
            Feature Pally2_2 = new Feature ("Paladin", "Spellcasting (Paladin)", 2);
            Feature Pally2_3 = new Feature ("Paladin", "Divine Smite", 2);
            Feature Pally5 = new Feature ("Paladin", "Divine Smite", 5);
            Feature Pally10 = new Feature ("Paladin", "Aura of Courage", 10);
            Feature Pally11 = new Feature ("Paladin", "Improved Divine Smite", 11);
            Feature Pally14 = new Feature ("Paladin", "Cleansing Touch", 14);
            Feature Pally18 = new Feature ("Paladin", "Aura Improvements", 18);
    //Ranger
            Feature rngr1 = new Feature ("Ranger", "Favored Enemy", 1); //Need to expand on this, as it covers a number of possible options
            Feature rngr1_1 = new Feature ("Ranger", "Natural Explorer", 1);
            Feature rngr2 = new Feature ("Ranger", "Fighting Style", 2); //Reqs expansion, similar to other Fighting Style options
            Feature rngr2_2 = new Feature ("Ranger", "Spellcasting (Ranger)", 2);
            Feature rngr3 = new Feature ("Ranger", "Primeval Awareness", 3);
            Feature rngr5 = new Feature ("Ranger", "Extra Attack", 5);
            Feature rngr6 = new Feature ("Ranger", "Favored Enemy and Natural Explorer Improvements", 6);
            Feature rngr10 = new Feature ("Ranger", "Natural Explorer Improvement", 10);
            Feature rngr10_1 = new Feature ("Ranger", "Hide in Plain Sight", 10);
            Feature rngr14 = new Feature ("Ranger", "Favored Enemy Improvement (Level 14)", 14);
            Feature rngr14_1 = new Feature ("Ranger", "Vanish", 14);
            Feature rngr18 = new Feature ("Ranger", "Feral Senses", 18);
            Feature rngr20 = new Feature ("Ranger", "Foe Slayer", 20);
    //Rogue
            Feature rg1 = new Feature ("Rogue", "Expertise", 1);
            Feature rg1_1 = new Feature ("Rogue", "Sneak Attack", 1);
            Feature rg1_2 = new Feature ("Rogue", "Thieves Cant", 1);
            Feature rg2 = new Feature ("Rogue", "Cunning Action", 2);
            Feature rg5 = new Feature ("Rogue", "Uncanny Dodge", 5);
            Feature rg6 = new Feature ("Rogue", "Expertise", 6);
            Feature rg7 = new Feature ("Rogue", "Evasion", 7);
            Feature rg11 = new Feature ("Rogue", "Reliable Talent", 11);
            Feature rg14 = new Feature ("Rogue", "Blindsense", 14);
            Feature rg15 = new Feature ("Rogue", "Slippery Mind", 15);
            Feature rg18 = new Feature ("Rogue", "Elusive", 18);
            Feature rg20 = new Feature ("Rogue", "Stroke of Luck", 20);
    //Sorcerer
            Feature sorc1 = new Feature ("Sorcerer", "Spellcasting (Sorcerer)", 1);
            Feature sorc2 = new Feature ("Sorcerer", "Font of Magic", 2);
            Feature sorc3 = new Feature ("Sorcerer", "Metamagic (Level 3)", 3); //Have to generate options
            Feature sorc10 = new Feature ("Sorcerer", "Metamagic (Level 10)", 10);
            Feature sorc17 = new Feature ("Sorcerer", "Metamagic (Level 17)", 17);
            Feature sorc20 = new Feature ("Sorcerer", "Sorcerous Restoration", 20);
    //Warlock
            Feature lock0 = new Feature ("Warlock", "Pact Magic", 1);
            Feature lock2 = new Feature ("Warlock", "Eldritch Invocations", 2); //Need to expand based on Invocations
            Feature lock3 = new Feature ("Warlock", "Pact Boon", 3); // Need to expand on this to generate options, probably list each pact boon under this and when Pact boon is recieved, add one of the options to the list
            Feature lock11 = new Feature ("Warlock", "Mystic Arcanum (Level 6)", 11);
            Feature lock13 = new Feature ("Warlock", "Mystic Arcanum (Level 7)", 13);
            Feature lock15 = new Feature ("Warlock", "Mystic Arcanum (Level 8)", 15);
            Feature lock17 = new Feature ("Warlock", "Mystic Arcanum (Level 9)", 17);
            Feature lock20 = new Feature ("Warlock", "Eldritch Master", 20);
    //Wizard
            Feature Wiz1 = new Feature ("Wizard", "Spellcasting (Wizard)", 1);
            Feature Wiz1_1 = new Feature ("Wizard", "Arcane Recovery", 1);
            Feature Wiz18 = new Feature ("Wizard", "Spell Mastery", 18);
            Feature Wiz20 = new Feature ("Wizard", "Signature Spell", 20);



            _context.SaveChanges();
        }
    }
}
