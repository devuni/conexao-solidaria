using ConexaoSolidaria.Application.Campanhas;
using ConexaoSolidaria.Application.Common;
using ConexaoSolidaria.Domain.Common;
using ConexaoSolidaria.Domain.Entities;
using ConexaoSolidaria.Domain.Enums;
using ConexaoSolidaria.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConexaoSolidaria.Api.Controllers;

[ApiController]
[Route("api/v1/campanhas")]
[Authorize(Roles = "GestorONG")]
public sealed class CampanhasController(ApplicationDbContext context) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<CampanhaResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<CampanhaResponse>> CriarAsync(
        [FromBody] CriarCampanhaRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryParseStatus(request.Status, out var status))
            return StatusInvalido();

        try
        {
            var campanha = new Campanha(
                request.Titulo,
                request.Descricao,
                request.DataInicio,
                request.DataFim,
                request.MetaFinanceira,
                status);

            context.Campanhas.Add(campanha);
            await context.SaveChangesAsync(cancellationToken);

            var response = MapToResponse(campanha);

            return Created($"/api/v1/campanhas/{campanha.Id}", response);
        }
        catch (DomainException exception)
        {
            return BadRequest(new ApiErrorResponse(
                "REGRA_CAMPANHA_INVALIDA",
                exception.Message));
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType<CampanhaResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CampanhaResponse>> AtualizarAsync(
        Guid id,
        [FromBody] AtualizarCampanhaRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryParseStatus(request.Status, out var status))
            return StatusInvalido();

        var campanha = await context.Campanhas
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (campanha is null)
        {
            return NotFound(new ApiErrorResponse(
                "CAMPANHA_NAO_ENCONTRADA",
                "Campanha não encontrada."));
        }

        try
        {
            campanha.Atualizar(
                request.Titulo,
                request.Descricao,
                request.DataInicio,
                request.DataFim,
                request.MetaFinanceira,
                status);

            await context.SaveChangesAsync(cancellationToken);

            return Ok(MapToResponse(campanha));
        }
        catch (DomainException exception)
        {
            return BadRequest(new ApiErrorResponse(
                "REGRA_CAMPANHA_INVALIDA",
                exception.Message));
        }
    }

    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<CampanhaResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<CampanhaResponse>>> ListarAsync(
        CancellationToken cancellationToken)
    {
        var campanhas = await context.Campanhas
            .AsNoTracking()
            .OrderByDescending(x => x.CriadaEm)
            .Select(x => MapToResponse(x))
            .ToListAsync(cancellationToken);

        return Ok(campanhas);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<CampanhaResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CampanhaResponse>> ObterPorIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var campanha = await context.Campanhas
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => MapToResponse(x))
            .FirstOrDefaultAsync(cancellationToken);

        if (campanha is null)
        {
            return NotFound(new ApiErrorResponse(
                "CAMPANHA_NAO_ENCONTRADA",
                "Campanha não encontrada."));
        }

        return Ok(campanha);
    }

    private static bool TryParseStatus(string value, out StatusCampanha status)
    {
        return Enum.TryParse(value, ignoreCase: true, out status)
            && Enum.IsDefined(status);
    }

    private BadRequestObjectResult StatusInvalido()
    {
        return BadRequest(new ApiErrorResponse(
            "STATUS_CAMPANHA_INVALIDO",
            "Status inválido. Use: Ativa, Concluida ou Cancelada."));
    }

    private static CampanhaResponse MapToResponse(Campanha campanha)
    {
        return new CampanhaResponse(
            campanha.Id,
            campanha.Titulo,
            campanha.Descricao,
            campanha.DataInicio,
            campanha.DataFim,
            campanha.MetaFinanceira,
            campanha.ValorTotalArrecadado,
            campanha.Status.ToString());
    }
}
