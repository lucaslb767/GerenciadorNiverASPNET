using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gerenciamento_aniversario_ASPNET.Models
{
    public class Pessoa
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataDeAniversario { get; set; }
        public int DiasRestantes { get; set; }
        public int ProximoAniversario()
        {
            DateTime momento = DateTime.Today;
            DateTime dataAniversario = new DateTime(momento.Year, DataDeAniversario.Month, DataDeAniversario.Day);

            if (dataAniversario < momento)
            {
                dataAniversario = dataAniversario.AddYears(1);
            }

            int diferancaData = (dataAniversario - momento).Days;
            return diferancaData;
        }
    }
}
