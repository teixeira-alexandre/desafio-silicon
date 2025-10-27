using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoBanco.Data;
using ProjetoBanco.Dtos;
using ProjetoBanco.Models;
using ProjetoBanco.Services;

namespace ProjetoBanco.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContasController : ControllerBase
    {
        private readonly BancoContext _db;
        private readonly ContaService _svc;

        public ContasController(BancoContext db, ContaService svc) { _db = db; _svc = svc; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Conta>>> Listar() =>
            Ok(await _db.Contas.AsNoTracking().ToListAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Conta>> Obter(int id)
        {
            var conta = await _svc.ObterConta(id);
            if (conta == null) return NotFound();
            return Ok(conta);
        }

        [HttpPost]
        public async Task<ActionResult<Conta>> Criar([FromBody] CriarContaDto dto)
        {
            try
            {
                var conta = await _svc.CriarConta(dto.IdPessoa, dto.LimiteSaqueDiario, dto.TipoConta, dto.SaldoInicial);
                return CreatedAtAction(nameof(Obter), new { id = conta.IdConta }, conta);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        [HttpPost("{id}/deposito")]
        public async Task<ActionResult> Depositar(int id, [FromBody] DepositoDto dto)
        {
            try
            {
                var saldo = await _svc.Depositar(id, dto.Valor);
                return Ok(new { saldoAtual = saldo });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        [HttpPost("{id}/saque")]
        public async Task<ActionResult> Sacar(int id, [FromBody] SaqueDto dto)
        {
            try
            {
                var saldo = await _svc.Sacar(id, dto.Valor);
                return Ok(new { saldoAtual = saldo });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        [HttpGet("{id}/saldo")]
        public async Task<ActionResult> Saldo(int id)
        {
            try
            {
                var saldo = await _svc.ConsultarSaldo(id);
                return Ok(new { saldo });
            }
            catch (Exception ex)
            {
                return NotFound(new { erro = ex.Message });
            }
        }

        [HttpPost("{id}/bloqueio")]
        public async Task<ActionResult> Bloquear(int id)
        {
            try
            {
                await _svc.Bloquear(id);
                return Ok(new { bloqueada = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        [HttpGet("{id}/extrato")]
        public async Task<ActionResult<IEnumerable<Transacao>>> Extrato(int id, [FromQuery] DateTime? de, [FromQuery] DateTime? ate)
        {
            try
            {
                var lista = await _svc.Extrato(id, de, ate);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}
