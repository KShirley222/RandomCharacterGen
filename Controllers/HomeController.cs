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
        //Berserker
            Feature ber1 = new Feature ("Path of the Berserker", "Frenzy", 3);
            Feature ber2 = new Feature ("Path of the Berserker", "Mindless Rage", 6);
            Feature ber3 = new Feature ("Path of the Berserker", "Intimidating Presence", 10);
            Feature ber4 = new Feature ("Path of the Berserker", "Retaliation", 14);
        //Totem
            Feature tot1 = new Feature ("Path of the Totem Warrior", "Spirit Seeker", 3);
            Feature tot2 = new Feature ("Path of the Totem Warrior", "Totem Spirit", 3); //*Choices
            Feature tot3 = new Feature ("Path of the Totem Warrior", "Aspect of the Beast", 6); //*Choices
            Feature tot4 = new Feature ("Path of the Totem Warrior", "Spirit Walker", 10);
            Feature tot5 = new Feature ("Path of the Totem Warrior", "Totemic Attunement", 14); //*Choices
    //Bard
            Feature bard0 = new Feature ("Bard", "Bardic Inspiration (d6)", 1);
            Feature bardspc = new Feature ("Bard", "Spellcasting (Bard)", 1);
            Feature bard2 = new Feature ("Bard", "Jack of All Trades", 2);
            Feature bard22 = new Feature ("Bard", "Song of Rest (d6)", 2);
            Feature bard3 = new Feature ("Bard", "Expertise (2 Skills)", 3);
            Feature bard5 = new Feature ("Bard", "Bardic Inspiration (d8)", 5);
            Feature bard55 = new Feature ("Bard", "Font of Inspiration", 5);
            Feature bard6 = new Feature ("Bard", "Countercharm", 6);
            Feature bard9 = new Feature ("Bard", "Song of Rest (d8)", 9);
            Feature bard10_1 = new Feature("Bard", "Bardic Inspiration (d10)", 10);
            Feature bard10_2 = new Feature("Bard", "Magical Secrets (Level 10)", 10);
            Feature bard10_3 = new Feature("Bard", "Expertise (2 Skills)", 10);
            Feature bard13 = new Feature("Bard", "Song of Rest (d10)", 13);
            Feature bard14_1= new Feature("Bard", "Magical Secrets (Level 14)", 14);
            Feature bard15 = new Feature("Bard", "Bardic Inspiration (d12)", 15);
            Feature bard17 = new Feature("Bard", "Song of Rest (d12)", 17);
            Feature bard18 = new Feature("Bard", "Magical Secrets (Level 18)", 18);
            Feature bard20 = new Feature("Bard", "Superior Inspiration", 20);
        //College of Lore
            Feature Lore1 = new Feature ("College of Lore", "Bonus Proficiencies (Any 3 Skills)", 3); //*Gains 3 additional proficiencies
            Feature Lore2 = new Feature ("College of Lore", "Cutting Words", 3);
            Feature Lore3 = new Feature ("College of Lore", "Additional Magical Secrets", 6); //*Gains 2 additional spells NOT KNOWN from any list, up to third level
            Feature Lore4 = new Feature ("College of Lore", "Peerless Skill", 14);
        //College of Valor
            Feature Valor1 = new Feature ("College of Valor", "Bonus Proficiencies (Medium Armor, Shields, Martial Weapons)", 3); //*Proficiencies is armor/weapons. Should we account for those as well?
            Feature Valor2 = new Feature ("College of Valor", "Combat Inspiration", 3);
            Feature Valor3 = new Feature ("College of Valor", "Extra Attack", 6);
            Feature Valor4 = new Feature ("College of Valor", "Battle Magic", 14);
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
        //Knowledge Domain
            Feature Know1 = new Feature ("Knowledge Domain", "Blessings of Knowledge", 1);
            Feature Know2 = new Feature ("Knowledge Domain", "Channel Divinity: Knowledge of the Ages", 2);
            Feature Know3 = new Feature ("Knowledge Domain", "Channel Divinity: Read Thoughts", 6);
            Feature Know4 = new Feature ("Knowledge Domain", "Potent Spellcasting", 8);
            Feature Know5 = new Feature ("Knowledge Domain", "Visions of the Past", 17);
        //Life Domain
            Feature Life1 = new Feature ("Life Domain", "Bonus Proficiency (Heavy Armor)", 1);
            Feature Life2 = new Feature ("Life Domain", "Disciple of Life", 1);
            Feature Life3 = new Feature ("Life Domain", "Channel Divinity: Preserve Life", 2);
            Feature Life4 = new Feature ("Life Domain", "Blessed Healer", 6);
            Feature Life5 = new Feature ("Life Domain", "Divine Strike", 8);
            Feature Life6 = new Feature ("Life Domain", "Supreme Healing", 17);
        //Light Domain
            Feature Light1 = new Feature ("Light Domain", "Bonus Cantrip (Light Cantrip)", 1);
            Feature Light2 = new Feature ("Light Domain", "Warding Flare", 1);
            Feature Light3 = new Feature ("Light Domain", "Channel Divinity: Radiance of the Dawn", 2);
            Feature Light4 = new Feature ("Light Domain", "Improved Flare", 6);
            Feature Light5 = new Feature ("Light Domain", "Potent Spellcasting", 8);
            Feature Light6 = new Feature ("Light Domain", "Corona of Light", 17);
        //Nature Domain
            Feature Nature1 = new Feature ("Nature Domain", "Acolyte of Nature", 1);
            Feature Nature2 = new Feature ("Nature Domain", "Bonus Proficiency (Heavy Armor)", 1);
            Feature Nature3 = new Feature ("Nature Domain", "Channel Divinity: Charm Animals and Plants", 2);
            Feature Nature4 = new Feature ("Nature Domain", "Dampen Elements", 6);
            Feature Nature5 = new Feature ("Nature Domain", "Divine Strike", 8);
            Feature Nature6 = new Feature ("Nature Domain", "Master of Nature", 17);
        //Tempest Domain
            Feature Tempest1 = new Feature ("Tempest Domain", "Bonus Proficiency (Martial Weapons, Heavy Armor)", 1);
            Feature Tempest2 = new Feature ("Tempest Domain", "Wrath of the Storm", 1);
            Feature Tempest3 = new Feature ("Tempest Domain", "Channel Divinity: Destructive Wrath", 2);
            Feature Tempest4 = new Feature ("Tempest Domain", "Thunderbolt Strike", 6);
            Feature Tempest5 = new Feature ("Tempest Domain", "Divine Strike", 8);
            Feature Tempest6 = new Feature ("Tempest Domain", "Stormborn", 17);
        //Trickery Domain
            Feature Trickery1 = new Feature ("Trickery Domain", "Blessing of the Trickster", 1);
            Feature Trickery2 = new Feature ("Trickery Domain", "Channel Divinity: Invoke Duplicity", 2);
            Feature Trickery3 = new Feature ("Trickery Domain", "Channel Divinity: Cloak of Shadows", 6);
            Feature Trickery4 = new Feature ("Trickery Domain", "Divine Strike", 8);
            Feature Trickery5 = new Feature ("Trickery Domain", "Improved Duplicity", 17);
        //War Domain
            Feature War1 = new Feature ("War Domain", "Bonus Proficiency (Martial Weapons, Heavy Armor)", 1);
            Feature War2 = new Feature ("War Domain", "War Priest", 1);
            Feature War3 = new Feature ("War Domain", "Channel Divinity: Guided Strike", 2);
            Feature War4 = new Feature ("War Domain", "Channel Divinity: War God's Blessing", 6);
            Feature War5 = new Feature ("War Domain", "Divine Strike", 8);
            Feature War6 = new Feature ("War Domain", "Avatar of Battle", 17);
    //Druid
            Feature Druid1 = new Feature("Druid", "Spellcasting (Druid)", 1);
            Feature Druid1_1 = new Feature("Druid", "Druidic", 1);
            Feature Druid2 = new Feature("Druid", "Wild Shape", 2);
            Feature Druid4 = new Feature("Druid", "Wild Shape Improvement (Level 4)", 4);
            Feature Druid8 = new Feature("Druid", "Wild Shape Improvement(Level 8)", 8);
            Feature Druid18 = new Feature("Druid", "Timeless Body", 18);
            Feature Druid18_1 = new Feature("Druid", "Beast Spells", 18);
            Feature Druid20 = new Feature("Druid", "Archdruid", 20);
        //Circle of the Land - *If we can do a Linq Query for Contains on Circle of the Land, we can add all of these, I think. Then, when adding spells, we can do a similar function for the Land keywords (e.g. Arctic, Grassland, etc.)
            Feature Land1 = new Feature ("Circle of the Land", "Bonus Cantrip (Any Druid Cantrip)", 2);
            Feature Land2 = new Feature ("Circle of the Land", "Natural Recovery", 2);
            Feature Land3 = new Feature ("Circle of the Land", "Land's Stride", 6);
            Feature Land4 = new Feature ("Circle of the Land", "Nature's Ward", 10);
            Feature Land5 = new Feature ("Circle of the Land", "Nature's Sanctuary", 14);
        //Circle of the Moon
            Feature Moon1 = new Feature ("Circle of the Moon", "Combat Wild Shape", 2);
            Feature Moon2 = new Feature ("Circle of the Moon", "Circle Forms", 2);
            Feature Moon3 = new Feature ("Circle of the Moon", "Primal Strike", 6);
            Feature Moon4 = new Feature ("Circle of the Moon", "Elemental Wild Shape", 10);
            Feature Moon5 = new Feature ("Circle of the Moon", "Thousand Forms", 14);
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
        //Champion
            Feature champ1 = new Feature("Champion", "Improved Critical", 3);
            Feature champ2 = new Feature("Champion", "Remarkable Athlete", 7);
            Feature champ3 = new Feature("Champion", "Additional Fighting Style", 10);
            Feature champ4 = new Feature("Champion", "Superior Critical Critical", 15);
            Feature champ5 = new Feature("Champion", "Survivor", 18);
        //Battle Master
            Feature bm1 = new Feature("Battle Master", "Combat Superiority", 3);
            Feature bm2 = new Feature("Battle Master", "Combat Student of War", 3);
            Feature bm3 = new Feature("Battle Master", "Student of War", 7);
            Feature bm4 = new Feature("Battle Master", "Improved Combat Superiority", 10);
            Feature bm5 = new Feature("Battle Master", "Relentless", 15);
        //Eldritch Knight
            Feature ek1 = new Feature("Eldritch Knight", "Spellcasting (Eldritch Knight)", 3);
            Feature ek2 = new Feature("Eldritch Knight", "War Magic", 7);
            Feature ek3 = new Feature("Eldritch Knight", "Eldritch Strike", 10);
            Feature ek4 = new Feature("Eldritch Knight", "Arcane Charge", 15);
            Feature ek5 = new Feature("Eldritch Knight", "Improved War Magic", 18);
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
        //Way of the Open Hand
            Feature openhand1 = new Feature ("Way of the Open Hand", "Open Hand Technique", 3);
            Feature openhand2 = new Feature ("Way of the Open Hand", "Wholeness of Body", 6);
            Feature openhand3 = new Feature ("Way of the Open Hand", "Tranquility", 11);
            Feature openhand4 = new Feature ("Way of the Open Hand", "Quivering Palm", 17);
        //Way of the Four Elements
            Feature avatar1 = new Feature ("Way of the Four Elements", "Disciple of the Elements", 3); //*Requires detail, goes into a number of potential options
        //Way of Shadow
            Feature shadow1 = new Feature ("Way of Shadow", "Shadow Arts", 3);
            Feature shadow2 = new Feature ("Way of Shadow", "Shadow Step", 6);
            Feature shadow3 = new Feature ("Way of Shadow", "Cloak of Shadows", 11);
            Feature shadow4 = new Feature ("Way of Shadow", "Opportunist", 17);
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
        //Oath of Devotion
            Feature OoD1 = new Feature ("Oath of Devotion", "Channel Divinity: Sacred Weapon/Turn the Unholy", 3);
            Feature OoD2 = new Feature ("Oath of Devotion", "Aura of Devotion", 7);
            Feature OoD3 = new Feature ("Oath of Devotion", "Purity of Spirit", 15);
            Feature OoD4 = new Feature ("Oath of Devotion", "Holy Nimbus", 20);
        //Oath of Vengeance
            Feature OoV1 = new Feature ("Oath of Vengeance", "Channel Divinity: Abjure Enemy/Vow of Enmity", 3);
            Feature OoV2 = new Feature ("Oath of Vengeance", "Relentless Avenger", 7);
            Feature OoV3 = new Feature ("Oath of Vengeance", "Soul of Vengeance", 15);
            Feature OoV4 = new Feature ("Oath of Vengeance", "Avenging Angel", 20);
        //Oath of the Ancients
            Feature OotA1 = new Feature ("Oath of the Ancients", "Channel Divinity: Nature's Wrath/Turn the Faithless", 3);
            Feature OotA2 = new Feature ("Oath of the Ancients", "Aura of Warding", 7);
            Feature OotA3 = new Feature ("Oath of the Ancients", "Undying Sentinel", 15);
            Feature OotA4 = new Feature ("Oath of the Ancients", "Elder Champion", 20);
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
        //Hunter
            Feature hunt1 = new Feature ("Hunter", "Hunter's Prey", 3); //*Choice
            Feature hunt2 = new Feature ("Hunter", "Defensive Tactics", 7); //*Choice
            Feature hunt3 = new Feature ("Hunter", "Multiattack", 11); //*Choice
            Feature hunt4 = new Feature ("Hunter", "Superior Hunter's Defense", 15); //*Choice
        //Beast Master
            Feature BM1 = new Feature ("Beast Master", "Ranger's Companion", 3);
            Feature BM2 = new Feature ("Beast Master", "Exceptional Training", 7);
            Feature BM3 = new Feature ("Beast Master", "Bestial Fury", 11);
            Feature BM4 = new Feature ("Beast Master", "Share Spells", 15);
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
        //Thief
            Feature thf1 = new Feature ("Thief", "Fast Hands", 3);
            Feature thf2 = new Feature ("Thief", "Second-Story Work", 3);
            Feature thf3 = new Feature ("Thief", "Supreme Sneak", 9);
            Feature thf4 = new Feature ("Thief", "Use Magic Device", 13);
            Feature thf5 = new Feature ("Thief", "Thief's Reflexes", 17);
        //Assassin
            Feature assassin1 = new Feature ("Assassin", "Bonus Proficiencies (Disguise Kit, Poisoner's Kit)", 3);
            Feature assassin2 = new Feature ("Assassin", "Assassinate", 3);
            Feature assassin3 = new Feature ("Assassin", "Infiltration Expertise", 9);
            Feature assassin4 = new Feature ("Assassin", "Imposter", 13);
            Feature assassin5 = new Feature ("Assassin", "Death Strike", 17);
        //Arcane Trickster
            Feature AT1 = new Feature ("Arcane Trickster", "Spellcasting (Arcane Trickster)", 3);
            Feature AT2 = new Feature ("Arcane Trickster", "Mage Hand Legerdemain", 3);
            Feature AT3 = new Feature ("Arcane Trickster", "Magical Ambush", 9);
            Feature AT4 = new Feature ("Arcane Trickster", "Versatile Trickster", 13);
            Feature AT5 = new Feature ("Arcane Trickster", "Spell Thief", 17);
    //Sorcerer
            Feature sorc1 = new Feature ("Sorcerer", "Spellcasting (Sorcerer)", 1);
            Feature sorc2 = new Feature ("Sorcerer", "Font of Magic", 2);
            Feature sorc3 = new Feature ("Sorcerer", "Metamagic (Level 3)", 3); //Have to generate options
            Feature sorc10 = new Feature ("Sorcerer", "Metamagic (Level 10)", 10);
            Feature sorc17 = new Feature ("Sorcerer", "Metamagic (Level 17)", 17);
            Feature sorc20 = new Feature ("Sorcerer", "Sorcerous Restoration", 20);
        //Draconic Bloodline
            Feature DB1 = new Feature ("Draconic Bloodline", "Dragon Ancestor", 1);//*Choice
            Feature DB2 = new Feature ("Draconic Bloodline", "Draconic Resilience", 1);
            Feature DB3 = new Feature ("Draconic Bloodline", "Elemental Affinity", 6); //Affected by Dragon Ancestor Choice
            Feature DB4 = new Feature ("Draconic Bloodline", "Dragon Wings", 14);
            Feature DB5 = new Feature ("Draconic Bloodline", "Draconic Presence", 18);
        //Wild Magic
            Feature WM1 = new Feature ("Wild Magic", "Wild Magic Surge", 1);
            Feature WM2 = new Feature ("Wild Magic", "Tides of Chaos", 1);
            Feature WM3 = new Feature ("Wild Magic", "Bend Luck", 6);
            Feature WM4 = new Feature ("Wild Magic", "Controlled Chaos", 14);
            Feature WM5 = new Feature ("Wild Magic", "Spell Bombardment", 18);
    //Warlock
            Feature lock0 = new Feature ("Warlock", "Pact Magic", 1);
            Feature lock2 = new Feature ("Warlock", "Eldritch Invocations", 2); //Need to expand based on Invocations
            Feature lock3 = new Feature ("Warlock", "Pact Boon", 3); // Need to expand on this to generate options, probably list each pact boon under this and when Pact boon is recieved, add one of the options to the list
            Feature lock11 = new Feature ("Warlock", "Mystic Arcanum (Level 6)", 11);
            Feature lock13 = new Feature ("Warlock", "Mystic Arcanum (Level 7)", 13);
            Feature lock15 = new Feature ("Warlock", "Mystic Arcanum (Level 8)", 15);
            Feature lock17 = new Feature ("Warlock", "Mystic Arcanum (Level 9)", 17);
            Feature lock20 = new Feature ("Warlock", "Eldritch Master", 20);
        //The Archfey
            Feature af1 = new Feature ("The Archfey", "Fey Presence", 1);
            Feature af2 = new Feature ("The Archfey", "Misty Escape", 6);
            Feature af3 = new Feature ("The Archfey", "Beguiling Defenses", 10);
            Feature af4 = new Feature ("The Archfey", "Dark Delirium", 14);
        //The Great Old One
            Feature GOO1 = new Feature ("The Great Old One", "Awakened Mind", 1);
            Feature GOO2 = new Feature ("The Great Old One", "Entropic Ward", 6);
            Feature GOO3 = new Feature ("The Great Old One", "Thought Shield", 10);
            Feature GOO4 = new Feature ("The Great Old One", "Create Thrall", 14);
        //The Fiend
            Feature fnd1 = new Feature ("The Fiend", "Dark One's Blessing", 1);
            Feature fnd2 = new Feature ("The Fiend", "Dark One's Own Luck", 6);
            Feature fnd3 = new Feature ("The Fiend", "Fiendish Resilience", 10);
            Feature fnd4 = new Feature ("The Fiend", "Hurl Through Hell", 14);
    //Wizard
            Feature Wiz1 = new Feature ("Wizard", "Spellcasting (Wizard)", 1);
            Feature Wiz1_1 = new Feature ("Wizard", "Arcane Recovery", 1);
            Feature Wiz18 = new Feature ("Wizard", "Spell Mastery", 18);
            Feature Wiz20 = new Feature ("Wizard", "Signature Spell", 20);
        //School of Abjuration
            Feature abj1 = new Feature ("School of Abjuration", "Abjuration Savant", 2);
            Feature abj2 = new Feature ("School of Abjuration", "Arcane Ward", 2);
            Feature abj3 = new Feature ("School of Abjuration", "Projected Ward", 6);
            Feature abj4 = new Feature ("School of Abjuration", "Improved Abjuration", 10);
            Feature abj5 = new Feature ("School of Abjuration", "Spell Resistance", 14);
        //School of Conjuration
            Feature conj1 = new Feature ("School of Conjuration", "Conjuration Savant", 2);
            Feature conj2 = new Feature ("School of Conjuration", "Minor Conjuration", 2);
            Feature conj3 = new Feature ("School of Conjuration", "Benign Transposition", 6);
            Feature conj4 = new Feature ("School of Conjuration", "Focused Conjuration", 10);
            Feature conj5 = new Feature ("School of Conjuration", "Durable Summons", 14);
        //School of Divination
            Feature div1 = new Feature ("School of Divination", "Divination Savant", 2);
            Feature div2 = new Feature ("School of Divination", "Portent", 2);
            Feature div3 = new Feature ("School of Divination", "Expert Divination", 6);
            Feature div4 = new Feature ("School of Divination", "The Third Eye", 10);
            Feature div5 = new Feature ("School of Divination", "Greater Portent", 14);
        //School of Enchantment
            Feature enc1 = new Feature ("School of Enchantment", "Enchantment Savant", 2);
            Feature enc2 = new Feature ("School of Enchantment", "Hypnotic Gaze", 2);
            Feature enc3 = new Feature ("School of Enchantment", "Instinctive Charm", 6);
            Feature enc4 = new Feature ("School of Enchantment", "Split Enchantment", 10);
            Feature enc5 = new Feature ("School of Enchantment", "Alter Memories", 14);
        //School of Evocation
            Feature evo1 = new Feature ("School of Evocation", "Evocation Savant", 2);
            Feature evo2 = new Feature ("School of Evocation", "Sculpt Spells", 2);
            Feature evo3 = new Feature ("School of Evocation", "Potent Cantrip", 6);
            Feature evo4 = new Feature ("School of Evocation", "Empowered Evocation", 10);
            Feature evo5 = new Feature ("School of Evocation", "Overchannel", 14);
        //School of Illusion
            Feature illu1 = new Feature ("School of Illusion", "Illusion Savant", 2);
            Feature illu2 = new Feature ("School of Illusion", "Improved Minor Illusion", 2);
            Feature illu3 = new Feature ("School of Illusion", "Malleable Illusion", 6);
            Feature illu4 = new Feature ("School of Illusion", "Illusory Self", 10);
            Feature illu5 = new Feature ("School of Illusion", "Illusory Reality", 14);
        //School of Necromancy
            Feature necro1 = new Feature ("School of Necromancy", "Necromancy Savant", 2);
            Feature necro2 = new Feature ("School of Necromancy", "Grim Harvest", 2);
            Feature necro3 = new Feature ("School of Necromancy", "Undead Thralls", 6);
            Feature necro4 = new Feature ("School of Necromancy", "Inured to Death", 10);
            Feature necro5 = new Feature ("School of Necromancy", "Command Undead", 14);
        //School of Transmutation
            Feature trans1 = new Feature ("School of Transmutation", "Transmutation Savant", 2);
            Feature trans2 = new Feature ("School of Transmutation", "Minor Alchemy", 2);
            Feature trans3 = new Feature ("School of Transmutation", "Transmuter's Stone", 6);
            Feature trans4 = new Feature ("School of Transmutation", "Shapechanger", 10);
            Feature trans5 = new Feature ("School of Transmutation", "Master Transmuter", 14);

        _context.SaveChanges();
        }
    }
}
