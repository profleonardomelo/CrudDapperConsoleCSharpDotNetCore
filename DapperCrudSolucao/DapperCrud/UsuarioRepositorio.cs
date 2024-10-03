using System.Data;
using MySql.Data.MySqlClient;
using Dapper;

namespace DapperCrud
{
    internal class UsuarioRepositorio
    {
        private readonly string _connectionString;

        internal UsuarioRepositorio(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection Conexao => new MySqlConnection(_connectionString);

        internal void Adicionar(Usuario usuario)
        {
            using (IDbConnection bd = Conexao)
            {
                string sql = "INSERT INTO Usuario (Nome, Idade) VALUES (@Nome, @Idade)";
                bd.Execute(sql, new { usuario.Nome, usuario.Idade });
            }
        }

        internal List<Usuario> ObterTodos()
        {
            using (IDbConnection bd = Conexao)
            {
                string sql = "SELECT Id, Nome, Idade FROM Usuario";
                return bd.Query<Usuario>(sql).ToList();
            }
        }

        internal Usuario? ObterPorId(int id)
        {
            using (IDbConnection bd = Conexao)
            {
                string sql = "SELECT Id, Nome, Idade FROM Usuario WHERE Id = @Id";
                return bd.QueryFirstOrDefault<Usuario>(sql, new { Id = id });
            }
        }

        internal List<Usuario> ObterPorFiltro(string? nome = null, int? idade = null)
        {
            using (IDbConnection bd = Conexao)
            {
                string sql = "SELECT Id, Nome, Idade FROM Usuario WHERE 1=1" + " ";

                if (!string.IsNullOrEmpty(nome))
                {
                    sql += "AND Nome LIKE @Nome";
                }

                if (idade.HasValue)
                {
                    sql += "AND Idade = @Idade";
                }

                return bd.Query<Usuario>(sql, new { Nome = $"%{nome}%", Idade = idade }).ToList();
            }
        }

        internal void Atualizar(Usuario usuario)
        {
            using (IDbConnection bd = Conexao)
            {
                string sql = "UPDATE Usuario SET Nome = @Nome, Idade = @Idade WHERE Id = @Id";
                bd.Execute(sql, new { usuario.Nome, usuario.Idade, usuario.Id });
            }
        }

        public void Remover(int id)
        {
            using (IDbConnection bd = Conexao)
            {
                string sql = "DELETE FROM Usuario WHERE Id = @Id";
                bd.Execute(sql, new { Id = id });
            }
        }
    }
}
