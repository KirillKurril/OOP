using Microsoft.EntityFrameworkCore;
using ServerDB.Models;

namespace ServerDB.DBContext
{
    public class RoomsDBContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Rooms.db");
        }
        public void SaveData()
        {
            SaveChanges();
        }
    }
}