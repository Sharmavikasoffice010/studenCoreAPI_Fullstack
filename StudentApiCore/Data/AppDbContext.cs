using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using StudentApiCore.Models;

namespace StudentApiCore.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

        public DbSet<Student> Students { get; set; }
    }
}
