using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            var tarefa = _context.Tarefas.Find(id); //Pega o ID

            if (tarefa == null)
                return NotFound(); //Não encontrando ID retorna 404
            
            return Ok(tarefa);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            var tarefaBanco = _context.Tarefas.ToList(); //Retorna uma lista com todas as tarefas no DB

            return Ok(tarefaBanco);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            var tarefa = _context.Tarefas.Where(x => x.Titulo == titulo); //Pega os dados digitados

            return Ok(tarefa); //Retorna os dados encontrados
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date); //Pega a data digitada
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            var tarefa = _context.Tarefas.Where(x => x.Status == status); //Mostra o Enum pra escolha
            return Ok(tarefa);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" }); //Se estiver vazio

            _context.Add(tarefa); //Adiciona tarefa
            _context.SaveChanges(); //Salva no DB

            //Retorna a tarefa criada e seu ID
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            tarefaBanco.Id = tarefa.Id;
            tarefaBanco.Status = tarefa.Status;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Titulo = tarefa.Titulo;

            _context.Tarefas.Update(tarefaBanco); //Update no DB
            _context.SaveChanges(); //Salva as informações no DB

            return Ok(tarefaBanco);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            _context.Tarefas.Remove(tarefaBanco); //Remove as informações
            _context.SaveChanges(); //Salva 

            return NoContent(); //Mostra status 20* se não tiver erro
        }
    }
}
