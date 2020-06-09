using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CharacterGenerator.Models
{
    public class Feature
    {
        [Key]
        public int FeatureId {get; set;}
        public int CharacterId {get; set;}
        public string FeatSource {get; set;}
        public string FeatureName {get; set;}
        public int FeatLevel {get; set;}
        public List<FeatureAssoc> Players { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Feature(){}

        public Feature(string source, string name, int level_obtained)
        {
            FeatSource = source;
            FeatureName = name;
            FeatLevel = level_obtained;
        }


        public List<Feature> GenerateFeatureList(){
            List<Feature> BuildFeatures = new List<Feature>();
                //Barbarian
                BuildFeatures.Add(new Feature("Barbarian", "Rage", 1));
                BuildFeatures.Add(new Feature("Barbarian", "Unarmored Defense (Barbarian)", 1));
                BuildFeatures.Add(new Feature("Barbarian", "Reckless Attack", 2));
                BuildFeatures.Add(new Feature("Barbarian", "Danger Sense", 2)); 
                BuildFeatures.Add(new Feature("Barbarian", "Extra Attack", 5));
                BuildFeatures.Add(new Feature("Barbarian", "Fast Movement", 5));
                BuildFeatures.Add(new Feature("Barbarian", "Feral Instinct", 7));
                BuildFeatures.Add(new Feature("Barbarian", "Brutal Critical (1 Die)", 9));
                BuildFeatures.Add(new Feature("Barbarian", "Relentless Rage", 11));
                BuildFeatures.Add(new Feature("Barbarian", "Brutal Critical (2 Dice)", 13));
                BuildFeatures.Add(new Feature("Barbarian", "Persistent Rage", 15));
                BuildFeatures.Add(new Feature("Barbarian", "Brutal Critical (3 Dice)", 17));
                BuildFeatures.Add(new Feature("Barbarian", "Indomitable Might", 18));
                BuildFeatures.Add(new Feature("Barbarian", "Primal Champion", 20));
                //Berserker
                BuildFeatures.Add(new Feature("Path of the Berserker", "Frenzy", 3));
                BuildFeatures.Add(new Feature("Path of the Berserker", "Mindless Rage", 6));
                BuildFeatures.Add(new Feature("Path of the Berserker", "Intimidating Presence", 10));
                BuildFeatures.Add(new Feature("Path of the Berserker", "Retaliation", 14));
                //Totem
                BuildFeatures.Add(new Feature("Path of the Totem Warrior", "Spirit Seeker", 3));
                BuildFeatures.Add(new Feature("Path of the Totem Warrior", "Totem Spirit", 3)); //*Choices
                BuildFeatures.Add(new Feature("Path of the Totem Warrior", "Aspect of the Beast", 6)); //*Choices
                BuildFeatures.Add(new Feature("Path of the Totem Warrior", "Spirit Walker", 10));
                BuildFeatures.Add(new Feature("Path of the Totem Warrior", "Totemic Attunement", 14)); //*Choices
                //Bard
                BuildFeatures.Add(new Feature("Bard", "Bardic Inspiration (d6)", 1));
                BuildFeatures.Add(new Feature("Bard", "Spellcasting (Bard)", 1));
                BuildFeatures.Add(new Feature("Bard", "Jack of All Trades", 2));
                BuildFeatures.Add(new Feature("Bard", "Song of Rest (d6)", 2));
                BuildFeatures.Add(new Feature("Bard", "Expertise (2 Skills)", 3));
                BuildFeatures.Add(new Feature("Bard", "Bardic Inspiration (d8)", 5));
                BuildFeatures.Add(new Feature("Bard", "Font of Inspiration", 5));
                BuildFeatures.Add(new Feature("Bard", "Countercharm", 6));
                BuildFeatures.Add(new Feature("Bard", "Song of Rest (d8)", 9));
                BuildFeatures.Add(new Feature("Bard", "Bardic Inspiration (d10)", 10));
                BuildFeatures.Add(new Feature("Bard", "Magical Secrets (Level 10)", 10));
                BuildFeatures.Add(new Feature("Bard", "Expertise (2 Skills)", 10));
                BuildFeatures.Add(new Feature("Bard", "Song of Rest (d10)", 13));
                BuildFeatures.Add(new Feature("Bard", "Magical Secrets (Level 14)", 14));
                BuildFeatures.Add(new Feature("Bard", "Bardic Inspiration (d12)", 15));
                BuildFeatures.Add(new Feature("Bard", "Song of Rest (d12)", 17));
                BuildFeatures.Add(new Feature("Bard", "Magical Secrets (Level 18)", 18));
                BuildFeatures.Add(new Feature("Bard", "Superior Inspiration", 20));
                //College of Lore
                BuildFeatures.Add(new Feature("College of Lore", "Bonus Proficiencies (Any 3 Skills)", 3)); //*Gains 3 additional proficiencies
                BuildFeatures.Add(new Feature("College of Lore", "Cutting Words", 3));
                BuildFeatures.Add(new Feature("College of Lore", "Additional Magical Secrets", 6)); //*Gains 2 additional spells NOT KNOWN from any list, up to third level
                BuildFeatures.Add(new Feature("College of Lore", "Peerless Skill", 14));
                //College of Valor
                BuildFeatures.Add(new Feature("College of Valor", "Bonus Proficiencies (Medium Armor, Shields, Martial Weapons)", 3)); //*Proficiencies is armor/weapons. Should we account for those as well?
                BuildFeatures.Add(new Feature("College of Valor", "Combat Inspiration", 3));
                BuildFeatures.Add(new Feature("College of Valor", "Extra Attack", 6));
                BuildFeatures.Add(new Feature("College of Valor", "Battle Magic", 14));
                //Cleric
                BuildFeatures.Add(new Feature("Cleric", "Spellcasting (Cleric)", 1));
                BuildFeatures.Add(new Feature("Cleric", "Channel Divinity (1/rest)", 2));
                BuildFeatures.Add(new Feature("Cleric", "Destroy Undead (CR 1/2)", 5));
                BuildFeatures.Add(new Feature("Cleric", "Channel Divinity (2/Rest)", 6));
                BuildFeatures.Add(new Feature("Cleric", "Destroy Undead (CR 1)", 8));
                BuildFeatures.Add(new Feature("Cleric", "Divine Intervention", 10));
                BuildFeatures.Add(new Feature("Cleric", "Destroy Undead (CR 2)", 11));
                BuildFeatures.Add(new Feature("Cleric", "Destroy Undead (CR 3)", 14));
                BuildFeatures.Add(new Feature("Cleric", "Destroy Undead (CR 4)", 17));
                BuildFeatures.Add(new Feature("Cleric", "Channel Divinity (3/rest", 18));
                BuildFeatures.Add(new Feature("Cleric", "Divine Intervention Improvement", 20));
                //Knowledge Domain
                BuildFeatures.Add(new Feature("Knowledge Domain", "Blessings of Knowledge", 1));
                BuildFeatures.Add(new Feature("Knowledge Domain", "Channel Divinity: Knowledge of the Ages", 2));
                BuildFeatures.Add(new Feature("Knowledge Domain", "Channel Divinity: Read Thoughts", 6));
                BuildFeatures.Add(new Feature("Knowledge Domain", "Potent Spellcasting", 8));
                BuildFeatures.Add(new Feature("Knowledge Domain", "Visions of the Past", 17));
                //Life Domain
                BuildFeatures.Add(new Feature("Life Domain", "Bonus Proficiency (Heavy Armor)", 1));
                BuildFeatures.Add(new Feature("Life Domain", "Disciple of Life", 1));
                BuildFeatures.Add(new Feature("Life Domain", "Channel Divinity: Preserve Life", 2));
                BuildFeatures.Add(new Feature("Life Domain", "Blessed Healer", 6));
                BuildFeatures.Add(new Feature("Life Domain", "Divine Strike", 8));
                BuildFeatures.Add(new Feature("Life Domain", "Supreme Healing", 17));
                //Light Domain
                BuildFeatures.Add(new Feature("Light Domain", "Bonus Cantrip (Light Cantrip)", 1));
                BuildFeatures.Add(new Feature("Light Domain", "Warding Flare", 1));
                BuildFeatures.Add(new Feature("Light Domain", "Channel Divinity: Radiance of the Dawn", 2));
                BuildFeatures.Add(new Feature("Light Domain", "Improved Flare", 6));
                BuildFeatures.Add(new Feature("Light Domain", "Potent Spellcasting", 8));
                BuildFeatures.Add(new Feature("Light Domain", "Corona of Light", 17));
                //Nature Domain
                BuildFeatures.Add(new Feature("Nature Domain", "Acolyte of Nature", 1));
                BuildFeatures.Add(new Feature("Nature Domain", "Bonus Proficiency (Heavy Armor)", 1));
                BuildFeatures.Add(new Feature("Nature Domain", "Channel Divinity: Charm Animals and Plants", 2));
                BuildFeatures.Add(new Feature("Nature Domain", "Dampen Elements", 6));
                BuildFeatures.Add(new Feature("Nature Domain", "Divine Strike", 8));
                BuildFeatures.Add(new Feature("Nature Domain", "Master of Nature", 17));
                //Tempest Domain
                BuildFeatures.Add(new Feature("Tempest Domain", "Bonus Proficiency (Martial Weapons, Heavy Armor)", 1));
                BuildFeatures.Add(new Feature("Tempest Domain", "Wrath of the Storm", 1));
                BuildFeatures.Add(new Feature("Tempest Domain", "Channel Divinity: Destructive Wrath", 2));
                BuildFeatures.Add(new Feature("Tempest Domain", "Thunderbolt Strike", 6));
                BuildFeatures.Add(new Feature("Tempest Domain", "Divine Strike", 8));
                BuildFeatures.Add(new Feature("Tempest Domain", "Stormborn", 17));
                //Trickery Domain
                BuildFeatures.Add(new Feature("Trickery Domain", "Blessing of the Trickster", 1));
                BuildFeatures.Add(new Feature("Trickery Domain", "Channel Divinity: Invoke Duplicity", 2));
                BuildFeatures.Add(new Feature("Trickery Domain", "Channel Divinity: Cloak of Shadows", 6));
                BuildFeatures.Add(new Feature("Trickery Domain", "Divine Strike", 8));
                BuildFeatures.Add(new Feature("Trickery Domain", "Improved Duplicity", 17));
                //War Domain
                BuildFeatures.Add(new Feature("War Domain", "Bonus Proficiency (Martial Weapons, Heavy Armor)", 1));
                BuildFeatures.Add(new Feature("War Domain", "War Priest", 1));
                BuildFeatures.Add(new Feature("War Domain", "Channel Divinity: Guided Strike", 2));
                BuildFeatures.Add(new Feature("War Domain", "Channel Divinity: War God's Blessing", 6));
                BuildFeatures.Add(new Feature("War Domain", "Divine Strike", 8));
                BuildFeatures.Add(new Feature("War Domain", "Avatar of Battle", 17));
                //Druid
                BuildFeatures.Add(new Feature("Druid", "Spellcasting (Druid)", 1));
                BuildFeatures.Add(new Feature("Druid", "Druidic", 1));
                BuildFeatures.Add(new Feature("Druid", "Wild Shape", 2));
                BuildFeatures.Add(new Feature("Druid", "Wild Shape Improvement (Level 4)", 4));
                BuildFeatures.Add(new Feature("Druid", "Wild Shape Improvement(Level 8)", 8));
                BuildFeatures.Add(new Feature("Druid", "Timeless Body", 18));
                BuildFeatures.Add(new Feature("Druid", "Beast Spells", 18));
                BuildFeatures.Add(new Feature("Druid", "Archdruid", 20));
                //Circle of the Land - *If we can do a Linq Query for Contains on Circle of the Land, we can add all of these, I think. Then, when adding spells, we can do a similar function for the Land keywords (e.g. Arctic, Grassland, etc.)
                //Since all Druids are circle of the land under SRD, setting all of them to Druid currently makes more sense.
                BuildFeatures.Add(new Feature("Druid", "Natural Recovery", 2));
                BuildFeatures.Add(new Feature("Druid", "Land's Stride", 6));
                BuildFeatures.Add(new Feature("Druid", "Nature's Ward", 10));
                BuildFeatures.Add(new Feature("Druid", "Nature's Sanctuary", 14));
                //Circle of the Moon
                BuildFeatures.Add(new Feature("Circle of the Moon", "Combat Wild Shape", 2));
                BuildFeatures.Add(new Feature("Circle of the Moon", "Circle Forms", 2));
                BuildFeatures.Add(new Feature("Circle of the Moon", "Primal Strike", 6));
                BuildFeatures.Add(new Feature("Circle of the Moon", "Elemental Wild Shape", 10));
                BuildFeatures.Add(new Feature("Circle of the Moon", "Thousand Forms", 14));
                //Fighter
                BuildFeatures.Add(new Feature("Fighter", "Fighting Style", 1));
                // need to implement randomization for Fighting Style choice
                BuildFeatures.Add(new Feature("Fighter", "Second Wind", 1));
                BuildFeatures.Add(new Feature("Fighter", "Action Surge", 2));
                BuildFeatures.Add(new Feature("Fighter", "Extra Attack", 5));
                BuildFeatures.Add(new Feature("Fighter", "Indomitable (1 Use)", 9));
                BuildFeatures.Add(new Feature("Fighter", "Extra Attack (2)", 11));
                BuildFeatures.Add(new Feature("Fighter", "Indomitable (2 Use)", 13));
                BuildFeatures.Add(new Feature("Fighter", "Action Surge (2 Uses)", 17));
                BuildFeatures.Add(new Feature("Fighter", "Indomitable (3 Uses)", 17));
                BuildFeatures.Add(new Feature("Fighter", "Extra Attack (3)", 20));
                //Champion
                BuildFeatures.Add(new Feature("Champion", "Improved Critical", 3));
                BuildFeatures.Add(new Feature("Champion", "Remarkable Athlete", 7));
                BuildFeatures.Add(new Feature("Champion", "Additional Fighting Style", 10));
                BuildFeatures.Add(new Feature("Champion", "Superior Critical Critical", 15));
                BuildFeatures.Add(new Feature("Champion", "Survivor", 18));
                //Battle Master
                BuildFeatures.Add(new Feature("Battle Master", "Combat Superiority", 3));
                BuildFeatures.Add(new Feature("Battle Master", "Combat Student of War", 3));
                BuildFeatures.Add(new Feature("Battle Master", "Student of War", 7));
                BuildFeatures.Add(new Feature("Battle Master", "Improved Combat Superiority", 10));
                BuildFeatures.Add(new Feature("Battle Master", "Relentless", 15));
                //Eldritch Knight
                BuildFeatures.Add(new Feature("Eldritch Knight", "Spellcasting (Eldritch Knight)", 3));
                BuildFeatures.Add(new Feature("Eldritch Knight", "War Magic", 7));
                BuildFeatures.Add(new Feature("Eldritch Knight", "Eldritch Strike", 10));
                BuildFeatures.Add(new Feature("Eldritch Knight", "Arcane Charge", 15));
                BuildFeatures.Add(new Feature("Eldritch Knight", "Improved War Magic", 18));
                //Monk
                BuildFeatures.Add(new Feature("Monk", "Unarmored Defense (Monk)", 1));
                BuildFeatures.Add(new Feature("Monk", "Martial Arts", 1));
                BuildFeatures.Add(new Feature("Monk", "Ki", 2));
                BuildFeatures.Add(new Feature("Monk", "Unarmored Movement", 2));
                BuildFeatures.Add(new Feature("Monk", "Deflect Missiles", 3));
                BuildFeatures.Add(new Feature("Monk", "Slow Fall", 4));
                BuildFeatures.Add(new Feature("Monk", "Extra Attack", 5));
                BuildFeatures.Add(new Feature("Monk", "Stunning Strike", 5));
                BuildFeatures.Add(new Feature("Monk", "Ki-Empowered Strikes", 6));
                BuildFeatures.Add(new Feature("Monk", "Evasion", 7));
                BuildFeatures.Add(new Feature("Monk", "Stillness of Mind", 7));
                BuildFeatures.Add(new Feature("Monk", "Unarmored Movement Improvement", 9));
                BuildFeatures.Add(new Feature("Monk", "Purity of Body", 10));
                BuildFeatures.Add(new Feature("Monk", "Tongue of the Sun and Moon", 1));
                BuildFeatures.Add(new Feature("Monk", "Diamond Soul", 14));
                BuildFeatures.Add(new Feature("Monk", "Timeless Body", 15));
                BuildFeatures.Add(new Feature("Monk", "Empty Body", 18));
                BuildFeatures.Add(new Feature("Monk", "Perfect Self", 20));
                //Way of the Open Hand
                BuildFeatures.Add(new Feature("Way of the Open Hand", "Open Hand Technique", 3));
                BuildFeatures.Add(new Feature("Way of the Open Hand", "Wholeness of Body", 6));
                BuildFeatures.Add(new Feature("Way of the Open Hand", "Tranquility", 11));
                BuildFeatures.Add(new Feature("Way of the Open Hand", "Quivering Palm", 17));
                //Way of the Four Elements
                BuildFeatures.Add(new Feature("Way of the Four Elements", "Disciple of the Elements", 3));
                //Way of Shadow
                BuildFeatures.Add(new Feature("Way of Shadow", "Shadow Arts", 3));
                BuildFeatures.Add(new Feature("Way of Shadow", "Shadow Step", 6));
                BuildFeatures.Add(new Feature("Way of Shadow", "Cloak of Shadows", 11));
                BuildFeatures.Add(new Feature("Way of Shadow", "Opportunist", 17));
                //Paladin
                BuildFeatures.Add(new Feature("Paladin", "Divine Sense", 1));
                BuildFeatures.Add(new Feature("Paladin", "Lay on Hands", 1));
                BuildFeatures.Add(new Feature("Paladin", "Fighting Style", 2));
                BuildFeatures.Add(new Feature("Paladin", "Spellcasting (Paladin)", 2));
                BuildFeatures.Add(new Feature("Paladin", "Divine Smite", 2));
                BuildFeatures.Add(new Feature("Paladin", "Divine Smite", 5));
                BuildFeatures.Add(new Feature("Paladin", "Aura of Courage", 10));
                BuildFeatures.Add(new Feature("Paladin", "Improved Divine Smite", 11));
                BuildFeatures.Add(new Feature("Paladin", "Cleansing Touch", 14));
                BuildFeatures.Add(new Feature("Paladin", "Aura Improvements", 18));
                //Oath of Devotion
                BuildFeatures.Add(new Feature("Oath of Devotion", "Channel Divinity: Sacred Weapon/Turn the Unholy", 3));
                BuildFeatures.Add(new Feature("Oath of Devotion", "Aura of Devotion", 7));
                BuildFeatures.Add(new Feature("Oath of Devotion", "Purity of Spirit", 15));
                BuildFeatures.Add(new Feature("Oath of Devotion", "Holy Nimbus", 20));
                //Oath of Vengeance
                BuildFeatures.Add(new Feature("Oath of Vengeance", "Channel Divinity: Abjure Enemy/Vow of Enmity", 3));
                BuildFeatures.Add(new Feature("Oath of Vengeance", "Relentless Avenger", 7));
                BuildFeatures.Add(new Feature("Oath of Vengeance", "Soul of Vengeance", 15));
                BuildFeatures.Add(new Feature("Oath of Vengeance", "Avenging Angel", 20));
                //Oath of the Ancients
                BuildFeatures.Add(new Feature("Oath of the Ancients", "Channel Divinity: Nature's Wrath/Turn the Faithless", 3));
                BuildFeatures.Add(new Feature("Oath of the Ancients", "Aura of Warding", 7));
                BuildFeatures.Add(new Feature("Oath of the Ancients", "Undying Sentinel", 15));
                BuildFeatures.Add(new Feature("Oath of the Ancients", "Elder Champion", 20));
                //Ranger
                BuildFeatures.Add(new Feature("Ranger", "Favored Enemy", 1));
                //Need to expand on this, as it covers a number of possible options
                BuildFeatures.Add(new Feature("Ranger", "Natural Explorer", 1));
                BuildFeatures.Add(new Feature("Ranger", "Fighting Style", 2));
                //Reqs expansion, similar to other Fighting Style options
                BuildFeatures.Add(new Feature("Ranger", "Spellcasting (Ranger)", 2));
                BuildFeatures.Add(new Feature("Ranger", "Primeval Awareness", 3));
                BuildFeatures.Add(new Feature("Ranger", "Extra Attack", 5));
                BuildFeatures.Add(new Feature("Ranger", "Favored Enemy and Natural Explorer Improvements", 6));
                BuildFeatures.Add(new Feature("Ranger", "Natural Explorer Improvement", 10));
                BuildFeatures.Add(new Feature("Ranger", "Hide in Plain Sight", 10));
                BuildFeatures.Add(new Feature("Ranger", "Favored Enemy Improvement (Level 14)", 14));
                BuildFeatures.Add(new Feature("Ranger", "Vanish", 14));
                BuildFeatures.Add(new Feature("Ranger", "Feral Senses", 18));
                BuildFeatures.Add(new Feature("Ranger", "Foe Slayer", 20));
                //Hunter
                BuildFeatures.Add(new Feature("Hunter", "Hunter's Prey", 3)); //*Choice
                BuildFeatures.Add(new Feature("Hunter", "Defensive Tactics", 7)); //*Choice
                BuildFeatures.Add(new Feature("Hunter", "Multiattack", 11)); //*Choice\
                BuildFeatures.Add(new Feature("Hunter", "Superior Hunter's Defense", 15)); //*Choice
                //Beast Master
                BuildFeatures.Add(new Feature("Beast Master", "Ranger's Companion", 3));
                BuildFeatures.Add(new Feature("Beast Master", "Exceptional Training", 7));
                BuildFeatures.Add(new Feature("Beast Master", "Bestial Fury", 11));
                BuildFeatures.Add(new Feature("Beast Master", "Share Spells", 15));
                //Rogue
                BuildFeatures.Add(new Feature("Rogue", "Expertise", 1));
                BuildFeatures.Add(new Feature("Rogue", "Sneak Attack", 1));
                BuildFeatures.Add(new Feature("Rogue", "Thieves Cant", 1));
                BuildFeatures.Add(new Feature("Rogue", "Cunning Action", 2));
                BuildFeatures.Add(new Feature("Rogue", "Uncanny Dodge", 5));
                BuildFeatures.Add(new Feature("Rogue", "Expertise", 6));
                BuildFeatures.Add(new Feature("Rogue", "Evasion", 7));
                BuildFeatures.Add(new Feature("Rogue", "Reliable Talent", 11));
                BuildFeatures.Add(new Feature("Rogue", "Blindsense", 14));
                BuildFeatures.Add(new Feature("Rogue", "Slippery Mind", 15));
                BuildFeatures.Add(new Feature("Rogue", "Elusive", 18));
                BuildFeatures.Add(new Feature("Rogue", "Stroke of Luck", 20));
                //Thief
                BuildFeatures.Add(new Feature("Thief", "Fast Hands", 3));
                BuildFeatures.Add(new Feature("Thief", "Second-Story Work", 3));
                BuildFeatures.Add(new Feature("Thief", "Supreme Sneak", 9));
                BuildFeatures.Add(new Feature("Thief", "Use Magic Device", 13));
                BuildFeatures.Add(new Feature("Thief", "Thief's Reflexes", 17));
                //Assassin
                BuildFeatures.Add(new Feature("Assassin", "Bonus Proficiencies (Disguise Kit, Poisoner's Kit)", 3));
                BuildFeatures.Add(new Feature("Assassin", "Assassinate", 3));
                BuildFeatures.Add(new Feature("Assassin", "Infiltration Expertise", 9));
                BuildFeatures.Add(new Feature("Assassin", "Imposter", 13));
                BuildFeatures.Add(new Feature("Assassin", "Death Strike", 17));
                //Arcane Trickster
                BuildFeatures.Add(new Feature("Arcane Trickster", "Spellcasting (Arcane Trickster)", 3));
                BuildFeatures.Add(new Feature("Arcane Trickster", "Mage Hand Legerdemain", 3));
                BuildFeatures.Add(new Feature("Arcane Trickster", "Magical Ambush", 9));
                BuildFeatures.Add(new Feature("Arcane Trickster", "Versatile Trickster", 13));
                BuildFeatures.Add(new Feature("Arcane Trickster", "Spell Thief", 17));
                //Sorcerer
                BuildFeatures.Add(new Feature("Sorcerer", "Spellcasting (Sorcerer)", 1));
                BuildFeatures.Add(new Feature("Sorcerer", "Font of Magic", 2));
                BuildFeatures.Add(new Feature("Sorcerer", "Metamagic (Level 3)", 3)); //Have to generate options
                BuildFeatures.Add(new Feature("Sorcerer", "Metamagic (Level 10)", 10));
                BuildFeatures.Add(new Feature("Sorcerer", "Metamagic (Level 17)", 17));
                BuildFeatures.Add(new Feature("Sorcerer", "Sorcerous Restoration", 20));
                //Draconic Bloodline
                BuildFeatures.Add(new Feature("Draconic Bloodline", "Dragon Ancestor", 1));//*Choice
                BuildFeatures.Add(new Feature("Draconic Bloodline", "Draconic Resilience", 1));
                BuildFeatures.Add(new Feature("Draconic Bloodline", "Elemental Affinity", 6)); //Affected by Dragon Ancestor Choice
                BuildFeatures.Add(new Feature("Draconic Bloodline", "Dragon Wings", 14));
                BuildFeatures.Add(new Feature("Draconic Bloodline", "Draconic Presence", 18));
                //Wild Magic
                BuildFeatures.Add(new Feature("Wild Magic", "Wild Magic Surge", 1));
                BuildFeatures.Add(new Feature("Wild Magic", "Tides of Chaos", 1));
                BuildFeatures.Add(new Feature("Wild Magic", "Bend Luck", 6));
                BuildFeatures.Add(new Feature("Wild Magic", "Controlled Chaos", 14));
                BuildFeatures.Add(new Feature("Wild Magic", "Spell Bombardment", 18));
                //Warlock
                BuildFeatures.Add(new Feature("Warlock", "Pact Magic", 1));
                BuildFeatures.Add(new Feature("Warlock", "Eldritch Invocations", 2)); //Need to expand based on Invocations
                BuildFeatures.Add(new Feature("Warlock", "Pact Boon", 3)); // Need to expand on this to generate options, probably list each pact boon under this and when Pact boon is recieved, add one of the options to the list
                BuildFeatures.Add(new Feature("Warlock", "Mystic Arcanum (Level 6)", 11));
                BuildFeatures.Add(new Feature("Warlock", "Mystic Arcanum (Level 7)", 13));
                BuildFeatures.Add(new Feature("Warlock", "Mystic Arcanum (Level 8)", 15));
                BuildFeatures.Add(new Feature("Warlock", "Mystic Arcanum (Level 9)", 17));
                BuildFeatures.Add(new Feature("Warlock", "Eldritch Master", 20));
                //The Archfey
                BuildFeatures.Add(new Feature("The Archfey", "Fey Presence", 1));
                BuildFeatures.Add(new Feature("The Archfey", "Misty Escape", 6));
                BuildFeatures.Add(new Feature("The Archfey", "Beguiling Defenses", 10));
                BuildFeatures.Add(new Feature("The Archfey", "Dark Delirium", 14));
                //The Great Old One
                BuildFeatures.Add(new Feature("The Great Old One", "Awakened Mind", 1));
                BuildFeatures.Add(new Feature("The Great Old One", "Entropic Ward", 6));
                BuildFeatures.Add(new Feature("The Great Old One", "Thought Shield", 10));
                BuildFeatures.Add(new Feature("The Great Old One", "Create Thrall", 14));
                //The Fiend
                BuildFeatures.Add(new Feature("The Fiend", "Dark One's Blessing", 1));
                BuildFeatures.Add(new Feature("The Fiend", "Dark One's Own Luck", 6));
                BuildFeatures.Add(new Feature("The Fiend", "Fiendish Resilience", 10));
                BuildFeatures.Add(new Feature("The Fiend", "Hurl Through Hell", 14));
                //Wizard
                BuildFeatures.Add(new Feature("Wizard", "Spellcasting (Wizard)", 1));
                BuildFeatures.Add(new Feature("Wizard", "Arcane Recovery", 1));
                BuildFeatures.Add(new Feature("Wizard", "Spell Mastery", 18));
                BuildFeatures.Add(new Feature("Wizard", "Signature Spell", 20));
                //School of Abjuration
                BuildFeatures.Add(new Feature("School of Abjuration", "Abjuration Savant", 2));
                BuildFeatures.Add(new Feature("School of Abjuration", "Arcane Ward", 2));
                BuildFeatures.Add(new Feature("School of Abjuration", "Projected Ward", 6));
                BuildFeatures.Add(new Feature("School of Abjuration", "Improved Abjuration", 10));
                BuildFeatures.Add(new Feature("School of Abjuration", "Spell Resistance", 14));
                //School of Conjuration
                BuildFeatures.Add(new Feature("School of Conjuration", "Conjuration Savant", 2));
                BuildFeatures.Add(new Feature("School of Conjuration", "Minor Conjuration", 2));
                BuildFeatures.Add(new Feature("School of Conjuration", "Benign Transposition", 6));
                BuildFeatures.Add(new Feature("School of Conjuration", "Focused Conjuration", 10));
                BuildFeatures.Add(new Feature("School of Conjuration", "Durable Summons", 14));
                //School of Divination
                BuildFeatures.Add(new Feature("School of Divination", "Divination Savant", 2));
                BuildFeatures.Add(new Feature("School of Divination", "Portent", 2));
                BuildFeatures.Add(new Feature("School of Divination", "Expert Divination", 6));
                BuildFeatures.Add(new Feature("School of Divination", "The Third Eye", 10));
                BuildFeatures.Add(new Feature("School of Divination", "Greater Portent", 14));
                //School of Enchantment
                BuildFeatures.Add(new Feature("School of Enchantment", "Enchantment Savant", 2));
                BuildFeatures.Add(new Feature("School of Enchantment", "Hypnotic Gaze", 2));
                BuildFeatures.Add(new Feature("School of Enchantment", "Instinctive Charm", 6));
                BuildFeatures.Add(new Feature("School of Enchantment", "Split Enchantment", 10));
                BuildFeatures.Add(new Feature("School of Enchantment", "Alter Memories", 14));
                //School of Evocation
                BuildFeatures.Add(new Feature("School of Evocation", "Evocation Savant", 2));
                BuildFeatures.Add(new Feature("School of Evocation", "Sculpt Spells", 2));
                BuildFeatures.Add(new Feature("School of Evocation", "Potent Cantrip", 6));
                BuildFeatures.Add(new Feature("School of Evocation", "Empowered Evocation", 10));
                BuildFeatures.Add(new Feature("School of Evocation", "Overchannel", 14));
                //School of Illusion
                BuildFeatures.Add(new Feature("School of Illusion", "Illusion Savant", 2));
                BuildFeatures.Add(new Feature("School of Illusion", "Improved Minor Illusion", 2));
                BuildFeatures.Add(new Feature("School of Illusion", "Malleable Illusion", 6));
                BuildFeatures.Add(new Feature("School of Illusion", "Illusory Self", 10));
                BuildFeatures.Add(new Feature("School of Illusion", "Illusory Reality", 14));
                //School of Necromancy
                BuildFeatures.Add(new Feature("School of Necromancy", "Necromancy Savant", 2));
                BuildFeatures.Add(new Feature("School of Necromancy", "Grim Harvest", 2));
                BuildFeatures.Add(new Feature("School of Necromancy", "Undead Thralls", 6));
                BuildFeatures.Add(new Feature("School of Necromancy", "Inured to Death", 10));
                BuildFeatures.Add(new Feature("School of Necromancy", "Command Undead", 14));
                //School of Transmutation
                BuildFeatures.Add(new Feature("School of Transmutation", "Transmutation Savant", 2));
                BuildFeatures.Add(new Feature("School of Transmutation", "Minor Alchemy", 2));
                BuildFeatures.Add(new Feature("School of Transmutation", "Transmuter's Stone", 6));
                BuildFeatures.Add(new Feature("School of Transmutation", "Shapechanger", 10));
                BuildFeatures.Add(new Feature("School of Transmutation", "Master Transmuter", 14));
    
            return BuildFeatures;
        }
    }
}