namespace ProjetoBanco.Dtos
{
    public class CriarContaDto
    {
        public int IdPessoa { get; set; }
        public decimal LimiteSaqueDiario { get; set; }
        public int TipoConta { get; set; }
        public decimal SaldoInicial { get; set; } = 0;
    }
}
