using Microsoft.EntityFrameworkCore;
using ProjetoBanco.Data;
using ProjetoBanco.Models;

namespace ProjetoBanco.Services
{
    public class ContaService
    {
        private readonly BancoContext _db;
        public ContaService(BancoContext db) { _db = db; }

        public async Task<Conta?> ObterConta(int id) =>
            await _db.Contas.Include(c => c.Pessoa).FirstOrDefaultAsync(c => c.IdConta == id);

        public async Task<Conta> CriarConta(int idPessoa, decimal limiteSaqueDiario, int tipoConta, decimal saldoInicial)
        {
            var pessoa = await _db.Pessoas.FindAsync(idPessoa) ?? throw new Exception("Pessoa não encontrada");
            var conta = new Conta
            {
                IdPessoa = idPessoa,
                LimiteSaqueDiario = Math.Max(0, limiteSaqueDiario),
                TipoConta = tipoConta,
                Saldo = Math.Max(0, saldoInicial),
                FlagAtivo = true,
                DataCriacao = DateTime.UtcNow
            };
            _db.Contas.Add(conta);
            if (saldoInicial > 0)
                _db.Transacoes.Add(new Transacao { IdConta = conta.IdConta, Valor = saldoInicial, DataTransacao = DateTime.UtcNow });
            await _db.SaveChangesAsync();
            return conta;
        }

        public async Task<decimal> Depositar(int idConta, decimal valor)
        {
            if (valor <= 0) throw new Exception("Valor do depósito deve ser positivo");
            var conta = await _db.Contas.FindAsync(idConta) ?? throw new Exception("Conta não encontrada");
            if (!conta.FlagAtivo) throw new Exception("Conta bloqueada");
            conta.Saldo += valor;
            _db.Transacoes.Add(new Transacao { IdConta = idConta, Valor = valor, DataTransacao = DateTime.UtcNow });
            await _db.SaveChangesAsync();
            return conta.Saldo;
        }

        public async Task<decimal> Sacar(int idConta, decimal valor)
        {
            if (valor <= 0) throw new Exception("Valor do saque deve ser positivo");
            var conta = await _db.Contas.FindAsync(idConta) ?? throw new Exception("Conta não encontrada");
            if (!conta.FlagAtivo) throw new Exception("Conta bloqueada");

            var hoje = DateTime.UtcNow.Date;
            var sacadoHoje = await _db.Transacoes
                .Where(t => t.IdConta == idConta && t.DataTransacao >= hoje && t.Valor < 0)
                .SumAsync(t => -t.Valor);

            if (sacadoHoje + valor > conta.LimiteSaqueDiario)
                throw new Exception("Limite de saque diário excedido");

            if (conta.Saldo < valor) throw new Exception("Saldo insuficiente");

            conta.Saldo -= valor;
            _db.Transacoes.Add(new Transacao { IdConta = idConta, Valor = -valor, DataTransacao = DateTime.UtcNow });
            await _db.SaveChangesAsync();
            return conta.Saldo;
        }

        public async Task<bool> Bloquear(int idConta)
        {
            var conta = await _db.Contas.FindAsync(idConta) ?? throw new Exception("Conta não encontrada");
            conta.FlagAtivo = false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> ConsultarSaldo(int idConta)
        {
            var conta = await _db.Contas.FindAsync(idConta) ?? throw new Exception("Conta não encontrada");
            return conta.Saldo;
        }

        public async Task<List<Transacao>> Extrato(int idConta, DateTime? de, DateTime? ate)
        {
            var q = _db.Transacoes.Where(t => t.IdConta == idConta);
            if (de.HasValue) q = q.Where(t => t.DataTransacao >= de.Value);
            if (ate.HasValue) q = q.Where(t => t.DataTransacao <= ate.Value);
            return await q.OrderByDescending(t => t.DataTransacao).ToListAsync();
        }
    }
}
