using ConexaoSolidaria.Application.Campanhas;
using ConexaoSolidaria.Domain.Enums;
using ConexaoSolidaria.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConexaoSolidaria.Api.Controllers;

[ApiController]
[Route("api/v1/transparencia")]
public sealed class TransparenciaController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet("campanhas")]
    [ProducesResponseType<IReadOnlyCollection<TransparenciaCampanhaResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<TransparenciaCampanhaResponse>>> ListarCampanhasAtivasAsync(
        CancellationToken cancellationToken)
    {
        var campanhas = await context.Campanhas
            .AsNoTracking()
            .Where(x => x.Status == StatusCampanha.Ativa)
            .OrderBy(x => x.DataFim)
            .Select(x => new TransparenciaCampanhaResponse(
                x.Id,
                x.Titulo,
                x.MetaFinanceira,
                x.ValorTotalArrecadado))
            .ToListAsync(cancellationToken);

        return Ok(campanhas);
    }
}
