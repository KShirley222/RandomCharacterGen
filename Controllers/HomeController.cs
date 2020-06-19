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
        
        Random rand = new Random();
        [HttpGet("")]
        public IActionResult Index()
        {
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
                test.isSaved = true;
                test.playerName = "First Character Generated";
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
                SpellAssoc AlwaysPreppedChecker = new SpellAssoc();
                foreach(Spell s in Spells)
                {
                    SpellAssoc a = new SpellAssoc(test, s);
                    //insert alwayspreppedfunction here
                    AlwaysPreppedChecker.AlwaysPreppedChecker(test,s,a);
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
            // random selection might need tweaking
            int characterNum = 1;
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
            AutoDelete();
            int? SessionId = HttpContext.Session.GetInt32("UserId");
            User SessionUser = _context.Users.FirstOrDefault( u => u.UserId == SessionId);
            if(SessionUser == null){
                return View("UserLogin");
            }

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

            //***Investigated PlayerClass and PlayerRace, for some reason the Hill Dwarf Subrace Trait, Dwarven Toughness was not being applied correctly, this is overt but should amend the issue.
            if(playerRace.Subrace == "Hill")
                {
                    playerStat.HitPoints += Level;
                }

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
            SpellAssoc AlwaysPreppedChecker = new SpellAssoc();
            foreach(Spell s in availableSpells)
            {
                SpellAssoc a = new SpellAssoc(newPlayer, s);//always true
                AlwaysPreppedChecker.AlwaysPreppedChecker(newPlayer, s, a);
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
                .ThenInclude(sa => sa.SpellA)
                .FirstOrDefault(c => c.CharacterId == character.CharacterId)
                .SpellList.Select(s => s.SpellA)
                .OrderBy(f => f.SpellLevel)
                .ToList();

            int passiveperception = 10+character.playerStat.Perception;

            dynamic MyModel = new ExpandoObject();
            MyModel.User = SessionUser;
            MyModel.Login = new Login();
            MyModel.Character = character;
            MyModel.Spells = Spells;
            MyModel.Features = Feats;
            MyModel.PassivePerception = passiveperception;

            return View("Index", MyModel);
        }

        [HttpGet("/update/{characterId}/{name}/{notes}")]
        public IActionResult UpdateNote(int characterId, string name, string notes)
        {
            // User Check to make sure login is completed, if not theredirect to login
            int? SessionId = HttpContext.Session.GetInt32("UserId");
            User SessionUser = _context.Users.FirstOrDefault( u => u.UserId == SessionId);
            if(SessionUser == null){
                return View("UserLogin");
            }
            NewCharacter CharacterUpdate = _context.NewCharacter.FirstOrDefault( c => c.CharacterId == characterId);
            CharacterUpdate.playerName = name;
            CharacterUpdate.playerNotes = notes;
            //isSaved Bool now in use v
            CharacterUpdate.isSaved = true;
            //isSaved Bool now in use ^
            _context.SaveChanges();
            return RedirectToAction("ViewCharacter",new {ID = characterId});
        }

        [HttpGet("/specific")]
        public IActionResult SpecificGeneration(string selectedrace, string selectedclass, string selectedlevel)
        {
            AutoDelete();
            int? SessionId = HttpContext.Session.GetInt32("UserId");
            User SessionUser = _context.Users.FirstOrDefault( u => u.UserId == SessionId);
            if(SessionUser == null){
                return View("UserLogin");
            }
            int Level = Int32.Parse(selectedlevel);
            
            if (Level == 0) //Assign the base choice value as == 0
                {
    
                    Level = rand.Next(1,21);
                }
            //creating input for creation and modification of base in other functions
            PlayerStat playerStat = new PlayerStat(Level);

            // Run player stat through background
            PlayerBG playerBG = new PlayerBG();
            // Returns player Stat VVV
            playerBG.BGSelector(playerStat, playerBG);
            
            // Determines player Race and then run stats
            PlayerRace playerRace = new PlayerRace();
            // Returns player Stat VVV

            // Determine Player Class and run playerStat
            if (selectedrace != "Random")
                {
                    playerRace.SpecificRaceSelector(Level, playerStat, playerRace, selectedrace);
                }
            else
                {
                playerRace.RaceSelector(Level, playerStat, playerRace);
                }
            PlayerClass playerClass = new PlayerClass(Level, playerStat);
            // Returns player Stat VVV
            if (selectedclass != "Random")
                {
                    playerClass.SpecClassSelector(Level, playerStat, playerClass, selectedclass);
                }
            else
                {
                    playerClass.ClassSelector(Level, playerStat, playerClass);
                }
            // reruns numbers based on updated proficiencies and skill increases
            playerStat.UpdatePro(playerStat);

            //* Copied from /GenerateNew with similar reasoning to the comment. Dwarven Toughness isn't being applied correctly.
            if(playerRace.Subrace == "Hill")
                {
                    playerStat.HitPoints += Level;
                }

            // create connection to all character objects within the character
            NewCharacter newPlayer = new NewCharacter(Level, playerStat,playerRace,playerClass, playerBG, SessionUser);

            _context.PlayerStats.Add(playerStat); 
            _context.PlayerBGs.Add(playerBG); 
            _context.PlayerClasses.Add(playerClass);
            _context.PlayerRaces.Add(playerRace);
            _context.NewCharacter.Add(newPlayer);

            // Feat Generation
            List<Feature> charFeatures = _context.Features.Where(f =>  ((f.FeatSource == playerClass.ClassName) && (f.FeatLevel <= Level)) || ((f.FeatSource == playerClass.SubClassName) && (f.FeatLevel <= Level))).ToList();
            foreach( Feature feat in charFeatures )
            {
                var Fassoc = new FeatureAssoc(newPlayer, feat); 
                _context.Feature_Associations.Add(Fassoc);
                _context.SaveChanges();
            }

            List<Feature> Feats = _context.NewCharacter.Include(c => c.FeaturesList) 
                        .ThenInclude(fa => fa.FeatureA) 
                        .FirstOrDefault(c => c.CharacterId == newPlayer.CharacterId) 
                        .FeaturesList.Select(f => f.FeatureA)
                        .OrderBy(f => f.FeatLevel)
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
            SpellAssoc AlwaysPreppedChecker = new SpellAssoc();
            foreach(Spell s in availableSpells)
            {
                SpellAssoc a = new SpellAssoc(newPlayer, s);//always true
                AlwaysPreppedChecker.AlwaysPreppedChecker(newPlayer, s, a);
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

        public void AutoDelete()
        {
            NewCharacter CharToDelete = _context.NewCharacter.FirstOrDefault(ctd => ctd.isSaved == false && ctd.CreatedAt < DateTime.Now.AddHours(-24));
            if (CharToDelete != null)
                {
                //Need to unravel the new character object and delete the character objects connected to it
                //Spell Associations - check
                //Feature Associations - check
                //playerClass - check
                //playerRace - check
                //playerStat - check
                //playerBG - check
                List<SpellAssoc> SpellsAssocToDelete = _context.Spell_Associations.Where(sa => sa.CharacterId == CharToDelete.CharacterId).ToList();
                    foreach (SpellAssoc sa in SpellsAssocToDelete)
                    {
                        _context.Spell_Associations.Remove(sa);
                    }
                List<FeatureAssoc> FeatAssocToDelete = _context.Feature_Associations.Where(fa => fa.CharacterId == CharToDelete.CharacterId).ToList();
                    foreach (FeatureAssoc fa in FeatAssocToDelete)
                    {
                        _context.Feature_Associations.Remove(fa);
                    }
                PlayerClass PClassToDelete = _context.PlayerClasses.FirstOrDefault(pc => pc.PlayerClassId == CharToDelete.playerClassId);
                    _context.PlayerClasses.Remove(PClassToDelete);
                PlayerRace PRaceToDelete = _context.PlayerRaces.FirstOrDefault(pr => pr.PlayerRaceId == CharToDelete.playerRaceId);
                    _context.PlayerRaces.Remove(PRaceToDelete);
                PlayerStat PStatToDelete = _context.PlayerStats.FirstOrDefault(ps => ps.PlayerStatId == CharToDelete.playerStatId);
                    _context.PlayerStats.Remove(PStatToDelete);
                PlayerBG PGBToDelete = _context.PlayerBGs.FirstOrDefault(pbg => pbg.PlayerBGId == CharToDelete.playerBGId);
                    _context.PlayerBGs.Remove(PGBToDelete);

                _context.NewCharacter.Remove(CharToDelete);
                }
        }

        [HttpGet("/delete/{characterid}")]
        public IActionResult deleteChar(int characterid)
        {
            NewCharacter CharToDelete = _context.NewCharacter.FirstOrDefault(ctd => ctd.CharacterId == characterid);
            if (CharToDelete != null)
                {
                List<SpellAssoc> SpellsAssocToDelete = _context.Spell_Associations.Where(sa => sa.CharacterId == CharToDelete.CharacterId).ToList();
                    foreach (SpellAssoc sa in SpellsAssocToDelete)
                    {
                        _context.Spell_Associations.Remove(sa);
                    }
                List<FeatureAssoc> FeatAssocToDelete = _context.Feature_Associations.Where(fa => fa.CharacterId == CharToDelete.CharacterId).ToList();
                    foreach (FeatureAssoc fa in FeatAssocToDelete)
                    {
                        _context.Feature_Associations.Remove(fa);
                    }
                PlayerClass PClassToDelete = _context.PlayerClasses.FirstOrDefault(pc => pc.PlayerClassId == CharToDelete.playerClassId);
                    _context.PlayerClasses.Remove(PClassToDelete);
                PlayerRace PRaceToDelete = _context.PlayerRaces.FirstOrDefault(pr => pr.PlayerRaceId == CharToDelete.playerRaceId);
                    _context.PlayerRaces.Remove(PRaceToDelete);
                PlayerStat PStatToDelete = _context.PlayerStats.FirstOrDefault(ps => ps.PlayerStatId == CharToDelete.playerStatId);
                    _context.PlayerStats.Remove(PStatToDelete);
                PlayerBG PGBToDelete = _context.PlayerBGs.FirstOrDefault(pbg => pbg.PlayerBGId == CharToDelete.playerBGId);
                    _context.PlayerBGs.Remove(PGBToDelete);

                _context.NewCharacter.Remove(CharToDelete);
                }
                _context.SaveChanges();
            int? SessionId = HttpContext.Session.GetInt32("UserId");
            return RedirectToAction("Profile", new {ID = SessionId});
        }
        
        // ===================
        // ===================================================
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
