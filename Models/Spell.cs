using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CharacterGenerator.Models
{
    public class Spell
    {
        [Key]
        public int SpellId {get; set;}
        public int CharacterId {get; set;}
        public PlayerStat Caster {get; set;}
        public string SpellName {get; set;}
        public string Source1 {get; set;}
        public string Source2 {get; set;}
        public string Source3 {get; set;}
        public string Source4 {get; set;}
        public string Source5 {get;set;}
        public string Source6 {get;set;}
        public string Source7 {get;set;}

        public int SpellLevel {get; set;}

        public List<SpellAssoc> Players { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Spell(){}
        public Spell(int SPL_LVL, string name, string source1)
        {
            SpellLevel = SPL_LVL;
            SpellName = name;
            Source1 = source1;
        }
        public Spell(int SPL_LVL, string name, string source1, string source2)
        {
            SpellLevel = SPL_LVL;
            SpellName = name;
            Source1 = source1;
            Source2 = source2;
        }
        public Spell(int SPL_LVL, string name, string source1, string source2, string source3)
        {
            SpellLevel = SPL_LVL;
            SpellName = name;
            Source1 = source1;
            Source2 = source2;
            Source3 = source3;
        }
        public Spell(int SPL_LVL, string name, string source1, string source2, string source3, string source4)
        {
            SpellLevel = SPL_LVL;
            SpellName = name;
            Source1 = source1;
            Source2 = source2;
            Source3 = source3;
            Source4 = source4;
        }
        public Spell(int SPL_LVL, string name, string source1, string source2, string source3, string source4, string source5)
        {
            SpellLevel = SPL_LVL;
            SpellName = name;
            Source1 = source1;
            Source2 = source2;
            Source3 = source3;
            Source4 = source4;
            Source5 = source5;
        }
        public Spell(int SPL_LVL, string name, string source1, string source2, string source3, string source4, string source5, string source6)
        {
            SpellLevel = SPL_LVL;
            SpellName = name;
            Source1 = source1;
            Source2 = source2;
            Source3 = source3;
            Source4 = source4;
            Source5 = source5;
            Source6 = source6;
        }
        public Spell(int SPL_LVL, string name, string source1, string source2, string source3, string source4, string source5, string source6, string source7)
        {
            SpellLevel = SPL_LVL;
            SpellName = name;
            Source1 = source1;
            Source2 = source2;
            Source3 = source3;
            Source4 = source4;
            Source5 = source5;
            Source6 = source6;
            Source7 = source7;
        }


        public List<Spell> GenerateSpellList()
        {
            List<Spell> BuildSpells = new List<Spell>();
            string Wizard = "Wizard";
            string Bard = "Bard";
            string Druid = "Druid";
            string Ranger = "Ranger";
            string Paladin = "Paladin";
            string Cleric = "Cleric";
            string Sorcerer = "Sorcerer";
            string Warlock = "Warlock";

            BuildSpells.Add(new Spell(0,"Acid Splash", "Wizard"));
            BuildSpells.Add(new Spell(0, "Chill Touch", "Sorcerer", "Warlock", "Wizard"));
            BuildSpells.Add(new Spell(0, "Dancing Lights", "Sorcerer", "Bard", "Wizard"));
            BuildSpells.Add(new Spell(0, "Druidcraft", "Druid"));
            BuildSpells.Add(new Spell(0, "Eldritch Blast", "Warlock"));
            BuildSpells.Add(new Spell(0, "Firebolt", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(0, "Guidance", "Cleric", "Druid"));
            BuildSpells.Add(new Spell(0, "Light", "Bard", "Cleric", "Sorcerer"));
            BuildSpells.Add(new Spell(0, "Mage Hand", "Bard", "Sorcerer", "Warlock", "Wizard"));
            BuildSpells.Add(new Spell(0, "Mending", "Bard", "Druid", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(0, "Message", "Bard","Sorcerer"));
            BuildSpells.Add(new Spell(0, "Minor Illusion", "Bard", "Sorcerer", "Warlock", "Wizard"));
            BuildSpells.Add(new Spell(0, "Poison Spray", "Druid", "Sorcerer", "Warlock", "Wizard"));
            BuildSpells.Add(new Spell(0, "Prestidigitation", "Bard", "Sorcerer", "Warlock", "Wizard"));
            BuildSpells.Add(new Spell(0, "Produce Flame", "Druid"));
            BuildSpells.Add(new Spell(0, "Ray of Frost", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(0, "Resistance", "Cleric:", "Druid"));
            BuildSpells.Add(new Spell(0, "Sacred Flame", "Cleric"));
            BuildSpells.Add(new Spell(0, "Shillelagh", "Druid"));
            BuildSpells.Add(new Spell(0, "Shocking Grasp", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(0, "Spare the Dying", "Cleric"));
            BuildSpells.Add(new Spell(0, "Thaumaturgy", "Cleric"));
            BuildSpells.Add(new Spell(0, "True Strike", "Bard", "Sorcerer", "Warlock", "Wizard"));
            BuildSpells.Add(new Spell(0, "Viscious Mockery", "Bard"));
            //Level 1
            BuildSpells.Add(new Spell(1, "Alarm", "Ranger", "Wizard"));
            BuildSpells.Add(new Spell(1, "Animal Friendship", "Bard", "Druid", "Ranger"));
            BuildSpells.Add(new Spell(1, "Bane", "Bard", "Cleric"));
            BuildSpells.Add(new Spell(1, "Bless", "Cleric", "Paladin"));
            BuildSpells.Add(new Spell(1, "Burning Hands", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(1, "Charm Person", "Bard", "Druid", "Sorcerer", "Warlock", "Wizard"));
            BuildSpells.Add(new Spell(1, "Color Spray", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(1, "Command", "Cleric", "Paladin"));
            BuildSpells.Add(new Spell(1, "Comprehend Languages", "Bard", "Sorcerer", "Warlock", "Wizard"));
            BuildSpells.Add(new Spell(1, "Create or Destroy Water", "Cleric", "Druid"));
            BuildSpells.Add(new Spell(1, "Cure Wounds", "Bard", "Cleric", "Druid"));
            BuildSpells.Add(new Spell(1, "Detect Evil and Good", "Cleric", "Paladin"));
            BuildSpells.Add(new Spell(1, "Detect Magic", "Bard", "Cleric", "Druid", "Paladin", "Ranger", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(1, "Detect Poison and Disease", "Cleric", "Druid", "Paladin", "Ranger"));
            BuildSpells.Add(new Spell(1, "Disguise Self", "Bard", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(1, "Divine Favor","Paladin"));
            BuildSpells.Add(new Spell(1, "Entangle", "Druid"));
            BuildSpells.Add(new Spell(1, "Expeditious Retreat", "Sorcerer", "Warlock", "Wizard"));
            BuildSpells.Add(new Spell(1, "Faerie Fire", "Bard", "Druid"));
            BuildSpells.Add(new Spell(1, "False Life", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(1, "Feather Fall", "Bard", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(1, "Find Familiar", "Wizard"));
            BuildSpells.Add(new Spell(1, "Floating Disc", "Wizard"));
            BuildSpells.Add(new Spell(1, "Fog Cloud", "Druid", "Ranger", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(1, "Goodberry", "Druid", "Ranger"));
            BuildSpells.Add(new Spell(1, "Grease", "Wizard"));
            BuildSpells.Add(new Spell(1, "Guiding Bolt", "Cleric"));
            BuildSpells.Add(new Spell(1, "Healing Word", "Bard", "Cleric", "Druid"));
            BuildSpells.Add(new Spell(1, "Hellish Rebuke", "Warlock"));
            BuildSpells.Add(new Spell(1, "Heroism", "Bard", "Paladin"));
            BuildSpells.Add(new Spell(1, "Hideous Laughter", "Bard", "Wizard"));
            BuildSpells.Add(new Spell(1, "Hunter's Mark", "Ranger"));
            BuildSpells.Add(new Spell(1, "Identify", "Bard", "Wizard"));
            BuildSpells.Add(new Spell(1, "Illusory Script", "Bard", "Wizard"));
            BuildSpells.Add(new Spell(1, "Inflict Wounds", "Cleric"));
            BuildSpells.Add(new Spell(1, "Jump", "Druid", "Ranger", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(1, "Longstrider", "Bard", "Druid", "Ranger", "Wizard"));
            BuildSpells.Add(new Spell(1, "Mage Armor", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(1, "Magic Missile", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(1, "Protection from Evil and Good", "Cleric", "Paladin", "Warlock", "Wizard"));
            BuildSpells.Add(new Spell(1, "Purify Food and Drink", "Cleric", "Druid", "Paladin"));
            BuildSpells.Add(new Spell(1, "Sanctuary", "Cleric"));
            BuildSpells.Add(new Spell(1, "Shield", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(1, "Shield of Faith", "Cleric", "Paladin"));
            BuildSpells.Add(new Spell(1, "Silent Image", "Bard", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(1, "Sleep", "Bard", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(1, "Speak With Animals", "Bard", "Druid", "Ranger"));
            BuildSpells.Add(new Spell(1, "Thunderwave", "Bard", "Druid","Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(1, "Unseen Servant", "Bard", "Warlock", "Wizard"));
            //level 2
            BuildSpells.Add(new Spell(2, "Acid Arrow", "Wizard"));
            BuildSpells.Add(new Spell(2, "Aid", "Cleric", "Paladin"));
            BuildSpells.Add(new Spell(2, "Alter Self", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(2, "Animal Messenger", "Bard", "Druid", "Ranger"));
            BuildSpells.Add(new Spell(2, "Arcane Lock", "Wizard"));
            BuildSpells.Add(new Spell(2, "Arcanist's Magic Aura", "Wizard"));
            BuildSpells.Add(new Spell(2, "Augury", "Cleric"));
            BuildSpells.Add(new Spell(2, "Bark Skin", "Druid", "Ranger"));
            BuildSpells.Add(new Spell(2, "Blindness/Deafness", "Bard", "Cleric", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(2, "Blur", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(2, "Branding Smite", "Paladin"));
            BuildSpells.Add(new Spell(2, "Calm Emotions", "Bard", "Cleric"));
            BuildSpells.Add(new Spell(2, "Continual Flame", "Cleric", "Wizard"));
            BuildSpells.Add(new Spell(2, "Darkness", "Sorcerer", "Warlock", "Wizard"));
            BuildSpells.Add(new Spell(2, "Darkvision", "Druid", "Ranger", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(2, "Detect Thoughts", "Bard", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(2, "Enhance Ability", "Bard", "Cleric", "Druid", "Sorcerer"));
            BuildSpells.Add(new Spell(2, "Enlarge/Reduce", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(2, "Enthrall", "Bard", "Warlock"));
            BuildSpells.Add(new Spell(2, "Find Steed", "Paladin"));
            BuildSpells.Add(new Spell(2, "Find Traps","Cleric", "Druid", "Ranger"));
            BuildSpells.Add(new Spell(2, "Flame Blade", "Druid"));
            BuildSpells.Add(new Spell(2, "Flaming Sphere", "Druid", "Wizard"));
            BuildSpells.Add(new Spell(2, "Gentle Repose", "Cleric", "Wizard"));
            BuildSpells.Add(new Spell(2, "Gust of Wind", "Druid", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(2, "Heat Metal", "Bard", "Druid"));
            BuildSpells.Add(new Spell(2, "Hold Person", "Bard", "Cleric", "Druid", "Sorcerer", "Warlock", "Wizard"));
            BuildSpells.Add(new Spell(2, "Invisibility", "Bard", "Sorcerer", "Warlock", "Wizard"));
            BuildSpells.Add(new Spell(2, "Knock", "Bard", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(2, "Lesser Restoration", "Bard", "Cleric", "Druid", "Paladin", "Ranger"));
            BuildSpells.Add(new Spell(2, "Levitate", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(2, "Locate Animals or Plants", "Druid","Ranger"));
            BuildSpells.Add(new Spell(2, "Locate Object", "Bard","Cleric", "Druid", "Paladin", "Ranger", "Wizard"));
            BuildSpells.Add(new Spell(2, "Magic Mouth", "Bard", "Wizard"));
            BuildSpells.Add(new Spell(2, "Magic Weapon", "Paladin", "Wizard"));
            BuildSpells.Add(new Spell(2, "Mirror Image", "Sorcerer","Warlock", "Wizard"));
            BuildSpells.Add(new Spell(2, "Misty Step", "Sorcerer", "Warlock", "Wizard"));
            BuildSpells.Add(new Spell(2, "Moonbeam", "Druid"));
            BuildSpells.Add(new Spell(2, "Pass Without Trace", "Druid", "Ranger"));
            BuildSpells.Add(new Spell(2, "Prayer of Healing", "Cleric"));
            BuildSpells.Add(new Spell(2, "Protection from Poison", "Cleric", "Druid", "Paladin", "Ranger"));
            BuildSpells.Add(new Spell(2, "Ray of Enfeeblement", "Warlock", "Wizard"));
            BuildSpells.Add(new Spell(2, "Rope Trick", "Wizard"));
            BuildSpells.Add(new Spell(2, "Scorching Ray", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(2, "See Invisibility", "Bard", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(2, "Shatter", "Bard", "Sorcerer", "Warlock", "Wizard"));
            BuildSpells.Add(new Spell(2, "Silence", "Bard", "Cleric", "Ranger"));
            BuildSpells.Add(new Spell(2, "Spider Climb", "Sorcerer","Warlock", "Wizard"));
            BuildSpells.Add(new Spell(2, "Spike Growth", "Druid"));
            BuildSpells.Add(new Spell(2, "Spiritual Weapon", "Cleric"));
            BuildSpells.Add(new Spell(2, "Suggestion", "Bard", "Sorcerer", "Warlock", "Wizard"));
            BuildSpells.Add(new Spell(2, "Warding Bond", "Cleric"));
            BuildSpells.Add(new Spell(2, "Web", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(2, "Zone of Truth","Bard", "Cleric", "Paladin"));
            // Level 3
            BuildSpells.Add(new Spell(3, "Animate Dead", "Wizard", "Cleric"));
            BuildSpells.Add(new Spell(3, "Beacon of Hope", "Cleric"));
            BuildSpells.Add(new Spell(3, "Bestow Curse", "Bard", "Cleric", "Wizard"));
            BuildSpells.Add(new Spell(3, "Blink", "Wizard", "Sorcerer"));
            BuildSpells.Add(new Spell(3, "Call Lightning", "Druid"));
            BuildSpells.Add(new Spell(3, "Clairvoyance", "Bard", "Cleric", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(3, "Conjure Animals", "Druid", "Ranger"));
            BuildSpells.Add(new Spell(3, "Counterspell", "Warlock", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(3, "Create Food and Water", "Palandin", "Cleric", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(3, "Daylight", "Druid", "Cleric", "Paladin", "Ranger", "Sorcerer"));
            BuildSpells.Add(new Spell(3, "Dispel Magic", "Bard", "Cleric", "Druid", "Paladin", "Sorcerer", "Warlock", "Wizard"));
            BuildSpells.Add(new Spell(3, "Fear", "Bard", "Warlock", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(3, "Fireball","Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(3, "Fly","Warlock", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(3, "Gaseous Form", "Warlock", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(3, "Glyph of Warding", "Bard", "Cleric", "Wizard"));
            BuildSpells.Add(new Spell(3, "Haste", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(3, "Hypnotic Pattern", "Bard", "Warlock", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(3, "Lightning Bolt", "Bard", "Warlock", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(3, "Magic Circle", "Cleric", "Warlock", "Paladin", "Wizard"));
            BuildSpells.Add(new Spell(3, "Major Image", "Bard", "Sorcerer", "Warlock", "Wizard"));
            BuildSpells.Add(new Spell(3, "Mass Healing", "Cleric"));
            BuildSpells.Add(new Spell(3, "Mass Healing", "Cleric"));
            BuildSpells.Add(new Spell(3, "Meld into Stone", "Cleric", "Druid"));
            BuildSpells.Add(new Spell(3, "Nondetection", "Bard", "Rangers", "Wizards"));
            BuildSpells.Add(new Spell(3, "Phantom Steed", "Wizard"));
            BuildSpells.Add(new Spell(3, "Plant Growth", "Bard", "Druid", "Ranger"));
            BuildSpells.Add(new Spell(3, "Protection From Energy", "Cleric", "Druid", "Ranger", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(3, "Remove Curse", "Cleric", "Paladin", "Warlock", "Wizard"));
            BuildSpells.Add(new Spell(3, "Revivify", "Cleric", "Paladin"));
            BuildSpells.Add(new Spell(3, "Sending", "Cleric", "Bard", "Wizard"));
            BuildSpells.Add(new Spell(3, "Sleet Storm", "Druic", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(3, "Slow", "Srocerer", "Wizard"));
            BuildSpells.Add(new Spell(3, "Speak with Dead", "Cleric", "Bard"));
            BuildSpells.Add(new Spell(3, "Speak with Plants", "Druid", "Ranger"));
            BuildSpells.Add(new Spell(3, "Spirit Gaurdians", "Cleric"));
            BuildSpells.Add(new Spell(3, "Stinking Cloud", "Sorcerer", "Wizard" ));
            BuildSpells.Add(new Spell(3, "Tiny Hut", "Wizard", "Bard" ));
            BuildSpells.Add(new Spell(3, "Tongues", "Cleric", "Warlock", "Sorcerer", "Wizard", "Bard" ));
            BuildSpells.Add(new Spell(3, "Vampric Touch", "Warlock", "Wizard" ));
            BuildSpells.Add(new Spell(3, "Water Breathing", "Druid", "Ranger", "Sorcerer", "Wizard" ));
            BuildSpells.Add(new Spell(3, "Water Walk", "Cleric", "Druid", "Ranger", "Sorcerer" ));
            BuildSpells.Add(new Spell(3, "Wind Wall", "Druid", "Ranger"));
            // Level 4
            BuildSpells.Add(new Spell(4, "Arcane Eye", "Wizard"));
            BuildSpells.Add(new Spell(4, "Banishment", "Cleric", "Paladin", "Sorcerer", "Warlock", "Wizard"));
            BuildSpells.Add(new Spell(4, "Black Tentacles", "Wizard"));
            BuildSpells.Add(new Spell(4, "Blight", "Druid", "Sorcerer", "Warlock", "Wizard"));
            BuildSpells.Add(new Spell(4, "Compulsion", "Bard"));
            BuildSpells.Add(new Spell(4, "Confusion", "Bard", "Druid", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(4, "Conjure Minor Elementals", "Druid", "Wizard"));
            BuildSpells.Add(new Spell(4, "Conjure Woodland Beings", "Druid", "Ranger"));
            BuildSpells.Add(new Spell(4, "Control Water", "Cleric", "Druid", "Wizard"));
            BuildSpells.Add(new Spell(4, "Death Ward", "Cleric", "Paladin"));
            BuildSpells.Add(new Spell(4, "Dimension Door", "Bard","Sorcerer", "Warlock", "Wizard"));
            BuildSpells.Add(new Spell(4, "Divination", "Cleric"));
            BuildSpells.Add(new Spell(4, "Dominate Beast", "Druid", "Sorcerer"));
            BuildSpells.Add(new Spell(4, "Fabricate", "Wizard"));
            BuildSpells.Add(new Spell(4, "Faithel Hound", "Wizard"));
            BuildSpells.Add(new Spell(4, "Fire Shield", "Wizard"));
            BuildSpells.Add(new Spell(4, "Freedom of Movement", "Bard", "Cleric", "Druid", "Ranger"));
            BuildSpells.Add(new Spell(4, "Giant Insect", "Druid"));
            BuildSpells.Add(new Spell(4, "Greater Invisibility", "Bard", "Sorcerer","Wizard"));
            BuildSpells.Add(new Spell(4, "Guardian of Faith", "Cleric"));
            BuildSpells.Add(new Spell(4, "Hallucinatory Terrain", "Bard","Warlock", "Wizard"));
            BuildSpells.Add(new Spell(4, "Locate Creature", "Bard","Cleric","Druid", "Paladin","Ranger", "Wizard"));
            BuildSpells.Add(new Spell(4, "Phantasmal Killer","Wizard"));
            BuildSpells.Add(new Spell(4, "Polymorph", "Bard", "Druid", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(4, "Private Sanctum", "Wizard"));
            BuildSpells.Add(new Spell(4, "Resilient Sphere", "Wizard"));
            BuildSpells.Add(new Spell(4, "Secret Chest", "Wizard"));
            BuildSpells.Add(new Spell(4, "Stone Shape", "Cleric", "Druid", "Wizard"));
            BuildSpells.Add(new Spell(4, "Stoneskin", "Druid", "Ranger", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(4, "Wall of Fire", "Druid","Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(4, "Ice Storm", "Druid","Sorcerer", "Wizard"));
                       // 5th Level
            BuildSpells.Add(new Spell(5, "Animate Objects", "Bard","Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(5, "Antilife Shell", "Druid"));
            BuildSpells.Add(new Spell(5, "Arcane Hand", "Wizard"));
            BuildSpells.Add(new Spell(5, "Awaken", "Bard", "Druid"));
            BuildSpells.Add(new Spell(5, "Cloudkill", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(5, "Commune", "Cleric"));
            BuildSpells.Add(new Spell(5, "Commune with Nature", "Druid", "Ranger"));
            BuildSpells.Add(new Spell(5, "Cone of Cold", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(5, "Conjure Elemental", "Druid", "Wizard"));
            BuildSpells.Add(new Spell(5, "Contact Other Plane", "Warlock", "Wizard"));
            BuildSpells.Add(new Spell(5, "Contagion", "Cleric", "Druid"));
            BuildSpells.Add(new Spell(5, "Creation", "Sorcerer", "Wizard"));
            BuildSpells.Add(new Spell(5, "Dispel Evil and Good", "Cleric", "Paladin"));
            BuildSpells.Add(new Spell(5, "Dominate Person", "Sorcerer", "Bard", "Wizard"));
            BuildSpells.Add(new Spell(5, "Dream", Wizard, Warlock, "Bard"));
            BuildSpells.Add(new Spell(5, "Flame Strike", Cleric));
            BuildSpells.Add(new Spell(5, "Geas", Bard, Cleric, Druid, Paladin, Wizard));
            BuildSpells.Add(new Spell(5, "Greater Restoration", Bard, Cleric, Druid));
            BuildSpells.Add(new Spell(5, "Hallow", Cleric));
            BuildSpells.Add(new Spell(5, "Hold Monster", Bard, Sorcerer, Warlock, Wizard));
            BuildSpells.Add(new Spell(5, "Insect Plague", Sorcerer, Cleric, Druid));
            BuildSpells.Add(new Spell(5, "Legend Lore", Bard, Cleric, Wizard));
            BuildSpells.Add(new Spell(5, "Mass Cure Wounds", Bard, Cleric, Druid));
            BuildSpells.Add(new Spell(5, "Mislead", Bard, Wizard));
            BuildSpells.Add(new Spell(5, "Modify Memory", Bard, Wizard));
            BuildSpells.Add(new Spell(5, "Passwall", Wizard));
            BuildSpells.Add(new Spell(5, "Planar Binding", Bard, Cleric, Druid, Wizard));
            BuildSpells.Add(new Spell(5, "Raise Dead", Bard, Cleric, Paladin));
            BuildSpells.Add(new Spell(5, "Reincarnate", Druid));
            BuildSpells.Add(new Spell(5, "Scrying", Bard, Cleric, Druid, Warlock, Wizard));
            BuildSpells.Add(new Spell(5, "Seeming", Bard, Sorcerer, Wizard));
            BuildSpells.Add(new Spell(5, "Telekinesis", Sorcerer, Wizard));
            BuildSpells.Add(new Spell(5, "Telepathic Bond", Wizard));
            BuildSpells.Add(new Spell(5, "Teleportation Circle", Bard, Sorcerer, Wizard));
            BuildSpells.Add(new Spell(5, "Tree Stride", Ranger, Druid));
            BuildSpells.Add(new Spell(5, "Wall of Force", Wizard));
            BuildSpells.Add(new Spell(5, "Wall of Stone", Wizard, Sorcerer, Druid));
            // Level 6
            BuildSpells.Add(new Spell(6, "Blade Barrier", Cleric));
            BuildSpells.Add(new Spell(6, "Chain Lightning", Sorcerer, Wizard));
            BuildSpells.Add(new Spell(6, "Circle of Death", Sorcerer, Warlock, Wizard));
            BuildSpells.Add(new Spell(6, "Conjure Fey", Druid, Warlock));
            BuildSpells.Add(new Spell(6, "Contingency", Wizard));
            BuildSpells.Add(new Spell(6, "Create Undead", Cleric, Warlock, Wizard));
            BuildSpells.Add(new Spell(6, "Disintegrate", Sorcerer, Wizard));
            BuildSpells.Add(new Spell(6, "eyebite", Bard, Sorcerer, Warlock, Wizard));
            BuildSpells.Add(new Spell(6, "Find the Path", Bard, Cleric, Druid));
            BuildSpells.Add(new Spell(6, "Flesh to Stone", Warlock, Wizard));
            BuildSpells.Add(new Spell(6, "Forbiddance", Cleric));
            BuildSpells.Add(new Spell(6, "Freezing Sphere", Wizard));
            BuildSpells.Add(new Spell(6, "Globe of Invulnerability", Sorcerer, Wizard));
            BuildSpells.Add(new Spell(6, "Guards and Wards", Bard, Wizard));
            BuildSpells.Add(new Spell(6, "Harm", Cleric));
            BuildSpells.Add(new Spell(6, "Heal", Cleric, Druid));
            BuildSpells.Add(new Spell(6, "Heroes' Feast", Cleric, Druid));
            BuildSpells.Add(new Spell(6, "Instant Summons", Wizard));
            BuildSpells.Add(new Spell(6, "Irresistable Dance", Bard, Wizard));
            BuildSpells.Add(new Spell(6, "Magic Jar", Wizard));
            BuildSpells.Add(new Spell(6, "Mass Suggestion", Bard, Sorcerer, Warlock, Wizard));
            BuildSpells.Add(new Spell(6, "Move Earth", Druid, Sorcerer, Wizard));
            BuildSpells.Add(new Spell(6, "Planar Ally", Cleric));
            BuildSpells.Add(new Spell(6, "Programmed Illusion", Bard, Wizard));
            BuildSpells.Add(new Spell(6, "Sunbeam", Druid, Sorcerer, Wizard));
            BuildSpells.Add(new Spell(6, "Transport via Plants", Druid));
            BuildSpells.Add(new Spell(6, "True Seeing", Bard, Cleric, Sorcerer, Warlock, Wizard));
            BuildSpells.Add(new Spell(6, "Wall of Ice", Wizard));
            BuildSpells.Add(new Spell(6, "Wall of Thorns", Druid));
            BuildSpells.Add(new Spell(6, "Wind Walk", Druid));
            BuildSpells.Add(new Spell(6, "Word of Recall", Cleric));
            //Level 7
            BuildSpells.Add(new Spell(7, "Arcane Sword", Bard, Wizard));
            BuildSpells.Add(new Spell(7, "Conjure Celestial", Cleric));
            BuildSpells.Add(new Spell(7, "Delayed Blast Fireball", Wizard));
            BuildSpells.Add(new Spell(7, "Divine Word", Cleric));
            BuildSpells.Add(new Spell(7, "Etherealness", Bard, Cleric, Sorcerer, Warlock, Wizard));
            BuildSpells.Add(new Spell(7, "Finger of Death", Sorcerer, Warlock, Wizard));
            BuildSpells.Add(new Spell(7, "Firestorm", Cleric, Druid, Sorcerer));
            BuildSpells.Add(new Spell(7, "Forcecage", Bard, Warlock, Wizard));
            BuildSpells.Add(new Spell(7, "Magnificent Mansion", Bard, Wizard));
            BuildSpells.Add(new Spell(7, "Mirage Arcane", Bard, Druid, Wizard));
            BuildSpells.Add(new Spell(7, "Plane Shift", Bard, Wizard));
            BuildSpells.Add(new Spell(7, "Prismatic Spray", Sorcerer, Wizard));
            BuildSpells.Add(new Spell(7, "Project Image", Bard, Wizard));
            BuildSpells.Add(new Spell(7, "Regenerate", Bard, Cleric, Druid));
            BuildSpells.Add(new Spell(7, "Resurrection", Bard, Cleric));
            BuildSpells.Add(new Spell(7, "Reverse Gravity", Druid, Sorcerer, Wizard));
            BuildSpells.Add(new Spell(7, "Sequester", Wizard));
            BuildSpells.Add(new Spell(7, "Simulacrum", Wizard));
            BuildSpells.Add(new Spell(7, "Symbol", Bard, Cleric, Wizard));
            BuildSpells.Add(new Spell(7, "Teleport", Bard, Sorcerer, Wizard));
               //Level 8
            BuildSpells.Add(new Spell(8, "Animal Shapes", Druid));
            BuildSpells.Add(new Spell(8, "Antimagic Field", Cleric, Wizard));
            BuildSpells.Add(new Spell(8, "Antipathy/Sympathy", Druid, Wizard));
            BuildSpells.Add(new Spell(8, "Clone", Wizard));
            BuildSpells.Add(new Spell(8, "Control Weather", Cleric, Druid, Wizard));
            BuildSpells.Add(new Spell(8, "Demiplane", Warlock, Wizard));
            BuildSpells.Add(new Spell(8, "Dominate Monster", Bard, Sorcerer, Warlock, Wizard));
            BuildSpells.Add(new Spell(8, "Earthquake", Cleric, Druid, Sorcerer));
            BuildSpells.Add(new Spell(8, "Feeblemind",Bard, Druid, Warlock, Wizard));
            BuildSpells.Add(new Spell(8, "Glibness", Bard, Warlock));
            BuildSpells.Add(new Spell(8, "Holy Aura", Cleric));
            BuildSpells.Add(new Spell(8, "Incindiary Cloud", Sorcerer, Wizard));
            BuildSpells.Add(new Spell(8, "Maze", Wizard));
            BuildSpells.Add(new Spell(8, "Mind Blank", Bard, Wizard));
            BuildSpells.Add(new Spell(8, "Power Word Stun", Bard, Sorcerer, Warlock, Wizard));
            BuildSpells.Add(new Spell(8, "Sunburst", Druid, Sorcerer, Wizard));
            //Level 9
            BuildSpells.Add(new Spell(9, "Astral Projection", Cleric, Warlock, Wizard));
            BuildSpells.Add(new Spell(9, "Foresight", Bard, Druid, Warlock, Wizard));
            BuildSpells.Add(new Spell(9, "Gate", Cleric, Sorcerer, Wizard));
            BuildSpells.Add(new Spell(9, "Imprisonment", Warlock, Wizard));
            BuildSpells.Add(new Spell(9, "Meteor Swarm", Sorcerer, Wizard));
            BuildSpells.Add(new Spell(9, "Power Word Kill", Bard, Sorcerer, Warlock, Wizard));
            BuildSpells.Add(new Spell(9, "Prismatic Wall", Wizard));
            BuildSpells.Add(new Spell(9, "Shapechange", Druid, Wizard));
            BuildSpells.Add(new Spell(9, "Time Stop", Sorcerer, Wizard));
            BuildSpells.Add(new Spell(9, "True Polymorph", Bard, Druid, Warlock, Wizard));
            BuildSpells.Add(new Spell(9, "Weird", Wizard));
            BuildSpells.Add(new Spell(9, "Wish", Sorcerer, Wizard));
            BuildSpells.Add(new Spell(9, "Storm of Vengeance", Druid));

            return BuildSpells;
            
        }
    }

}