using Back_ColheitaSolidaria.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_ColheitaSolidaria.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Recebedor> Recebedores { get; set; }
        public DbSet<Solicitacao> Solicitacoes { get; set; }
        public DbSet<Doacao> Doacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doacao>()
                .HasOne(d => d.Usuario)
                .WithMany(c => c.Doacoes)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
