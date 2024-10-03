using MySql.Data.MySqlClient;
using Dapper;

namespace DapperCrud
{
    internal static class ConfiguracaoBancoDeDados
    {
        internal static void CriarBancoDeDados(string connectionString)
        {
            using (var conexao = new MySqlConnection(connectionString))
            {
                conexao.Open();

                string scriptBanco = @"
                    CREATE DATABASE IF NOT EXISTS TesteDapper;
                    USE TesteDapper;
                    CREATE TABLE IF NOT EXISTS Usuario (
                        Id INT AUTO_INCREMENT,
                        Nome VARCHAR(100) NOT NULL,
                        Idade INT NOT NULL,
                        PRIMARY KEY (Id)
                    );";

                conexao.Execute(scriptBanco);
            }
        }
    }
}
