using Gerenciamento_aniversario_ASPNET.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Gerenciamento_aniversario_ASPNET.Repository
{
    public class PessoaRepository
    {
        private string ConnectionString { get; set; }

        public PessoaRepository(IConfiguration configuration)
        {
            this.ConnectionString = configuration.GetConnectionString("Gerenciamento");
        }

        public void Save(Pessoa pessoa)
        {
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(this.ConnectionString)) 
            {
                string sql = "INSERT INTO PESSOA(NOME, SOBRENOME, DATANASCIMENTO) VALUES (@P1, @P2, @P3)";

                var command = connection.CreateCommand();

                command.CommandText = sql;
                command.Parameters.AddWithValue("P1", pessoa.Nome);
                command.Parameters.AddWithValue("P2", pessoa.Sobrenome);
                command.Parameters.AddWithValue("P3", pessoa.DataNascimento);

                command.CommandType = System.Data.CommandType.Text;

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            } 
        }

        public void Update(Pessoa pessoa)
        {
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(this.ConnectionString))
            {
                string sql = @"
                    UPDATE PESSOA
                    SET NOME = @P1,
                    SOBRENOME = @P2,
                    DATANASCIMENTO = @P3,
                    WHERE ID = @P4
                
                ";

                var command = connection.CreateCommand();

                command.CommandText = sql;
                command.Parameters.AddWithValue("P1", pessoa.Nome);
                command.Parameters.AddWithValue("P2", pessoa.Sobrenome);
                command.Parameters.AddWithValue("P3", pessoa.DataNascimento);
                command.Parameters.AddWithValue("P4", pessoa.Id);

                command.CommandType = System.Data.CommandType.Text;

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Delete(Pessoa pessoa)
        {
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(this.ConnectionString))
            {
                string sql = @"
                    DELETE FROM PESSOA
                    WHERE ID = @P1
                
                ";

                var command = connection.CreateCommand();

                command.CommandText = sql;
                command.Parameters.AddWithValue("P1", pessoa.Id);

                command.CommandType = System.Data.CommandType.Text;

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public List<Pessoa> GetAll()
        {
            List<Pessoa> result = new List<Pessoa>();
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(this.ConnectionString))
            {
                string sql = @"
                    SELECT ID, NOME, SOBRENOME, DATANASCIMENTO
                    FROM PESSOA
                
                ";

                var command = connection.CreateCommand();

                command.CommandText = sql;

                command.CommandType = System.Data.CommandType.Text;

                connection.Open();
                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    result.Add(new Pessoa()
                    {
                        Id = dr.GetInt32("ID"),
                        Nome = dr.GetString("NOME"),
                        Sobrenome = dr.GetString("SOBRENOME"),
                        DataNascimento = dr.GetDateTime("DATANASCIMENTO")
                    });
                }
                connection.Close();
            }

            return result;
        }

        public Pessoa GetPessoaById(int id)
        {
            Pessoa result = null;
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(this.ConnectionString))
            {
                string sql = @"
                    SELECT ID, NOME, SOBRENOME, DATANASCIMENTO
                    FROM PESSOA
                    WHERE ID = @P1
                
                ";

                var command = connection.CreateCommand();

                command.CommandText = sql;
                command.Parameters.AddWithValue("P1", id);
                command.CommandType = System.Data.CommandType.Text;

                connection.Open();
                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    result = new Pessoa()
                    {
                        Id = dr.GetInt32("ID"),
                        Nome = dr.GetString("NOME"),
                        Sobrenome = dr.GetString("SOBRENOME"),
                        DataNascimento = dr.GetDateTime("DATANASCIMENTO")
                    };
                }
                connection.Close();
            }

            return result;
        }

        public List<Pessoa> Search(string nome, string sobreNome)
        {
            List<Pessoa> result = null;
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(this.ConnectionString))
            {
                string sql = @"
                    SELECT ID, NOME, SOBRENOME, DATANASCIMENTO
                    FROM PESSOA
                    WHERE (NOME LIKE '%@P1%' OR SOBRENOME LIKE '%@P2%')
                    ORDER BY DATANASCIMENTO ASC
                ";

                var command = connection.CreateCommand();

                command.CommandText = sql;
                command.Parameters.AddWithValue("P1", nome);
                command.Parameters.AddWithValue("P2", sobreNome);
                command.CommandType = System.Data.CommandType.Text;

                connection.Open();
                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    result.Add(new Pessoa()
                    {
                        Id = dr.GetInt32("ID"),
                        Nome = dr.GetString("NOME"),
                        Sobrenome = dr.GetString("SOBRENOME"),
                        DataNascimento = dr.GetDateTime("DATANASCIMENTO")
                    });
                }
                connection.Close();
            }

            return result;
        }
    }
}
