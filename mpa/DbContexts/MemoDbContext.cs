using Microsoft.EntityFrameworkCore;
using mpa.Models;

public class MemoDbContext : DbContext
{
  public DbSet<Memo> Memo { get; set; }
  public MemoDbContext(DbContextOptions<MemoDbContext> options)
      : base(options)
  {
  }
}