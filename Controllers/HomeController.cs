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

                // Dynamic model to provide all available objects and data
                dynamic MyModel = new ExpandoObject();
                MyModel.User = userCheck;
                MyModel.Login = new Login();
                MyModel.Character = test; 
                MyModel.Features = Feats;

                return View("index", MyModel);
            } 

            // if DB is populated with spells, feats, 1 character and 1 user are already present
            else
            {
            // Home screen selects and shows previously generated characters. IF players would like to generate new characters or save them, they must login
            int? SessionId = HttpContext.Session.GetInt32("UserId");
            User SessionUser = _context.Users.FirstOrDefault( u => u.UserId == SessionId);
            if(SessionUser == null)
            {
                SessionUser = _context.Users.FirstOrDefault( u => u.UserId == 1);
            }

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

            // Pass Model User, Character, Feats
            dynamic MyModel = new ExpandoObject();
            MyModel.User = SessionUser;
            MyModel.Login = new Login();
            MyModel.Character = character;
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

            List<Feature> Feats = _context.NewCharacter //Starting Construction from the character side
                        .Include(c => c.FeaturesList) // Including The Association Table List in construction
                        .ThenInclude(fa => fa.FeatureA) //Including items within the Association Table
                        .FirstOrDefault(c => c.CharacterId == newPlayer.CharacterId) // Selecting the character themselves
                        .FeaturesList.Select(f => f.FeatureA) // Selecting the Feats
                        .OrderBy(f => f.FeatLevel) // Ascending order for Feats
                        .ToList();

            // Skills/Spells only works for wiz
            // List<Spell> allSpells = GetPossibleSpells(newPlayer);
            // Console.WriteLine("TEST TWO ***TEST TWO ***");
            // Console.WriteLine(allSpells);
            // foreach (Spell spell in allSpells)
            // {
            //     Console.WriteLine(spell.SpellName);
            // }

            //     Console.WriteLine("TEST TWO ***TEST TWO ***");
            //     List<Spell> availableSpells = AvaialableSpellsWizard(allSpells, newPlayer);
            //     Console.WriteLine("TEST THREE ***TEST THREE ***");
            //     Console.WriteLine(availableSpells);
            //     foreach (Spell spell in availableSpells)
            //     {
            //         Console.WriteLine(spell.SpellName);
            //     }
            //     Console.WriteLine("TEST THREE ***TEST THREE ***");
            //     foreach(Spell added in availableSpells )
            //     {
            //         SpellAssoc a = new SpellAssoc(newPlayer, added);
            //         _context.Spell_Associations.Add(a);
            //         _context.SaveChanges();
            //     }
            
            
            
            //Dynamic model with USer, Login, Character
            dynamic MyModel = new ExpandoObject();
            MyModel.User = SessionUser;
            MyModel.Login = new Login();
            MyModel.Character = newPlayer;
            MyModel.Features = Feats;
            // MyModel.Spells = availableSpells;

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


        // spell stufff VVVVVVVVVVVV
       
        public List<Spell> GetPossibleSpells(NewCharacter PC)
        {
            
            List<Spell> A = _context.Spells.Where(s => 
                              s.Source1 == PC.playerClass.ClassName
                            ||s.Source2 == PC.playerClass.ClassName
                            ||s.Source3 == PC.playerClass.ClassName
                            ||s.Source4 == PC.playerClass.ClassName
                            ||s.Source5 == PC.playerClass.ClassName
                            ||s.Source6 == PC.playerClass.ClassName
                            ||s.Source7 == PC.playerClass.ClassName
                            ).ToList();
            Console.WriteLine("START Get Possible Spells A ++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine(A);
            foreach(Spell s in A)
            {
                Console.WriteLine(s.SpellName);
            }
            Console.WriteLine("END Get Possible Spells A ++++++++++++++++++++++++++++++++++++++");
            
            return A;
        }

        //Thank you GFG, this should help out immensely.
        public static List<Spell> RandomizeSpells(List<Spell> arr, int n) 
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
                Spell temp = arr.ElementAt(i); 
                arr[i] = arr.ElementAt(j); 
                arr[j] = temp; 
            }     
            return arr; 
        }

        //
        // Avaialable Spells, Wizard. Gets the spells learned by the wizard by level. Seems to work
        public List<Spell> AvaialableSpellsWizard(List<Spell> fullListAvail, NewCharacter PC)
        {
            List<Spell> Cantrips = fullListAvail.Where(s => s.SpellLevel == 0).ToList();
            RandomizeSpells(Cantrips, Cantrips.Count);
            List<Spell> levelOne = fullListAvail.Where(s => s.SpellLevel == 1).ToList();
            RandomizeSpells(levelOne, levelOne.Count);
            List<Spell> levelTwo = fullListAvail.Where(s => s.SpellLevel == 2).ToList();
            RandomizeSpells(levelTwo, levelTwo.Count);
            List<Spell> levelThree = fullListAvail.Where(s => s.SpellLevel == 3).ToList();
            RandomizeSpells(levelThree, levelThree.Count);
            List<Spell> levelFour = fullListAvail.Where(s => s.SpellLevel == 4).ToList();
            RandomizeSpells(levelFour, levelFour.Count);
            List<Spell> levelFive = fullListAvail.Where(s => s.SpellLevel == 5).ToList();
            RandomizeSpells(levelFive, levelFive.Count);
            List<Spell> levelSix = fullListAvail.Where(s => s.SpellLevel == 6).ToList();
            RandomizeSpells(levelSix, levelSix.Count);
            List<Spell> levelSeven = fullListAvail.Where(s => s.SpellLevel == 7).ToList();
            RandomizeSpells(levelSeven, levelSeven.Count);
            List<Spell> levelEight = fullListAvail.Where(s => s.SpellLevel == 8).ToList();
            RandomizeSpells(levelEight, levelEight.Count);
            List<Spell> levelNine = fullListAvail.Where(s => s.SpellLevel == 9).ToList();
            RandomizeSpells(levelNine, levelNine.Count);
            
            List<Spell> availableSpells = new List<Spell>();
            //Cantrips
            if (PC.Level >= 10)
                {
                    for (int c = 4; c> -1; c--)
                    {
                        availableSpells.Add(Cantrips[c]);
                    }
                }
                else if (PC.Level >= 4)
                    {
                        for (int c = 3; c> -1; c--)
                        {
                            availableSpells.Add(Cantrips[c]);
                            
                        }
                    }
                    else
                        {
                            for (int c = 2; c> -1; c--)
                            {
            
                                availableSpells.Add(Cantrips[c]);

                            }
                        }

            int l2 = 0;
            int l3 = 0;
            int l4 = 0;
            int l5 = 0;
            int l6 = 0;
            int l7 = 0;
            int l8 = 0;
            int l9 = 0;

            for(int i=1; i<=PC.Level; i++) // I was thinking of while loops lol
            {
                if(i == 1) // Can be modified for Paladins and Rangers to start @ 2
                    {
                        for(int j = 5; j>-1; j--)
                        {
                            //Add level1 association, twice to represent spells gained on level up
                            availableSpells.Add(levelOne[j]);
                        }
                    }
                else
                {
                    int Spell_Level_Available =(int)Math.Ceiling(.5*i);
                    switch(Spell_Level_Available)
                        {
                            case 1:
                                //Add level1 association, twice to represent spells gained on level up
                                availableSpells.Add(levelOne[7]);
                                availableSpells.Add(levelOne[8]);
                                break;
                            case 2:
                                //Add level2 association, twice to represent spells gained on level up
                                availableSpells.Add(levelTwo[l2]);
                                l2+=1;
                                availableSpells.Add(levelTwo[l2]);
                                l2+=1;
                                break;
                            case 3:
                                //Add level3 association, twice to represent spells gained on level up
                                availableSpells.Add(levelThree[l3]);
                                l3+=1;
                                availableSpells.Add(levelThree[l3]);
                                l3+=1;
                                break;
                            case 4:
                                //Add level4 association, twice to represent spells gained on level up
                                availableSpells.Add(levelFour[l4]);
                                l4+=1;
                                availableSpells.Add(levelFour[l4]);
                                l4+=1;
                                break;
                            case 5:
                                //Add level5 association, twice to represent spells gained on level up
                                availableSpells.Add(levelFive[l5]);
                                l5+=1;
                                availableSpells.Add(levelFive[l5]);
                                l5+=1;
                                break;
                            case 6:
                                //Add level6 association, twice to represent spells gained on level up
                                availableSpells.Add(levelSix[l6]);
                                l6+=1;
                                availableSpells.Add(levelSix[l6]);
                                l6+=1;
                                break;
                            case 7:
                                //Add level7 association, twice to represent spells gained on level up
                                availableSpells.Add(levelSeven[l7]);
                                l7+=1;
                                availableSpells.Add(levelSeven[l7]);
                                l7+=1;
                                break;
                            case 8:
                                //Add level8 association, twice to represent spells gained on level up
                                availableSpells.Add(levelEight[l8]);
                                l8+=1;
                                availableSpells.Add(levelEight[l8]);
                                l8+=1;
                                break;
                            case 9:
                                //Add level9 association, twice to represent spells gained on level up
                                availableSpells.Add(levelNine[l9]);
                                l9+=1;
                                availableSpells.Add(levelNine[l9]);
                                l9+=1;
                                break;
                            case 10:
                                //Add level9 association, twice to represent spells gained on level up
                                availableSpells.Add(levelNine[l9]);
                                l9+=1;
                                availableSpells.Add(levelNine[l9]);
                                l9+=1;
                                break;
                        }
                }
            }
            return availableSpells;
        }

        public int DetermineSpellsPreppedWiz(NewCharacter PC)
            {
                int preppedspells = PC.playerStat.IntMod + PC.Level;
                if (preppedspells < 1)
                    {
                        preppedspells = 1;
                    }
                return preppedspells;
            }
        
        public int DetermineSpellsPreppedClerDru(NewCharacter PC)
            {
                int preppedspells = PC.playerStat.WisMod + PC.Level;
                if (preppedspells < 1)
                    {
                        preppedspells = 1;
                    }
                return preppedspells;
            }

        //Bard Spell grabber, requires maitnence. Bards have a total of 16 spells from their own list that they acquire in a staggered progression. This can be a good test bed for how the sorcerer may also work.
        //Current idea: Bards have the ability to switch out spells that they currently know with another of the level that they have the ability to use. Randomize the list of spells that they currently have on level up, remove the last one, and then add 2 from the bard list for that level. Once they hit 9th level spells, we can have it select from levels 5-7 at random, continuing iterations of l5-l7 to select the appropriate spell to add, and switchthing them out accordingly. 
        //Magical Secrets is its own beast, but it can be done by adding all of the appropriate spells via LINQ query where the sources are !Bard, and adding them at the appropriate level.
        //Total Bard spells by 20: 18
        //Bard level 1 spells: 4
        public List<Spell> AvaialableSpellsBard(List<Spell> fullListAvail, NewCharacter PC)
        {
            List<Spell> Cantrips = fullListAvail.Where(s => s.SpellLevel == 0).ToList();
            RandomizeSpells(Cantrips, Cantrips.Count);
            List<Spell> levelOne = fullListAvail.Where(s => s.SpellLevel == 1).ToList();
            RandomizeSpells(levelOne, levelOne.Count);
            List<Spell> levelTwo = fullListAvail.Where(s => s.SpellLevel == 2).ToList();
            RandomizeSpells(levelTwo, levelTwo.Count);
            List<Spell> levelThree = fullListAvail.Where(s => s.SpellLevel == 3).ToList();
            RandomizeSpells(levelThree, levelThree.Count);
            List<Spell> levelFour = fullListAvail.Where(s => s.SpellLevel == 4).ToList();
            RandomizeSpells(levelFour, levelFour.Count);
            List<Spell> levelFive = fullListAvail.Where(s => s.SpellLevel == 5).ToList();
            RandomizeSpells(levelFive, levelFive.Count);
            List<Spell> levelSix = fullListAvail.Where(s => s.SpellLevel == 6).ToList();
            RandomizeSpells(levelSix, levelSix.Count);
            List<Spell> levelSeven = fullListAvail.Where(s => s.SpellLevel == 7).ToList();
            RandomizeSpells(levelSeven, levelSeven.Count);
            List<Spell> levelEight = fullListAvail.Where(s => s.SpellLevel == 8).ToList();
            RandomizeSpells(levelEight, levelEight.Count);
            List<Spell> levelNine = fullListAvail.Where(s => s.SpellLevel == 9).ToList();
            RandomizeSpells(levelNine, levelNine.Count);
            
            List<Spell> availableSpells = new List<Spell>();

            int l2 = 0;
            int l3 = 0;
            int l4 = 0;
            int l5 = 0;
            int l6 = 0;
            int l7 = 0;
            int l8 = 0;
            int l9 = 0;

            for(int i=1; i<=PC.Level; i++)
            {
                if(i == 1) // Can be modified for Paladins and Rangers to start @ 2
                    {
                        for(int j = 3; j>-1; j--)
                        {
                            //Add level1 association, twice to represent spells gained on level up
                            availableSpells.Add(levelOne[j]);
                        }
                    }
                    //4 spells base
                else
                {
                    int Spell_Level_Available =(int)Math.Ceiling(.5*i);
                    switch(Spell_Level_Available)
                        {
                            case 1:
                                //Add level1 association, twice to represent spells gained on level up
                                availableSpells.Add(levelOne[4]);
                                break;
                                //5 level 1 spells, 5 total
                            case 2:
                                //Add level2 association, twice to represent spells gained on level up
                                availableSpells.Add(levelTwo[l2]);
                                l2+=1;
                                break;
                                //5 level 1 spells, 1/2 level 2 spells, 6/7 total
                            case 3:
                                //Add level3 association, twice to represent spells gained on level up
                                availableSpells.Add(levelThree[l3]);
                                l3+=1;
                                break;
                                //5 level 1 spells, 2 level 2 spells, 1/2 level 3 spells 8/9 total
                            case 4:
                                //Add level4 association, twice to represent spells gained on level up
                                availableSpells.Add(levelFour[l4]);
                                l4+=1;
                                break;
                                //5 level 1 spells, 2 level 2 spells, 2 level 3 spells, 1/2 level 4 spells 10/11 total
                            case 5:
                                if (i == 10)
                                    {
                                        break;
                                    }
                                availableSpells.Add(levelFive[l5]);
                                l5+=1;
                                break;
                            case 6:
                                if (i == 12)
                                    {
                                        break;
                                    }
                                availableSpells.Add(levelSix[l6]);
                                l6+=1;
                                break;
                            case 7:
                                if (i == 14)
                                    {
                                        break;
                                    }
                                availableSpells.Add(levelSeven[l7]);
                                l7+=1;
                                break;
                            case 8:
                                if (i == 16)
                                    {
                                        break;
                                    }
                                availableSpells.Add(levelEight[l8]);
                                l8+=1;
                                break;
                            case 9:
                                if (i > 17)
                                {
                                    break;
                                }
                                availableSpells.Add(levelNine[l9]);
                                l9+=1;
                                break;
                            case 10:
                                if (i > 17)
                                {
                                    break;
                                }
                                availableSpells.Add(levelNine[l9]);
                                l9+=1;
                                break;
                        }
                }
            }
            //Cantrips. Made to reflect Bard progression
            if (PC.Level >= 10) //level 10 to final character cantrips
                {
                    for (int c = 3; c> -1; c--)
                    {
                        availableSpells.Add(Cantrips[c]);
                    }
                }
                else if (PC.Level >= 4) //Level 4 character cantrips
                    {
                        for (int c = 2; c> -1; c--)
                        {
                            availableSpells.Add(Cantrips[c]);
                        }
                    }
                    else// Base Character cantrips
                        {
                            for (int c = 1; c> -1; c--)
                            {
                                availableSpells.Add(Cantrips[c]);
                            }
                        }
                        //Magical Secrets
                        if (PC.Level >= 10)
                            { //Grabbing all spells NOT on the Bard Spell list
                            List<Spell> A = _context.Spells.Where(s => 
                                s.Source1 != PC.playerClass.ClassName && s.SpellLevel == 5
                                ||s.Source2 != PC.playerClass.ClassName && s.SpellLevel == 5
                                ||s.Source3 != PC.playerClass.ClassName && s.SpellLevel == 5
                                ||s.Source4 != PC.playerClass.ClassName && s.SpellLevel == 5
                                ||s.Source5 != PC.playerClass.ClassName && s.SpellLevel == 5
                                ||s.Source6 != PC.playerClass.ClassName && s.SpellLevel == 5
                                ||s.Source7 != PC.playerClass.ClassName && s.SpellLevel == 5
                                ).ToList();
                            RandomizeSpells(A, A.Count);
                            availableSpells.Add(A[0]);
                            availableSpells.Add(A[1]);
                            }
                        if (PC.Level >= 14)
                            {
                            List<Spell> B = _context.Spells.Where(s => 
                                s.Source1 != PC.playerClass.ClassName && s.SpellLevel == 7
                                ||s.Source2 != PC.playerClass.ClassName && s.SpellLevel == 7
                                ||s.Source3 != PC.playerClass.ClassName && s.SpellLevel == 7
                                ||s.Source4 != PC.playerClass.ClassName && s.SpellLevel == 7
                                ||s.Source5 != PC.playerClass.ClassName && s.SpellLevel == 7
                                ||s.Source6 != PC.playerClass.ClassName && s.SpellLevel == 7
                                ||s.Source7 != PC.playerClass.ClassName && s.SpellLevel == 7
                                ).ToList();
                            RandomizeSpells(B, B.Count);
                            availableSpells.Add(B[0]);
                            availableSpells.Add(B[1]);
                            }
                        if (PC.Level >= 18)
                            {
                            List<Spell> C = _context.Spells.Where(s => 
                                s.Source1 != PC.playerClass.ClassName && s.SpellLevel == 9
                                ||s.Source2 != PC.playerClass.ClassName && s.SpellLevel == 9
                                ||s.Source3 != PC.playerClass.ClassName && s.SpellLevel == 9
                                ||s.Source4 != PC.playerClass.ClassName && s.SpellLevel == 9
                                ||s.Source5 != PC.playerClass.ClassName && s.SpellLevel == 9
                                ||s.Source6 != PC.playerClass.ClassName && s.SpellLevel == 9
                                ||s.Source7 != PC.playerClass.ClassName && s.SpellLevel == 9
                                ).ToList();
                            RandomizeSpells(C, C.Count);
                            availableSpells.Add(C[0]);
                            availableSpells.Add(C[1]);
                            }
                        if (PC.Level >= 6 && PC.playerClass.SubClassName == "College of Lore")
                            {
                            List<Spell> D = _context.Spells.Where(s => 
                                s.Source1 != PC.playerClass.ClassName && s.SpellLevel == 3
                                ||s.Source2 != PC.playerClass.ClassName && s.SpellLevel == 3
                                ||s.Source3 != PC.playerClass.ClassName && s.SpellLevel == 3
                                ||s.Source4 != PC.playerClass.ClassName && s.SpellLevel == 3
                                ||s.Source5 != PC.playerClass.ClassName && s.SpellLevel == 3
                                ||s.Source6 != PC.playerClass.ClassName && s.SpellLevel == 3
                                ||s.Source7 != PC.playerClass.ClassName && s.SpellLevel == 3
                                ).ToList();
                            RandomizeSpells(D, D.Count);
                            availableSpells.Add(D[0]);
                            availableSpells.Add(D[1]);
                            }
            return availableSpells;
        }

        //Sorcerer spell grabber, requires maitnence
        //Similar structure for the Sorcerer as the Bard because of switching spells. Randomize the current list, drop one
        //As above with the Bard Cantrips, Sorcerer Cantrips be placed afterward to maintain Cantrip requirements by level.
        public List<Spell> AvaialableSpellsSorcerer(List<Spell> fullListAvail, NewCharacter PC)
        {
            List<Spell> Cantrips = fullListAvail.Where(s => s.SpellLevel == 0).ToList();
            RandomizeSpells(Cantrips, Cantrips.Count);
            List<Spell> levelOne = fullListAvail.Where(s => s.SpellLevel == 1).ToList();
            RandomizeSpells(levelOne, levelOne.Count);
            Console.WriteLine("levelOne");
            foreach(Spell spell in levelOne)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelTwo = fullListAvail.Where(s => s.SpellLevel == 2).ToList();
            RandomizeSpells(levelTwo, levelTwo.Count);
            Console.WriteLine("levelTwo");
            foreach(Spell spell in levelTwo)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelThree = fullListAvail.Where(s => s.SpellLevel == 3).ToList();
            RandomizeSpells(levelThree, levelThree.Count);
            Console.WriteLine("levelThree");
            foreach(Spell spell in fullListAvail)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelFour = fullListAvail.Where(s => s.SpellLevel == 4).ToList();
            RandomizeSpells(levelFour, levelFour.Count);
            Console.WriteLine("levelFour");
            foreach(Spell spell in levelFour)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelFive = fullListAvail.Where(s => s.SpellLevel == 5).ToList();
            RandomizeSpells(levelFive, levelFive.Count);
            Console.WriteLine("levelFive");
            foreach(Spell spell in levelFive)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelSix = fullListAvail.Where(s => s.SpellLevel == 6).ToList();
            RandomizeSpells(levelSix, levelSix.Count);
            Console.WriteLine("levelSix");
            foreach(Spell spell in levelSix)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelSeven = fullListAvail.Where(s => s.SpellLevel == 7).ToList();
            RandomizeSpells(levelSeven, levelSeven.Count);
            Console.WriteLine("levelSeven");
            foreach(Spell spell in levelSeven)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelEight = fullListAvail.Where(s => s.SpellLevel == 8).ToList();
            RandomizeSpells(levelEight, levelEight.Count);
            Console.WriteLine("levelEight");
            foreach(Spell spell in levelEight)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelNine = fullListAvail.Where(s => s.SpellLevel == 9).ToList();
            RandomizeSpells(levelNine, levelNine.Count);
            Console.WriteLine("levelNine");
            foreach(Spell spell in levelNine)
            {
                Console.WriteLine(spell.SpellName);
            }
            Console.WriteLine("Full list section end ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");

            List<Spell> availableSpells = new List<Spell>();


            //Based on Character's level, determine what spells known are chosen from the list available to the character
            //DetermineSpellLevel() can be modified to indicate access to spell tiers, using the Math.Ceiling functionality described there to determine spell level at the time that the spell could be chosen. Characters, through leveling progression, get access to different spells based upon their level and we can use that to determine what is added to their spells known.
            //Currently, we can use a For Loop(for(int i=1; i>ClassLevel; i++)) to determine spells known for a Wizard, grabbing from the appropriate list and adding it to what they know
            //Other classes will require some finangling, but the basic structure will be there once we understand how to implement it here.
            //Structure for spells: Available Spells (AS), Spells Known (SK) is a subset of AS, Prepared Spells is a subset of SK.
            //(A(S(PS)K)S) <- Loose diagram of the idea
            int l2 = 0;
            int l3 = 0;
            int l4 = 0;
            int l5 = 0;
            int l6 = 0;
            int l7 = 0;
            int l8 = 0;
            int l9 = 0;

            for(int i=1; i<=PC.Level; i++) // I was thinking of while loops lol
            {
                if(i == 1) // Can be modified for Paladins and Rangers to start @ 2
                    {
                        for(int j = 1; j>-1; j--)
                        {
                            //Add level1 association, twice to represent spells gained on level up
                            availableSpells.Add(levelOne[j]);// set to 1 for testing purposes
                        }
                    }
                else
                {
                    int Spell_Level_Available =(int)Math.Ceiling(.5*i);
                    Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                    Console.WriteLine(Spell_Level_Available);
                    switch(Spell_Level_Available)
                        {
                            case 1:
                                //Add level1 association, twice to represent spells gained on level up
                                availableSpells.Add(levelOne[2]);
                                break;
                            case 2:
                                //Add level2 association, twice to represent spells gained on level up
                                availableSpells.Add(levelTwo[1]);
                                Console.WriteLine(Spell_Level_Available);
                                Console.WriteLine(levelTwo[2]);
                                l2+=1;
                                break;
                            case 3:
                                //Add level3 association, twice to represent spells gained on level up
                                availableSpells.Add(levelThree[1]);
                                Console.WriteLine(Spell_Level_Available);
                                l3+=1;
                                break;
                            case 4:
                                //Add level4 association, twice to represent spells gained on level up
                                availableSpells.Add(levelFour[1]);
                                Console.WriteLine(Spell_Level_Available);
                                l4+=1;
                                break;
                            case 5:
                                //Add level5 association, twice to represent spells gained on level up
                                availableSpells.Add(levelFive[1]);
                                Console.WriteLine(Spell_Level_Available);
                                l5+=1;
                                break;
                            case 6:
                                //Add level6 association, twice to represent spells gained on level up
                                if (i == 12)
                                    {
                                        break;
                                    }
                                availableSpells.Add(levelSix[1]);
                                Console.WriteLine(Spell_Level_Available);
                                l6+=1;
                                break;
                            case 7:
                                //Add level7 association, twice to represent spells gained on level up
                                if (i == 14)
                                    {
                                        break;
                                    }
                                availableSpells.Add(levelSeven[1]);
                                Console.WriteLine(Spell_Level_Available);
                                l7+=1;
                                break;
                            case 8:
                                //Add level8 association, twice to represent spells gained on level up
                                if (i == 16)
                                    {
                                        break;
                                    }
                                availableSpells.Add(levelEight[1]);
                                Console.WriteLine(Spell_Level_Available);
                                l8+=1;
                                break;
                            case 9:
                                //Add level9 association, twice to represent spells gained on level up
                                if (i > 17)
                                    {
                                        break;
                                    }
                                availableSpells.Add(levelNine[1]);
                                Console.WriteLine(Spell_Level_Available);
                                l9+=1;
                                break;
                            case 10:
                                break;
                        }
            }
            //Cantrips. Need to be specified for Sorcerer Cantrip requirements
            if (PC.Level >= 10)
                {
                    for (int c = 4; c> -1; c--)
                    {
                        availableSpells.Add(Cantrips[c]);
                    }
                }
            else if (PC.Level >= 4)
                {
                    for (int c = 3; c> -1; c--)
                    {
                        availableSpells.Add(Cantrips[c]);
                    }
                }
                else 
                    {
                        for (int c = 2; c> -1; c--)
                        {
                            availableSpells.Add(Cantrips[c]);
                        }
                    }
                }
            return availableSpells;
        }

        public List<Spell> AvaialableSpellsWarlock(List<Spell> fullListAvail, NewCharacter PC)
        {
            List<Spell> Cantrips = fullListAvail.Where(s => s.SpellLevel == 0).ToList();
            RandomizeSpells(Cantrips, Cantrips.Count);
            List<Spell> levelOne = fullListAvail.Where(s => s.SpellLevel == 1).ToList();
            RandomizeSpells(levelOne, levelOne.Count);
            Console.WriteLine("levelOne");
            foreach(Spell spell in levelOne)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelTwo = fullListAvail.Where(s => s.SpellLevel == 2).ToList();
            RandomizeSpells(levelTwo, levelTwo.Count);
            Console.WriteLine("levelTwo");
            foreach(Spell spell in levelTwo)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelThree = fullListAvail.Where(s => s.SpellLevel == 3).ToList();
            RandomizeSpells(levelThree, levelThree.Count);
            Console.WriteLine("levelThree");
            foreach(Spell spell in fullListAvail)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelFour = fullListAvail.Where(s => s.SpellLevel == 4).ToList();
            RandomizeSpells(levelFour, levelFour.Count);
            Console.WriteLine("levelFour");
            foreach(Spell spell in levelFour)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelFive = fullListAvail.Where(s => s.SpellLevel == 5).ToList();
            RandomizeSpells(levelFive, levelFive.Count);
            Console.WriteLine("levelFive");
            foreach(Spell spell in levelFive)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelSix = fullListAvail.Where(s => s.SpellLevel == 6).ToList();
            RandomizeSpells(levelSix, levelSix.Count);
            Console.WriteLine("levelSix");
            foreach(Spell spell in levelSix)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelSeven = fullListAvail.Where(s => s.SpellLevel == 7).ToList();
            RandomizeSpells(levelSeven, levelSeven.Count);
            Console.WriteLine("levelSeven");
            foreach(Spell spell in levelSeven)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelEight = fullListAvail.Where(s => s.SpellLevel == 8).ToList();
            RandomizeSpells(levelEight, levelEight.Count);
            Console.WriteLine("levelEight");
            foreach(Spell spell in levelEight)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelNine = fullListAvail.Where(s => s.SpellLevel == 9).ToList();
            RandomizeSpells(levelNine, levelNine.Count);
            Console.WriteLine("levelNine");
            foreach(Spell spell in levelNine)
            {
                Console.WriteLine(spell.SpellName);
            }
            Console.WriteLine("Full list section end ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");

            List<Spell> availableSpells = new List<Spell>();


            //Based on Character's level, determine what spells known are chosen from the list available to the character
            //DetermineSpellLevel() can be modified to indicate access to spell tiers, using the Math.Ceiling functionality described there to determine spell level at the time that the spell could be chosen. Characters, through leveling progression, get access to different spells based upon their level and we can use that to determine what is added to their spells known.
            //Currently, we can use a For Loop(for(int i=1; i>ClassLevel; i++)) to determine spells known for a Wizard, grabbing from the appropriate list and adding it to what they know
            //Other classes will require some finangling, but the basic structure will be there once we understand how to implement it here.
            //Structure for spells: Available Spells (AS), Spells Known (SK) is a subset of AS, Prepared Spells is a subset of SK.
            //(A(S(PS)K)S) <- Loose diagram of the idea
            int l2 = 0;
            int l3 = 0;
            int l4 = 0;
            int l5 = 0;
            int l6 = 0;
            int l7 = 0;
            int l8 = 0;
            int l9 = 0;

            for(int i=1; i<=PC.Level; i++) // I was thinking of while loops lol
            {
                if(i == 1) // Can be modified for Paladins and Rangers to start @ 2
                    {
                        for(int j = 1; j>-1; j--)
                        {
                            //Add level1 association
                            availableSpells.Add(levelOne[j]);
                        }
                    }
                else
                {
                    int Spell_Level_Available =(int)Math.Ceiling(.5*i);
                    Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                    Console.WriteLine(Spell_Level_Available);
                    switch(Spell_Level_Available)
                        {
                            case 1:
                                //Add level1 association
                                availableSpells.Add(levelOne[2]);
                                break;
                            case 2:
                                //Add level2 association
                                availableSpells.Add(levelTwo[l2]);
                                Console.WriteLine(Spell_Level_Available);
                                Console.WriteLine(levelTwo[l2]);
                                l2+=1;
                                break;
                            case 3:
                                //Add level3 association
                                availableSpells.Add(levelThree[l3]);
                                Console.WriteLine(Spell_Level_Available);
                                l3+=1;
                                break;
                            case 4:
                                //Add level4 association
                                availableSpells.Add(levelFour[l4]);
                                Console.WriteLine(Spell_Level_Available);
                                l4+=1;
                                break;
                            case 5:
                                //Add level5 association
                                if (i == 10)
                                    {
                                        break;
                                    }
                                availableSpells.Add(levelFive[l5]);
                                Console.WriteLine(Spell_Level_Available);
                                l5+=1;
                                break;
                            case 6:
                                //Add level6 association
                                if (i == 11)
                                    {
                                        availableSpells.Add(levelFive[l5]);
                                        l5+=1;
                                        availableSpells.Add(levelSix[l6]);
                                        break;
                                    }
                                break;
                            case 7:
                                //Add level7 association
                                if (i == 13)
                                    {
                                        //Need to randomly implement level of spells chosen due to the unique nature of Warlock spellcasting, insuring that their limited selection of level 4 and 5 spells doesn't cause an Out of Bounds Error.
                                        //Can be refactored into it's own function
                                        Random rand = new Random();
                                        int randomspellselector = rand.Next(2,6);
                                        switch (randomspellselector)
                                            {
                                                case 2:
                                                    availableSpells.Add(levelTwo[l2]);
                                                    l2+=1;
                                                    break;
                                                case 3:
                                                    availableSpells.Add(levelThree[l3]);
                                                    l3+=1;
                                                    break;
                                                case 4:
                                                    if(l4 > 3) //level 4 Warlock Spells total = 4
                                                        {
                                                            goto case 3;
                                                        }
                                                    availableSpells.Add(levelFour[l4]);
                                                    l4+=1;
                                                    break;
                                                case 5:
                                                    if(l5 > 3) //level 5 Warlock Spells total = 4
                                                        {
                                                            goto case 2;
                                                        }
                                                    availableSpells.Add(levelFive[l5]);
                                                    l5+=1;
                                                    break;
                                            }
                                        availableSpells.Add(levelSeven[l7]);
                                        break;
                                    }
                                break;
                            case 8:
                                //Add level8 association
                                if (i == 15)
                                    {
                                        //Can be refactored into it's own function
                                        Random rand = new Random();
                                        int randomspellselector = rand.Next(2,6);
                                        switch (randomspellselector)
                                            {
                                                case 2:
                                                    availableSpells.Add(levelTwo[l2]);
                                                    l2+=1;
                                                    break;
                                                case 3:
                                                    availableSpells.Add(levelThree[l3]);
                                                    l3+=1;
                                                    break;
                                                case 4:
                                                    if(l4 > 3)
                                                        {
                                                            goto case 3;
                                                        }
                                                    availableSpells.Add(levelFour[l4]);
                                                    l4+=1;
                                                    break;
                                                case 5:
                                                    if(l5 > 3)
                                                        {
                                                            goto case 2;
                                                        }
                                                    availableSpells.Add(levelFive[l5]);
                                                    l5+=1;
                                                    break;
                                            }
                                        availableSpells.Add(levelEight[l8]);
                                        break;
                                    }
                                break;
                            case 9:
                                //Add level9 association
                                if (i == 17)
                                    {
                                        //Can be refactored into it's own function
                                        Random rand = new Random();
                                        int randomspellselector = rand.Next(2,6);
                                        switch (randomspellselector)
                                            {
                                                case 2:
                                                    availableSpells.Add(levelTwo[l2]);
                                                    l2+=1;
                                                    break;
                                                case 3:
                                                    availableSpells.Add(levelThree[l3]);
                                                    l3+=1;
                                                    break;
                                                case 4:
                                                    if(l4 > 3)
                                                        {
                                                            goto case 3;
                                                        }
                                                    availableSpells.Add(levelFour[l4]);
                                                    l4+=1;
                                                    break;
                                                case 5:
                                                    if(l5 > 3)
                                                        {
                                                            goto case 2;
                                                        }
                                                    availableSpells.Add(levelFive[l5]);
                                                    l5+=1;
                                                    break;
                                            }
                                        availableSpells.Add(levelNine[l9]);
                                        break;
                                    }
                                if (i == 19)
                                {
                                    //Can be refactored into it's own function
                                        Random rand = new Random();
                                        int randomspellselector = rand.Next(2,6);
                                        switch (randomspellselector)
                                            {
                                                case 2:
                                                    availableSpells.Add(levelTwo[l2]);
                                                    l2+=1;
                                                    break;
                                                case 3:
                                                    availableSpells.Add(levelThree[l3]);
                                                    l3+=1;
                                                    break;
                                                case 4:
                                                    if(l4 > 3)
                                                        {
                                                            goto case 3;
                                                        }
                                                    availableSpells.Add(levelFour[l4]);
                                                    l4+=1;
                                                    break;
                                                case 5:
                                                    if(l5 > 3)
                                                        {
                                                            goto case 2;
                                                        }
                                                    availableSpells.Add(levelFive[l5]);
                                                    l5+=1;
                                                    break;
                                            }
                                }
                                break;
                            case 10:
                                break;
                        }
            }
            //Cantrips. Made to insure that Eldritch Blast is included no matter what on the Warlock Spell list. Make sure to test this on Warlocks, as I am currently unsure if the wording is fully functional in regards to the Find calls below.
            availableSpells.Add(Cantrips.Find(s => s.SpellName == "Eldritch Blast"));
            Cantrips.Remove(Cantrips.Find(s => s.SpellName == "Eldritch Blast"));// Removes EB from the cantrips list to insure that it isn't selected twice
            if (PC.Level >= 10)
                {
                    for (int c = 2; c> -1; c--)
                    {
                        availableSpells.Add(Cantrips[c]);
                    }
                }
            else if (PC.Level >= 4)
                {
                    for (int c = 1; c> -1; c--)
                    {
                        availableSpells.Add(Cantrips[c]);
                    }
                }
                else 
                    {
                        for (int c = 0; c> -1; c--)
                        {
                            availableSpells.Add(Cantrips[c]);
                        }
                    }
                }
            return availableSpells;
        }

        public List<Spell> AvaialableSpellsRanger(List<Spell> fullListAvail, NewCharacter PC)
        {
            List<Spell> Cantrips = fullListAvail.Where(s => s.SpellLevel == 0).ToList();
            RandomizeSpells(Cantrips, Cantrips.Count);
            List<Spell> levelOne = fullListAvail.Where(s => s.SpellLevel == 1).ToList();
            RandomizeSpells(levelOne, levelOne.Count);
            Console.WriteLine("levelOne");
            foreach(Spell spell in levelOne)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelTwo = fullListAvail.Where(s => s.SpellLevel == 2).ToList();
            RandomizeSpells(levelTwo, levelTwo.Count);
            Console.WriteLine("levelTwo");
            foreach(Spell spell in levelTwo)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelThree = fullListAvail.Where(s => s.SpellLevel == 3).ToList();
            RandomizeSpells(levelThree, levelThree.Count);
            Console.WriteLine("levelThree");
            foreach(Spell spell in fullListAvail)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelFour = fullListAvail.Where(s => s.SpellLevel == 4).ToList();
            RandomizeSpells(levelFour, levelFour.Count);
            Console.WriteLine("levelFour");
            foreach(Spell spell in levelFour)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelFive = fullListAvail.Where(s => s.SpellLevel == 5).ToList();
            RandomizeSpells(levelFive, levelFive.Count);
            Console.WriteLine("levelFive");
            foreach(Spell spell in levelFive)
            {
                Console.WriteLine(spell.SpellName);
            }
            Console.WriteLine("Full list section end ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");

            List<Spell> availableSpells = new List<Spell>();


            //Based on Character's level, determine what spells known are chosen from the list available to the character
            //DetermineSpellLevel() can be modified to indicate access to spell tiers, using the Math.Ceiling functionality described there to determine spell level at the time that the spell could be chosen. Characters, through leveling progression, get access to different spells based upon their level and we can use that to determine what is added to their spells known.
            //Currently, we can use a For Loop(for(int i=1; i>ClassLevel; i++)) to determine spells known for a Wizard, grabbing from the appropriate list and adding it to what they know
            //Other classes will require some finangling, but the basic structure will be there once we understand how to implement it here.
            //Structure for spells: Available Spells (AS), Spells Known (SK) is a subset of AS, Prepared Spells is a subset of SK.
            //(A(S(PS)K)S) <- Loose diagram of the idea
            int l2 = 0;
            int l3 = 0;
            int l4 = 0;
            int l5 = 0;

            for(int i=2; i<=PC.Level; i++) // I was thinking of while loops lol
            {
                if(i == 2) // Can be modified for Paladins and Rangers to start @ 2
                    {
                        for(int j = 1; j>-1; j--)
                        {
                            //Add level1 association
                            availableSpells.Add(levelOne[j]);
                        }
                    }
                else
                {
                    int Spell_Level_Available =(int)Math.Ceiling(.25*i);
                    Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                    Console.WriteLine(Spell_Level_Available);
                    switch(Spell_Level_Available)
                        {
                            case 1:
                                //Add level1 association
                                if (i == 4)
                                {
                                    break;
                                }
                                availableSpells.Add(levelOne[2]);
                                break;
                            case 2:
                                //Add level2 association
                                if (i == 10)
                                {
                                    break;
                                }
                                availableSpells.Add(levelTwo[l2]);
                                Console.WriteLine(Spell_Level_Available);
                                Console.WriteLine(levelTwo[l2]);
                                l2+=1;
                                break;
                            case 3:
                                //Add level3 association
                                if (i == 12)
                                {
                                    break;
                                }
                                availableSpells.Add(levelThree[l3]);
                                Console.WriteLine(Spell_Level_Available);
                                l3+=1;
                                break;
                            case 4:
                                //Add level4 association
                                if (i == 14 || i == 16)
                                {
                                    break;
                                }
                                if (l4 > 14)
                                {
                                    goto case 3;
                                }
                                availableSpells.Add(levelFour[l4]);
                                Console.WriteLine(Spell_Level_Available);
                                l4+=1;
                                break;
                            case 5:
                                //Add level5 association
                                if (i == 18 || i == 20)
                                {
                                    break;
                                }
                                if (l5 > 2)
                                {
                                    goto case 4;
                                }
                                availableSpells.Add(levelFive[l5]);
                                Console.WriteLine(Spell_Level_Available);
                                l5+=1;
                                break;
                        }
                }
        }
        return availableSpells;
    }
    public List<Spell> AvaialableSpellsCleric(List<Spell> fullListAvail, NewCharacter PC)
        {
            List<Spell> Cantrips = fullListAvail.Where(s => s.SpellLevel == 0).ToList();
            RandomizeSpells(Cantrips, Cantrips.Count);
            List<Spell> levelOne = fullListAvail.Where(s => s.SpellLevel == 1).ToList();
            RandomizeSpells(levelOne, levelOne.Count);
            Console.WriteLine("levelOne");
            foreach(Spell spell in levelOne)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelTwo = fullListAvail.Where(s => s.SpellLevel == 2).ToList();
            RandomizeSpells(levelTwo, levelTwo.Count);
            Console.WriteLine("levelTwo");
            foreach(Spell spell in levelTwo)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelThree = fullListAvail.Where(s => s.SpellLevel == 3).ToList();
            RandomizeSpells(levelThree, levelThree.Count);
            Console.WriteLine("levelThree");
            foreach(Spell spell in fullListAvail)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelFour = fullListAvail.Where(s => s.SpellLevel == 4).ToList();
            RandomizeSpells(levelFour, levelFour.Count);
            Console.WriteLine("levelFour");
            foreach(Spell spell in levelFour)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelFive = fullListAvail.Where(s => s.SpellLevel == 5).ToList();
            RandomizeSpells(levelFive, levelFive.Count);
            Console.WriteLine("levelFive");
            foreach(Spell spell in levelFive)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelSix = fullListAvail.Where(s => s.SpellLevel == 6).ToList();
            RandomizeSpells(levelSix, levelSix.Count);
            Console.WriteLine("levelSix");
            foreach(Spell spell in levelSix)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelSeven = fullListAvail.Where(s => s.SpellLevel == 7).ToList();
            RandomizeSpells(levelSeven, levelSeven.Count);
            Console.WriteLine("levelSeven");
            foreach(Spell spell in levelSeven)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelEight = fullListAvail.Where(s => s.SpellLevel == 8).ToList();
            RandomizeSpells(levelEight, levelEight.Count);
            Console.WriteLine("levelEight");
            foreach(Spell spell in levelEight)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelNine = fullListAvail.Where(s => s.SpellLevel == 9).ToList();
            RandomizeSpells(levelNine, levelNine.Count);
            Console.WriteLine("levelNine");
            foreach(Spell spell in levelNine)
            {
                Console.WriteLine(spell.SpellName);
            }
            Console.WriteLine("Full list section end ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");

            List<Spell> availableSpells = new List<Spell>();


            //Based on Character's level, determine what spells known are chosen from the list available to the character
            //DetermineSpellLevel() can be modified to indicate access to spell tiers, using the Math.Ceiling functionality described there to determine spell level at the time that the spell could be chosen. Characters, through leveling progression, get access to different spells based upon their level and we can use that to determine what is added to their spells known.
            //Currently, we can use a For Loop(for(int i=1; i>ClassLevel; i++)) to determine spells known for a Wizard, grabbing from the appropriate list and adding it to what they know
            //Other classes will require some finangling, but the basic structure will be there once we understand how to implement it here.
            //Structure for spells: Available Spells (AS), Spells Known (SK) is a subset of AS, Prepared Spells is a subset of SK.
            //(A(S(PS)K)S) <- Loose diagram of the idea

            for(int i=1; i<=PC.Level; i++) // I was thinking of while loops lol
            {
                //i is a representation of level, since Clerics/Druids/Paladins get access to all of the associated spells at a particular level, all we need to do is add each in particular when they hit the determined levels
                switch(i)
                {
                    case 1:
                    foreach (Spell s in levelOne)
                    {
                        availableSpells.Add(s);
                    }
                    break;
                    case 3:
                    foreach (Spell s in levelTwo)
                    {
                        availableSpells.Add(s);
                    }
                    break;
                    case 5:
                    foreach (Spell s in levelThree)
                    {
                        availableSpells.Add(s);
                    }
                    break;
                    case 7:
                    foreach (Spell s in levelFour)
                    {
                        availableSpells.Add(s);
                    }
                    break;
                    case 9:
                    foreach (Spell s in levelFive)
                    {
                        availableSpells.Add(s);
                    }
                    break;
                    case 11:
                    foreach (Spell s in levelSix)
                    {
                        availableSpells.Add(s);
                    }
                    break;
                    case 13:
                    foreach (Spell s in levelSeven)
                    {
                        availableSpells.Add(s);
                    }
                    break;
                    case 15:
                    foreach (Spell s in levelEight)
                    {
                        availableSpells.Add(s);
                    }
                    break;
                    case 17:
                    foreach (Spell s in levelNine)
                    {
                        availableSpells.Add(s);
                    }
                    break;
                }
            }
            //Cantrips. Need to be specified for Sorcerer Cantrip requirements
            if (PC.Level >= 10)
                {
                    for (int c = 4; c> -1; c--)
                    {
                        availableSpells.Add(Cantrips[c]);
                    }
                }
            else if (PC.Level >= 4)
                {
                    for (int c = 3; c> -1; c--)
                    {
                        availableSpells.Add(Cantrips[c]);
                    }
                }
                else 
                    {
                        for (int c = 2; c> -1; c--)
                        {
                            availableSpells.Add(Cantrips[c]);
                        }
                    }
            return availableSpells;
        }

        public List<Spell> AvaialableSpellsDruid(List<Spell> fullListAvail, NewCharacter PC)
        {
            List<Spell> Cantrips = fullListAvail.Where(s => s.SpellLevel == 0).ToList();
            RandomizeSpells(Cantrips, Cantrips.Count);
            List<Spell> levelOne = fullListAvail.Where(s => s.SpellLevel == 1).ToList();
            RandomizeSpells(levelOne, levelOne.Count);
            Console.WriteLine("levelOne");
            foreach(Spell spell in levelOne)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelTwo = fullListAvail.Where(s => s.SpellLevel == 2).ToList();
            RandomizeSpells(levelTwo, levelTwo.Count);
            Console.WriteLine("levelTwo");
            foreach(Spell spell in levelTwo)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelThree = fullListAvail.Where(s => s.SpellLevel == 3).ToList();
            RandomizeSpells(levelThree, levelThree.Count);
            Console.WriteLine("levelThree");
            foreach(Spell spell in fullListAvail)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelFour = fullListAvail.Where(s => s.SpellLevel == 4).ToList();
            RandomizeSpells(levelFour, levelFour.Count);
            Console.WriteLine("levelFour");
            foreach(Spell spell in levelFour)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelFive = fullListAvail.Where(s => s.SpellLevel == 5).ToList();
            RandomizeSpells(levelFive, levelFive.Count);
            Console.WriteLine("levelFive");
            foreach(Spell spell in levelFive)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelSix = fullListAvail.Where(s => s.SpellLevel == 6).ToList();
            RandomizeSpells(levelSix, levelSix.Count);
            Console.WriteLine("levelSix");
            foreach(Spell spell in levelSix)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelSeven = fullListAvail.Where(s => s.SpellLevel == 7).ToList();
            RandomizeSpells(levelSeven, levelSeven.Count);
            Console.WriteLine("levelSeven");
            foreach(Spell spell in levelSeven)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelEight = fullListAvail.Where(s => s.SpellLevel == 8).ToList();
            RandomizeSpells(levelEight, levelEight.Count);
            Console.WriteLine("levelEight");
            foreach(Spell spell in levelEight)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelNine = fullListAvail.Where(s => s.SpellLevel == 9).ToList();
            RandomizeSpells(levelNine, levelNine.Count);
            Console.WriteLine("levelNine");
            foreach(Spell spell in levelNine)
            {
                Console.WriteLine(spell.SpellName);
            }
            Console.WriteLine("Full list section end ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");

            List<Spell> availableSpells = new List<Spell>();


            //Based on Character's level, determine what spells known are chosen from the list available to the character
            //DetermineSpellLevel() can be modified to indicate access to spell tiers, using the Math.Ceiling functionality described there to determine spell level at the time that the spell could be chosen. Characters, through leveling progression, get access to different spells based upon their level and we can use that to determine what is added to their spells known.
            //Currently, we can use a For Loop(for(int i=1; i>ClassLevel; i++)) to determine spells known for a Wizard, grabbing from the appropriate list and adding it to what they know
            //Other classes will require some finangling, but the basic structure will be there once we understand how to implement it here.
            //Structure for spells: Available Spells (AS), Spells Known (SK) is a subset of AS, Prepared Spells is a subset of SK.
            //(A(S(PS)K)S) <- Loose diagram of the idea

            for(int i=1; i<=PC.Level; i++) // I was thinking of while loops lol
            {
                //i is a representation of level, since Clerics/Druids/Paladins get access to all of the associated spells at a particular level, all we need to do is add each in particular when they hit the determined levels
                switch(i)
                {
                    case 1:
                    foreach (Spell s in levelOne)
                    {
                        availableSpells.Add(s);
                    }
                    break;
                    case 3:
                    foreach (Spell s in levelTwo)
                    {
                        availableSpells.Add(s);
                    }
                    break;
                    case 5:
                    foreach (Spell s in levelThree)
                    {
                        availableSpells.Add(s);
                    }
                    break;
                    case 7:
                    foreach (Spell s in levelFour)
                    {
                        availableSpells.Add(s);
                    }
                    break;
                    case 9:
                    foreach (Spell s in levelFive)
                    {
                        availableSpells.Add(s);
                    }
                    break;
                    case 11:
                    foreach (Spell s in levelSix)
                    {
                        availableSpells.Add(s);
                    }
                    break;
                    case 13:
                    foreach (Spell s in levelSeven)
                    {
                        availableSpells.Add(s);
                    }
                    break;
                    case 15:
                    foreach (Spell s in levelEight)
                    {
                        availableSpells.Add(s);
                    }
                    break;
                    case 17:
                    foreach (Spell s in levelNine)
                    {
                        availableSpells.Add(s);
                    }
                    break;
                }
            }
            //Cantrips. Need to be specified for Sorcerer Cantrip requirements
            if (PC.Level >= 10)
                {
                    for (int c = 3; c> -1; c--)
                    {
                        availableSpells.Add(Cantrips[c]);
                    }
                }
            else if (PC.Level >= 4)
                {
                    for (int c = 2; c> -1; c--)
                    {
                        availableSpells.Add(Cantrips[c]);
                    }
                }
                else 
                    {
                        for (int c = 1; c> -1; c--)
                        {
                            availableSpells.Add(Cantrips[c]);
                        }
                    }
            if (PC.playerClass.SubClassName == "Circle of the Land: Arctic" ||
                PC.playerClass.SubClassName == "Circle of the Land: Coast" ||
                PC.playerClass.SubClassName == "Circle of the Land: Desert" ||
                PC.playerClass.SubClassName == "Circle of the Land: Forest" ||
                PC.playerClass.SubClassName == "Circle of the Land: Grassland" ||
                PC.playerClass.SubClassName == "Circle of the Land: Mountain" ||
                PC.playerClass.SubClassName == "Circle of the Land: Swamp" ||
                PC.playerClass.SubClassName == "Circle of the Land: Underdark"
                )
                {
                    availableSpells.Add(Cantrips[Cantrips.Count-1]);
                }
            return availableSpells;
        }

        public List<Spell> AvaialableSpellsPaladin(List<Spell> fullListAvail, NewCharacter PC)
        {
            List<Spell> Cantrips = fullListAvail.Where(s => s.SpellLevel == 0).ToList();
            RandomizeSpells(Cantrips, Cantrips.Count);
            List<Spell> levelOne = fullListAvail.Where(s => s.SpellLevel == 1).ToList();
            RandomizeSpells(levelOne, levelOne.Count);
            Console.WriteLine("levelOne");
            foreach(Spell spell in levelOne)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelTwo = fullListAvail.Where(s => s.SpellLevel == 2).ToList();
            RandomizeSpells(levelTwo, levelTwo.Count);
            Console.WriteLine("levelTwo");
            foreach(Spell spell in levelTwo)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelThree = fullListAvail.Where(s => s.SpellLevel == 3).ToList();
            RandomizeSpells(levelThree, levelThree.Count);
            Console.WriteLine("levelThree");
            foreach(Spell spell in fullListAvail)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelFour = fullListAvail.Where(s => s.SpellLevel == 4).ToList();
            RandomizeSpells(levelFour, levelFour.Count);
            Console.WriteLine("levelFour");
            foreach(Spell spell in levelFour)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelFive = fullListAvail.Where(s => s.SpellLevel == 5).ToList();
            RandomizeSpells(levelFive, levelFive.Count);
            Console.WriteLine("levelFive");
            foreach(Spell spell in levelFive)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelSix = fullListAvail.Where(s => s.SpellLevel == 6).ToList();
            RandomizeSpells(levelSix, levelSix.Count);
            Console.WriteLine("levelSix");
            foreach(Spell spell in levelSix)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelSeven = fullListAvail.Where(s => s.SpellLevel == 7).ToList();
            RandomizeSpells(levelSeven, levelSeven.Count);
            Console.WriteLine("levelSeven");
            foreach(Spell spell in levelSeven)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelEight = fullListAvail.Where(s => s.SpellLevel == 8).ToList();
            RandomizeSpells(levelEight, levelEight.Count);
            Console.WriteLine("levelEight");
            foreach(Spell spell in levelEight)
            {
                Console.WriteLine(spell.SpellName);
            }
            List<Spell> levelNine = fullListAvail.Where(s => s.SpellLevel == 9).ToList();
            RandomizeSpells(levelNine, levelNine.Count);
            Console.WriteLine("levelNine");
            foreach(Spell spell in levelNine)
            {
                Console.WriteLine(spell.SpellName);
            }
            Console.WriteLine("Full list section end ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");

            List<Spell> availableSpells = new List<Spell>();


            //Based on Character's level, determine what spells known are chosen from the list available to the character
            //DetermineSpellLevel() can be modified to indicate access to spell tiers, using the Math.Ceiling functionality described there to determine spell level at the time that the spell could be chosen. Characters, through leveling progression, get access to different spells based upon their level and we can use that to determine what is added to their spells known.
            //Currently, we can use a For Loop(for(int i=1; i>ClassLevel; i++)) to determine spells known for a Wizard, grabbing from the appropriate list and adding it to what they know
            //Other classes will require some finangling, but the basic structure will be there once we understand how to implement it here.
            //Structure for spells: Available Spells (AS), Spells Known (SK) is a subset of AS, Prepared Spells is a subset of SK.
            //(A(S(PS)K)S) <- Loose diagram of the idea

            for(int i=1; i<=PC.Level; i++) // I was thinking of while loops lol
            {
                //i is a representation of level, since Clerics/Druids/Paladins get access to all of the associated spells at a particular level, all we need to do is add each in particular when they hit the determined levels
                switch(i)
                {
                    case 2:
                    foreach (Spell s in levelOne)
                    {
                        availableSpells.Add(s);
                    }
                    break;
                    case 5:
                    foreach (Spell s in levelTwo)
                    {
                        availableSpells.Add(s);
                    }
                    break;
                    case 9:
                    foreach (Spell s in levelThree)
                    {
                        availableSpells.Add(s);
                    }
                    break;
                    case 13:
                    foreach (Spell s in levelFour)
                    {
                        availableSpells.Add(s);
                    }
                    break;
                    case 17:
                    foreach (Spell s in levelFive)
                    {
                        availableSpells.Add(s);
                    }
                    break;
                }
            }
            return availableSpells;
        }
    }
}
