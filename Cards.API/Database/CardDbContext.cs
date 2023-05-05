using Cards.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Cards.API.Database
{
    public class CardDbContext : DbContext
    {
        public CardDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Card> Cards { get; set; }
    }
}
