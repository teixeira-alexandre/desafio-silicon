using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjetoBanco.Models
{
    public class Pessoa
    {
        public int IdPessoa { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }

        // Definicao de Contas
        [JsonIgnore]
        public List<Conta> Contas { get; set; } = new();
    }
}
