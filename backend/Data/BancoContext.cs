using Microsoft.EntityFrameworkCore;
using ProjetoBanco.Models;

namespace ProjetoBanco.Data
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options) {}

        public DbSet<Pessoa> Pessoas => Set<Pessoa>();
        public DbSet<Conta> Contas => Set<Conta>();
        public DbSet<Transacao> Transacoes => Set<Transacao>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pessoa>(e =>
            {
                e.ToTable("Pessoas");
                e.HasKey(x => x.IdPessoa);
                e.Property(x => x.Nome).IsRequired().HasMaxLength(120);
                e.Property(x => x.Cpf).IsRequired().HasMaxLength(14);
            });

            modelBuilder.Entity<Conta>(e =>
            {
                e.ToTable("Contas");
                e.HasKey(x => x.IdConta);
                e.Property(x => x.Saldo).HasPrecision(18,2);
                e.Property(x => x.LimiteSaqueDiario).HasPrecision(18,2);
                e.Property(x => x.FlagAtivo).HasDefaultValue(true);
                e.Property(x => x.DataCriacao).HasDefaultValueSql("CURRENT_TIMESTAMP");
                e.HasOne(x => x.Pessoa)
                    .WithMany(p => p.Contas)
                    .HasForeignKey(x => x.IdPessoa);
            });

            modelBuilder.Entity<Transacao>(e =>
            {
                e.ToTable("Transacoes");
                e.HasKey(x => x.IdTransacao);
                e.Property(x => x.Valor).HasPrecision(18,2);
                e.Property(x => x.DataTransacao).HasDefaultValueSql("CURRENT_TIMESTAMP");
                e.HasOne(x => x.Conta)
                    .WithMany(c => c.Transacoes)
                    .HasForeignKey(x => x.IdConta);
            });
        }
    }
}
