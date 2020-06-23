using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gerenciamento_aniversario_ASPNET.Models;
using Gerenciamento_aniversario_ASPNET.Repository;


namespace Gerenciamento_aniversario_ASPNET.Controllers
{
    public class PessoaController : Controller
    {
        private PessoaRepository PessoaRepository { get; set; }

        public PessoaController(PessoaRepository pessoaRepository)
        {
            this.PessoaRepository = pessoaRepository;
        }

        // GET: Pessoa
        [Route("Pessoas/HappyBirthday")]
        public ActionResult HappyBirthday()
        {
            DateTime dataDeHoje = DateTime.Today;
            var pessoa = PessoaRepository.GetAll().Where(pessoa => pessoa.DataDeAniversario.Day.Equals(dataDeHoje.Day) && pessoa.DataDeAniversario.Month.Equals(dataDeHoje.Month));

            return View(pessoa);
        }

        // GET: Pessoa
        [Route("Pessoa/")]
        public ActionResult Index()
        {
            var pessoa = PessoaRepository.ListaOrdenada();
            return View(pessoa);
        }

        // GET: Pessoa/Details/5
        [Route("Pessoas/Details/{id}")]
        public ActionResult Details(int id)
        {
            var pessoa = this.PessoaRepository.GetById(id);
            return View(pessoa);
        }

        //GET: Pessoa/Buscar
        [Route("Pessoas/Search")]
        public ActionResult Search(string nome)
        {
            var pessoa = PessoaRepository.BuscarPorNome(nome);
            return View(pessoa);
        }

        // GET: Pessoa/Create
        [Route("Pessoas/Create")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pessoa/Create
        [Route("Pessoas/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pessoa pessoa)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View();

                pessoa.DiasRestantes = pessoa.ProximoAniversario();
                PessoaRepository.Save(pessoa);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Pessoa/Edit/5
        [Route("Pessoas/Edit/{id}")]
        public ActionResult Edit(int id)
        {
            var pessoa = this.PessoaRepository.GetById(id);

            return View(pessoa);
        }

        // POST: Pessoa/Edit/5
        [Route("Pessoas/Edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Pessoa pessoa)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View();

                var pessoaEdit = PessoaRepository.GetById(id);

                pessoaEdit.Nome = pessoa.Nome;
                pessoaEdit.DataDeAniversario = pessoa.DataDeAniversario;

                PessoaRepository.Update(pessoaEdit);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Pessoa/Delete/5
        [Route("Pessoas/Delete/{id}")]
        public ActionResult Delete(int id)
        {
            var pessoa = this.PessoaRepository.GetById(id);
            return View(pessoa);
        }

        // POST: Pessoa/Delete/5
        [Route("Pessoas/Delete/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Pessoa pessoa)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View();

                PessoaRepository.Delete(pessoa);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}