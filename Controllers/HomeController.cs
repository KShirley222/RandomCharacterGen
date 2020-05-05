using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CharacterGenerator.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

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
            int level = rand.Next(1,21);
            Character newChar = new Character(level);
            newChar.SecondaryGeneration(newChar); 
            newChar.ThirdGeneration(newChar);
            newChar.FourthGeneration(newChar);
            newChar.FifthGeneration(newChar);
            _context.Characters.Add(newChar);
            _context.SaveChanges();
            return View(newChar);
        }

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

        // Seperate deneration and saving
        // saving requires userid in session, if not prompt for login register should kick in
        // would be cool if pop up happened so that character doesnt get removed
        [HttpGet("/class")]
        public IActionResult CharacterGenerator()
        {
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
            NewCharacter newPlayer = new NewCharacter(Level, playerStat,playerRace,playerClass, playerBG);
            _context.PlayerStats.Add(playerStat);
            _context.PlayerBGs.Add(playerBG);
            _context.PlayerClasses.Add(playerClass);
            _context.PlayerRaces.Add(playerRace);
            _context.NewCharacter.Add(newPlayer);
            _context.SaveChanges();

            //LINQ query

            return View(newPlayer);
        }

        [HttpPost("/class/save")]
        public IActionResult SaveCharacter(NewCharacter newPlayer, PlayerStat playerStat, PlayerBG playerBG, PlayerRace playerRace, PlayerClass playerClass)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if(userId == null)
                {
                    return View("/login");
                }
            else
                {
                    _context.PlayerStats.Add(playerStat);
                    _context.PlayerBGs.Add(playerBG);
                    _context.PlayerClasses.Add(playerClass);
                    _context.PlayerRaces.Add(playerRace);
                    _context.NewCharacter.Add(newPlayer);
                    _context.SaveChanges();
                    return View("class", newPlayer);
                }

        }


        [HttpPost("/register")]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(_context.Users.Any( u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email is already in use.");
                    return View("class");
                }
                if(_context.Users.Any( u => u.UserName == newUser.UserName))
                {
                    ModelState.AddModelError("UserName", "User Name is already in use.");
                    return View("class");
                }
                
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword( newUser, newUser.Password);

                _context.Users.Add(newUser);
                _context.SaveChanges();

                HttpContext.Session.SetInt32("UserId", newUser.UserId);
                int id = newUser.UserId;
                return View("class");
            }
            return View("class");
        }

        [HttpGet("/login")]
        public IActionResult Login(Login LoginUser)
        {
            if(ModelState.IsValid)
            {
                var userInDb = _context.Users.FirstOrDefault(u => u.Email == LoginUser.LoginEmail);
                if(userInDb == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("class");
                }
                else{
                    var hasher = new PasswordHasher<Login>();
                    var result = hasher.VerifyHashedPassword(LoginUser, userInDb.Password, LoginUser.LoginPassword);
                    if(result ==0)
                    {
                        ModelState.AddModelError("Email", "Invalid Email/Password");
                        return View("class");
                    }
                    else{
                        HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                        return View("class");
                    }
                }
            }
            return View("class");
        }
    }
}
