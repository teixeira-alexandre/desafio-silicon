using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoBanco.Models
{
    public class Conta
    {
        [Key]
        public int IdConta { get; set; }

        [ForeignKey("Pessoa")]
        public int IdPessoa { get; set; }
        public Pessoa? Pessoa { get; set; }

        public decimal Saldo { get; set; }
        public decimal LimiteSaqueDiario { get; set; }
        public bool FlagAtivo { get; set; } = true;
        public int TipoConta { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public List<Transacao> Transacoes { get; set; } = new();
    }
}
