using System.Security.Claims;
using ConexaoSolidaria.Application.Common;
using ConexaoSolidaria.Application.Doacoes;
using ConexaoSolidaria.Contracts;
using ConexaoSolidaria.Domain.Common;
using ConexaoSolidaria.Domain.Entities;
using ConexaoSolidaria.Infrastructure.Persistence;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConexaoSolidaria.Api.Controllers;

[ApiController]
[Route("api/v1/doacoes")]
[Authorize(Roles = "Doador")]
public sealed class DoacoesController(
    ApplicationDbContext context,
    IPublishEndpoint publishEndpoint) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<RegistrarDoacaoResponse>(StatusCodes.Status202Accepted)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RegistrarDoacaoResponse>> RegistrarAsync(
        [FromBody] RegistrarDoacaoRequest request,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(userIdClaim, out var doadorId))
        {
            return Unauthorized(new ApiErrorResponse(
                "TOKEN_INVALIDO",
                "Token inválido."));
        }

        var campanha = await context.Campanhas
            .FirstOrDefaultAsync(x => x.Id == request.IdCampanha, cancellationToken);

        if (campanha is null)
        {
            return NotFound(new ApiErrorResponse(
                "CAMPANHA_NAO_ENCONTRADA",
                "Campanha não encontrada."));
        }

        try
        {
            campanha.ValidarPodeReceberDoacao();

            var doacao = new Doacao(
                request.IdCampanha,
                doadorId,
                request.ValorDoacao);

            context.Doacoes.Add(doacao);
            await context.SaveChangesAsync(cancellationToken);

            var evento = new DoacaoRecebidaEvent(
                Guid.NewGuid(),
                doacao.Id,
                doacao.CampanhaId,
                doacao.DoadorId,
                doacao.Valor,
                doacao.CriadaEm);

            await publishEndpoint.Publish(evento, cancellationToken);

            var response = new RegistrarDoacaoResponse(
                doacao.Id,
                "RecebidaParaProcessamento",
                "Doação recebida e enviada para processamento assíncrono.");

            return Accepted(response);
        }
        catch (DomainException exception)
        {
            return BadRequest(new ApiErrorResponse(
                "REGRA_DOACAO_INVALIDA",
                exception.Message));
        }
    }
}
