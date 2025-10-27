using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjetoBanco.Models
{
    public class Transacao
    {
        [Key]
        public int IdTransacao { get; set; }

        [ForeignKey("Conta")]
        public int IdConta { get; set; }
        public Conta? Conta { get; set; }

        // Depósito > 0, Saque < 0
        public decimal Valor { get; set; }
        public DateTime DataTransacao { get; set; } = DateTime.UtcNow;
        
    }
}
