using System;
using System.ComponentModel.DataAnnotations;

namespace CharacterGenerator.Models
{
    public class SpellAssoc
    {
        [Key]
        public int SpellAssocId { get; set; }
        public int SpellId { get; set; }
        public Spell SpellA  { get; set; }
        public int CharacterId { get; set; }
        public bool Prepped { get; set; }
        public bool AlwaysPrepped { get; set; }
        public NewCharacter PlayerA { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public SpellAssoc()
        {
            
        }
        public SpellAssoc (NewCharacter character, Spell spell)
        {
            CharacterId = character.CharacterId;
            PlayerA = character;
            SpellA = spell;
            SpellId = spell.SpellId;
            Prepped = false;
            AlwaysPrepped = false;
        }

        public void AlwaysPreppedChecker(NewCharacter test, Spell s, SpellAssoc a)
        {
            switch(test.playerClass.SubClassName)
            {
                case "Circle of the Land: Arctic":
                         if (s.SpellName == "Hold Person"||
                            s.SpellName == "Spike Growth"||
                            s.SpellName == "Sleet Storm"||
                            s.SpellName == "Slow"||
                            s.SpellName == "Freedom of Movement"||
                            s.SpellName == "Ice Storm"||
                            s.SpellName == "Commune with Nature"||
                            s.SpellName == "Cone of Cold")
                                {
                                    a.AlwaysPrepped = true;
                                };
                            break;

                    case "Circle of the Land: Coast":
                            if (s.SpellName == "Mirror Image"||
                                s.SpellName == "Misty Step"||
                                s.SpellName == "Water Breathing"||
                                s.SpellName == "Water Walk"||
                                s.SpellName == "Freedom of Movement"||
                                s.SpellName == "Control Water"||
                                s.SpellName == "Conjure Elemental"||
                                s.SpellName == "Scrying")
                                {
                                    a.AlwaysPrepped = true;
                                }
                            break;

                    case "Circle of the Land: Desert":
                            if (s.SpellName == "Blur"||
                                s.SpellName == "Silence"||
                                s.SpellName == "Create Food and Water"||
                                s.SpellName == "Protection from Energy"||
                                s.SpellName == "Blight"||
                                s.SpellName == "Hallucinatory Terrain"||
                                s.SpellName == "Insect Plague"||
                                s.SpellName == "Wall of Stone")
                                {
                                    a.AlwaysPrepped = true;
                                }
                            break;

                    case "Circle of the Land: Forest":
                            if (s.SpellName == "Barkskin"||
                                s.SpellName == "Spider Climb"||
                                s.SpellName == "Call Lightning"||
                                s.SpellName == "Plant Growth"||
                                s.SpellName == "Freedom of Movement"||
                                s.SpellName == "Divination"||
                                s.SpellName == "Commune with Nature"||
                                s.SpellName == "Tree Stride")
                                {
                                    a.AlwaysPrepped = true;
                                }
                            break;
                    
                    case "Circle of the Land: Grassland":
                            if (s.SpellName == "Invisibilty"||
                                 s.SpellName == "Pass Without Trace"||
                                 s.SpellName == "Daylight"||
                                 s.SpellName == "Haste"||
                                 s.SpellName == "Freedom of Movement"||
                                 s.SpellName == "Divination"||
                                 s.SpellName == "Dream"||
                                 s.SpellName == "Insect Plague")
                                 {
                                    a.AlwaysPrepped = true;
                                 }
                            break;

                    case "Circle of the Land: Mountain":
                            if (s.SpellName == "Spider Climb"||
                                s.SpellName == "Spike Growth"||
                                s.SpellName == "Lightning Bolt"||
                                s.SpellName == "Meld into Stone"||
                                s.SpellName == "Stone Shape"||
                                s.SpellName == "Stoneskin"||
                                s.SpellName == "Passwall"||
                                s.SpellName == "Wall of Stone")
                                {
                                    a.AlwaysPrepped = true;
                                }
                            break;

                    case "Circle of the Land: Swamp":
                            if (s.SpellName == "Acid Arrow"||
                                s.SpellName == "Darkness"||
                                s.SpellName == "Water Walk"||
                                s.SpellName == "Stinking Cloud"||
                                s.SpellName == "Freedom of Movement"||
                                s.SpellName == "Locate Creature"||
                                s.SpellName == "Insect Plague"||
                                s.SpellName == "Scrying")
                                {
                                    a.AlwaysPrepped = true;
                                }
                            break;

                    case "Life Domain":
                            if(s.SpellName == "Bless"||
                                s.SpellName == "Cure Wounds"||
                                s.SpellName == "Lesser Restoration"||
                                s.SpellName == "Spiritual Weapon"||
                                s.SpellName == "Beacon of Hope"||
                                s.SpellName == "Revivify"||
                                s.SpellName == "Death Ward"||
                                s.SpellName == "Guardian of Faith"||
                                s.SpellName == "Mass Cure Wounds"||
                                s.SpellName == "Raise Dead")
                                {
                                    a.AlwaysPrepped = true;
                                }
                            break;

                    case "Oath of Devotion":
                            if(s.SpellName == "Protection from Evil & Good"||
                                s.SpellName == "Sanctuary"||
                                s.SpellName == "Lesser Restoration"||
                                s.SpellName == "Zone of Truth"||
                                s.SpellName == "Beacon of Hope"||
                                s.SpellName == "Dispel Magic"||
                                s.SpellName == "Freedom of Movement"||
                                s.SpellName == "Guardian of Faith"||
                                s.SpellName == "Commune"||
                                s.SpellName == "Flame Strike")
                                {
                                    a.AlwaysPrepped = true;
                                }
                            break;

                    case "The Fiend":
                            if (s.SpellName == "Burning Hands"||
                                s.SpellName == "Command"||
                                s.SpellName == "Blindness/Deafness"||
                                s.SpellName == "Scorching Ray"||
                                s.SpellName == "Fireball"||
                                s.SpellName == "Stinking Cloud"||
                                s.SpellName == "Fire Shield"||
                                s.SpellName == "Wall of Fire"||
                                s.SpellName == "Flame Strike"||
                                s.SpellName == "Hallow")
                                {
                                    a.AlwaysPrepped = true;
                                }
                            break;

            }
            if (test.playerClass.ClassName == "Sorcerer" ||
                test.playerClass.ClassName == "Warlock" ||
                test.playerClass.ClassName == "Bard" ||
                test.playerClass.ClassName == "Ranger")
                {
                    a.AlwaysPrepped = true;
                }
            
        }
    }
}