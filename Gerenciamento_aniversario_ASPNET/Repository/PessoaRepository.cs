using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Gerenciamento_aniversario_ASPNET.Models;

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
            using (var connection = new SqlConnection(this.ConnectionString))
            {
                var sql = @" INSERT INTO Pessoa(Nome, DATANASCIMENTO, SOBRENOME)
                             VALUES (@P1, @P2, @P3)
                ";

                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = sql;
                sqlCommand.Parameters.AddWithValue("P1", pessoa.Nome);
                sqlCommand.Parameters.AddWithValue("P2", pessoa.DataDeAniversario);
                sqlCommand.Parameters.AddWithValue("P3", pessoa.SobreNome);


                sqlCommand.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Update(Pessoa pessoa)
        {
            using (var connection = new SqlConnection(this.ConnectionString))
            {
                var sql = @" UPDATE PESSOA
                             SET Nome = @P1,
                             DATANASCIMENTO = @P2,
                             SOBRENOME = @P4
                             WHERE Id = @P3
                ";

                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = sql;
                sqlCommand.Parameters.AddWithValue("P1", pessoa.Nome);
                sqlCommand.Parameters.AddWithValue("P2", pessoa.DataDeAniversario);
                sqlCommand.Parameters.AddWithValue("P3", pessoa.Id);
                sqlCommand.Parameters.AddWithValue("P4", pessoa.SobreNome);

                sqlCommand.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Delete(Pessoa pessoa)
        {
            using (var connection = new SqlConnection(this.ConnectionString))
            {
                var sql = @" DELETE FROM Pessoa
                             WHERE Id = @P1
                ";

                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = sql;
                sqlCommand.Parameters.AddWithValue("P1", pessoa.Id);

                sqlCommand.ExecuteNonQuery();

                connection.Close();
            }
        }

        public List<Pessoa> GetAll()
        {
            List<Pessoa> result = new List<Pessoa>();

            using (var connection = new SqlConnection(this.ConnectionString))
            {

                var sql = @" SELECT ID, NOME, SOBRENOME, DATANASCIMENTO FROM Pessoa";

                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = sql;

                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    Pessoa pessoa = new Pessoa()
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        Nome = reader["Nome"].ToString(),
                        SobreNome = reader["SobreNome"].ToString(),
                        DataDeAniversario = Convert.ToDateTime(reader["DATANASCIMENTO"]),
                    };
                    pessoa.DiasRestantes = pessoa.ProximoAniversario();
                    result.Add(pessoa);
                }

                connection.Close();
            }

            return result;
        }

        public List<Pessoa> ListaOrdenada()
        {

            var pessoasOrdenadas = GetAll();

            return pessoasOrdenadas.OrderBy(pessoa => pessoa.DiasRestantes).ToList();
        }

        public List<Pessoa> BuscarPorNome(string nomePessoa)
        {
            List<Pessoa> result = new List<Pessoa>();

            using (var connection = new SqlConnection(this.ConnectionString))
            {

                var sql = @" SELECT Id, NOME, SOBRENOME, DATANASCIMENTO 
                             FROM Pessoa
                             WHERE (NOME LIKE '%' + @P1 + '%' OR SOBRENOME LIKE '%' + @P1 + '%')
                ";

                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = sql;
                sqlCommand.Parameters.AddWithValue("P1", nomePessoa);
                

                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    Pessoa pessoa = new Pessoa()
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        Nome = reader["Nome"].ToString(),
                        SobreNome = reader["SobreNome"].ToString(),
                        DataDeAniversario = Convert.ToDateTime(reader["DATANASCIMENTO"]),
                    };
                    result.Add(pessoa);
                }
                connection.Close();
            }
            return result;
        }

        public Pessoa GetById(int id)
        {
            List<Pessoa> result = new List<Pessoa>();

            using (var connection = new SqlConnection(this.ConnectionString))
            {

                var sql = @" SELECT Id, NOME,SOBRENOME, DATANASCIMENTO
                             FROM Pessoa
                             WHERE Id = @P1
                ";

                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = sql;
                sqlCommand.Parameters.AddWithValue("P1", id);

                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    Pessoa pessoa = new Pessoa()
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        Nome = reader["Nome"].ToString(),
                        SobreNome = reader["SobreNome"].ToString(),
                        DataDeAniversario = DateTime.Parse(reader["DATANASCIMENTO"].ToString()),
                    };

                    result.Add(pessoa);
                }

                connection.Close();
            }

            return result.FirstOrDefault();
        }
    }
}
