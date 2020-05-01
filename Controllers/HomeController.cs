using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CharacterGenerator.Models;

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


        // test sample

        // // character class/ Playerclass id/obj BG Id/obj
        // public IActionResult Test()
        // {

        //     Character newchar =new Character(level);
        //     Stats newstats =  
        //     PlayerClass newclass = new PlayerClass(newchar);

        //     PlayerClass abstract{}
        //     Race newRace  =  new Race(newchar);
        //     Background newBG = new Background(newchar);
        //     Race profieces + class profieces + mods
        
        //     Skill {
        //         skilllist = class.skilllist + race.skilllist + level.skilllest
        //         skill1= null;
        //         skill2 =null;
        //         considers race class
        //     }
        // } 
    }
}
