using Microsoft.EntityFrameworkCore;

namespace CharacterGenerator.Models
{
    public class MyContext : DbContext
    {
        public MyContext (DbContextOptions options) : base(options) {}
        public DbSet<Character> Characters { get; set; }
    }
}