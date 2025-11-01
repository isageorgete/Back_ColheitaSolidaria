using Microsoft.EntityFrameworkCore;
using Back_ColheitaSolidaria.Models;

namespace Back_ColheitaSolidaria.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Recebedor> Recebedores { get; set; }
        public DbSet<Solicitacao> Solicitacoes { get; set; }
    }
}

