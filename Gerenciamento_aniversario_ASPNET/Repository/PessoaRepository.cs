using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Gerenciamento_aniversario_ASPNET.Models;

namespace Gerenciamento_aniversario_ASPNET.Repository
{
    public class PessoaRepository
    {
        private string ConnectionString { get; set; }

        public PessoaRepository(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("Gerenciamento");
        }
        public void Save(Pessoa pessoa)
        {
            using (var connection = new SqlConnection(this.ConnectionString))
            {
                var sql = @" INSERT INTO Pessoa(NOME, SOBRENOME, DATANASCIMENTO)
                             VALUES (@P1, @P2, @P3)
                ";

                connection.Execute(sql, new { P1 = pessoa.NomePessoa, P2 = pessoa.SobrenomePessoa, P3 = pessoa.DataDeAniversario });
            }
        }

        public void Update(Pessoa pessoa)
        {
            using (var connection = new SqlConnection(this.ConnectionString))
            {

                var sql = @" 
                             UPDATE PESSOA
                                SET NOME = @P1,
                                SOBRENOME = @P2,
                                DATANASCIMENTO = @P3
                                WHERE ID = @P4;
                ";

                connection.Execute(sql, new { P1 = pessoa.NomePessoa, P2 = pessoa.SobrenomePessoa, P3 = pessoa.DataDeAniversario, P4 = pessoa.Id });
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(this.ConnectionString))
            {

                var sql = @" DELETE FROM PESSOA
                             WHERE ID = @P1 
                ";

                connection.Execute(sql, new { P1 = id });
            }
        }

        public List<Pessoa> GetAll()
        {
            List<Pessoa> result = new List<Pessoa>();

            using (var connection = new SqlConnection(this.ConnectionString))
            {

                var sql = @" SELECT ID, NOME, SOBRENOME, DATANASCIMENTO FROM PESSOA";

                result = connection.Query<Pessoa>(sql).ToList();
            }

            return result;
        }

        public Pessoa GetById(int id)
        {
            Pessoa result = null;

            using (var connection = new SqlConnection(this.ConnectionString))
            {

                var sql = @" SELECT ID, NOME, SOBRENOME, DATANASCIMENTO FROM Pessoa
                             WHERE Id = @P1
                ";

                result = connection.QueryFirstOrDefault<Pessoa>(sql, new { P1 = id });
            }

            return result;
        }

        public List<Pessoa> GetByName(string nome)
        {
            List<Pessoa> result = new List<Pessoa>();

            using (var connection = new SqlConnection(this.ConnectionString))
            {

                var sql = @" SELECT Id, NOME, SOBRENOME, DATANASCIMENTO FROM Pessoa
                             WHERE NOME LIKE '%' + @P1 + '%' COLLATE SQL_Latin1_General_CP1_CI_AI OR SOBRENOME LIKE '%' + @P1 + '%' COLLATE SQL_Latin1_General_CP1_CI_AI
                ";

                result = connection.Query<Pessoa>(sql, new { P1 = nome.Trim() }).ToList();
            }

            return result;
        }

        public List<Pessoa> GetTodayBirthday()
        {
            List<Pessoa> result = new List<Pessoa>();
            DateTime date = DateTime.Today;

            using (var connection = new SqlConnection(this.ConnectionString))
            {

                var sql = @" SELECT Id, NOME, SOBRENOME, DATANASCIMENTO FROM Pessoa
                             WHERE MONTH(DATANASCIMENTO) = @P1 AND DAY(DATANASCIMENTO) = @P2
                ";

                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                result = connection.Query<Pessoa>(sql, new { P1 = date.Month, P2 = date.Day }).ToList();
            }

            return result.OrderBy(pessoa => pessoa.DataDeAniversario).ToList();
        }

        public List<Pessoa> GetNextBirthday()
        {
            List<Pessoa> result = new List<Pessoa>();
            DateTime date = DateTime.Today;

            using (var connection = new SqlConnection(this.ConnectionString))
            {

                var sql = @"SELECT Id, NOME, SOBRENOME, DATANASCIMENTO 
                            FROM Pessoa
                            WHERE (MONTH(DATANASCIMENTO) <> @P1 OR MONTH(DATANASCIMENTO) = @P1) 
                            AND (DAY(DATANASCIMENTO) <> @P2 OR DAY(DATANASCIMENTO) = @P2)
                ";

                result = connection.Query<Pessoa>(sql, new { P1 = date.Month, P2 = date.Day, P3 = date.Date }).ToList();
            }

            return result.OrderBy(pessoa => pessoa.DiferencaAniversario()).ToList();
        }
    }
}
