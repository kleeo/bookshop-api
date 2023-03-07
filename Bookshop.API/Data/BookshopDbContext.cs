using Microsoft.EntityFrameworkCore;
using Bookshop.Models;

namespace Bookshop.Data
{
    public class BookshopDbContext : DbContext
    {
        public BookshopDbContext (DbContextOptions<BookshopDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; } = default!;

        public virtual DbSet<Product> Products { get; set; } = default!;

        public virtual DbSet<Booking> Bookings { get; set; } = default!;

        public virtual DbSet<StoreItem> StoreItems { get; set; } = default!;
    }
}
