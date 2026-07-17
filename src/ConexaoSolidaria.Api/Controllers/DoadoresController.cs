using ConexaoSolidaria.Application.Common;
using ConexaoSolidaria.Application.Doadores;
using ConexaoSolidaria.Application.Security;
using ConexaoSolidaria.Domain.Entities;
using ConexaoSolidaria.Domain.Enums;
using ConexaoSolidaria.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConexaoSolidaria.Api.Controllers;

[ApiController]
[Route("api/v1/doadores")]
public sealed class DoadoresController(
    ApplicationDbContext context,
    IPasswordHashService passwordHashService,
    ICpfValidator cpfValidator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<DoadorResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<DoadorResponse>> CadastrarAsync(
        [FromBody] CadastrarDoadorRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.NomeCompleto))
        {
            return BadRequest(new ApiErrorResponse(
                "NOME_OBRIGATORIO",
                "O nome completo é obrigatório."));
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest(new ApiErrorResponse(
                "EMAIL_OBRIGATORIO",
                "O e-mail é obrigatório."));
        }

        if (string.IsNullOrWhiteSpace(request.Senha) || request.Senha.Length < 8)
        {
            return BadRequest(new ApiErrorResponse(
                "SENHA_INVALIDA",
                "A senha deve possuir pelo menos 8 caracteres."));
        }

        if (!cpfValidator.IsValid(request.Cpf))
        {
            return BadRequest(new ApiErrorResponse(
                "CPF_INVALIDO",
                "O CPF informado é inválido."));
        }

        var email = request.Email.Trim().ToLowerInvariant();
        var cpf = cpfValidator.OnlyDigits(request.Cpf);

        var emailExists = await context.Usuarios
            .AnyAsync(x => x.Email == email, cancellationToken);

        if (emailExists)
        {
            return Conflict(new ApiErrorResponse(
                "EMAIL_JA_CADASTRADO",
                "Já existe um usuário cadastrado com esse e-mail."));
        }

        var cpfExists = await context.Usuarios
            .AnyAsync(x => x.Cpf == cpf, cancellationToken);

        if (cpfExists)
        {
            return Conflict(new ApiErrorResponse(
                "CPF_JA_CADASTRADO",
                "Já existe um usuário cadastrado com esse CPF."));
        }

        var usuario = new Usuario(
            request.NomeCompleto,
            email,
            cpf,
            passwordHashService.Hash(request.Senha),
            PerfilUsuario.Doador);

        context.Usuarios.Add(usuario);
        await context.SaveChangesAsync(cancellationToken);

        var response = new DoadorResponse(
            usuario.Id,
            usuario.NomeCompleto,
            usuario.Email,
            usuario.Cpf,
            usuario.Perfil.ToString());

        return Created($"/api/v1/doadores/{usuario.Id}", response);
    }
}
