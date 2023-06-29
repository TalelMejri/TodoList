using Microsoft.EntityFrameworkCore;

namespace TodoListProject.Models
{
    public class DbContextClasse : DbContext
    {
        public DbSet<Todos> Todos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"data source=Desktop-d1jfgmm\sqlexpress; initial catalog=TodosDb; integrated security=SSPI;TrustServerCertificate=True");
        }

    }
}
