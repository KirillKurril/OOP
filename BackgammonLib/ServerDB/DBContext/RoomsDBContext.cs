using Microsoft.EntityFrameworkCore;
using ServerDB.Models;

namespace ServerDB.DBContext
{
    public class RoomsDBContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }

        public RoomsDBContext(DbContextOptions options) : base(options) 
        {
            Database.EnsureCreated();
        }
        public void SaveData()
        {
            SaveChanges();
        }
    }
}