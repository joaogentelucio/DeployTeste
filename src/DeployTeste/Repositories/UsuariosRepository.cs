using FirebirdSql.Data.FirebirdClient;
using DeployTeste.Models;
using Microsoft.Extensions.Configuration;  // Ajuste para garantir que IConfiguration seja reconhecido
using System.Collections.Generic;

namespace DeployTeste.Repositories
{
    public class UsuariosRepository
    {
        private readonly string _connectionString;

        // Construtor que usa IConfiguration
        public UsuariosRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("FirebirdConnection");
        }

        // Método para inserir um usuário no banco de dados
        public void InserirUsuario(Usuario usuario)
        {
            try
            {
                using (var connection = new FbConnection(_connectionString))
                {
                    connection.Open();

                    var query = "INSERT INTO usuarios (nome, email, senha) VALUES (@Nome, @Email, @Senha)";
                    using (var command = new FbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nome", usuario.Nome);
                        command.Parameters.AddWithValue("@Email", usuario.Email);
                        command.Parameters.AddWithValue("@Senha", usuario.Senha);

                        // Recupera o ID gerado automaticamente
                        var newId = command.ExecuteScalar(); // Recupera o valor do ID gerado
                        usuario.Id = Convert.ToInt32(newId); // Atribui o ID ao objeto
                    }
                }
            }
            catch (Exception ex)
            {
                // Tratar exceções aqui, por exemplo, logando o erro
                throw new Exception("Erro ao inserir usuário", ex);
            }
        }

        // Método para listar todos os usuários do banco de dados
        public List<Usuario> ListarUsuarios()
        {
            var usuarios = new List<Usuario>();

            try
            {
                using (var connection = new FbConnection(_connectionString))
                {
                    connection.Open();

                    var query = "SELECT id, nome, email, senha FROM usuarios";
                    using (var command = new FbCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var usuario = new Usuario
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Nome = reader.GetString(reader.GetOrdinal("nome")),
                                    Email = reader.GetString(reader.GetOrdinal("email")),
                                    Senha = reader.GetString(reader.GetOrdinal("senha"))
                                };

                                usuarios.Add(usuario);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Tratar exceções aqui, por exemplo, logando o erro
                throw new Exception("Erro ao listar usuários", ex);
            }

            return usuarios;
        }
    }
}
