using DeployTeste.Models;
using DeployTeste.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace DeployTeste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuariosRepository _usuariosRepository;
        public UsuariosController(UsuariosRepository usuariosRepository)
        {
            _usuariosRepository = usuariosRepository;
        }

        [HttpPost("InserirUsuario")]
        public IActionResult InserirUsuario([FromBody] Usuario cadastro)
        {
            try
            {
                _usuariosRepository.InserirUsuario(cadastro);
                return CreatedAtAction(nameof(ListarUsuarios), new { id = cadastro.Id }, cadastro);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao criar o usuário: {ex.Message}");
            }
        }

        [HttpGet("ListarUsuarios")]
        public ActionResult<List<Usuario>> ListarUsuarios()
        {
            try
            {
                var usuarios = _usuariosRepository.ListarUsuarios();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao obter usuários: {ex.Message}");
            }
        }
    }
}
