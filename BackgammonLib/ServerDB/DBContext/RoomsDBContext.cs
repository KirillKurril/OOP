using Entities.GameServices;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using ServerDB.Models;
using System.Reflection.Metadata;

namespace ServerDB.DBContext
{
    public class RoomsDBContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<NetGame> NetGames { get; set; }
        public DbSet<Cell> Cell { get; set; }
        public DbSet<GameBoard> GameBoards { get; set; }
        public DbSet<Player> Players { get; set; }

        public RoomsDBContext(DbContextOptions options) : base(options) 
        {
            Database.EnsureCreated();
        }
/*        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                
        }*/
        public void SaveData()
        {
            SaveChanges();
        }
    }
}