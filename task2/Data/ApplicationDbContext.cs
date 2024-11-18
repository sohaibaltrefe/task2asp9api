using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using task2.Model;

namespace task2.Data
{
    public class ApplicationDbContext:DbContext
    {
       

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Product>  products { get; set; }

    }
}
