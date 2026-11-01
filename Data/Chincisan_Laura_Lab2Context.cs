using Microsoft.EntityFrameworkCore;

public class Chincisan_Laura_Lab2Context(DbContextOptions<Chincisan_Laura_Lab2Context> options) : DbContext(options)
{
    public DbSet<Chincisan_Laura_Lab2.Models.Book> Book { get; set; } = default!;

    public DbSet<Chincisan_Laura_Lab2.Models.Publisher> Publisher { get; set; } = default!;

    public DbSet<Chincisan_Laura_Lab2.Models.Authors> Authors { get; set; } = default!;
}