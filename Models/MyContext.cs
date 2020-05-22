using Microsoft.EntityFrameworkCore;

namespace CharacterGenerator.Models
{
    public class MyContext : DbContext
    {
        public MyContext (DbContextOptions options) : base(options) {}
        public DbSet<Character> Characters { get; set; }
        public DbSet<NewCharacter> NewCharacter { get; set; }
        public DbSet<PlayerStat> PlayerStats { get; set; }
        public DbSet<PlayerRace> PlayerRaces { get; set; }
        public DbSet<PlayerBG> PlayerBGs { get; set; }
        public DbSet <PlayerClass> PlayerClasses { get; set; }
        public DbSet <User> Users { get; set; }
        public DbSet <Feature> Features { get; set;}
        public DbSet <FeatureAssoc> Feature_Associations {get; set;}
        public DbSet <Spell> Spells {get; set;}
        public DbSet <SpellAssoc> Spell_Associations {get; set;}
    }
}