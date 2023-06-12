

using dotnet_rpg.models;

namespace dotnet_rpg.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
            
        }

        public DbSet<Character> Characters => Set<Character>();
        public DbSet<User> Users=>Set<User>();
        public DbSet<Weapon> Weapons=>Set<Weapon>();
        public DbSet<Skill> Skills=>Set<Skill>();
        public DbSet<CharacterSkill> CharacterSkills=>Set<CharacterSkill>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterSkill>()
                .HasKey(cs=>new {cs.CharacterId,cs.SKillId});
        }
    }
}