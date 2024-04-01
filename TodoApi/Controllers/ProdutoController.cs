using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    // Substitua [controller] pelo nome do controlador, que é o nome de classe do controlador
    // menos o sufixo "Controlador" por convenção. Para esta amostra, o nome da classe do controlador é
    //  ProdutosController. Portanto, o nome do controlador é "Produtos".
    // O roteamento do ASP.NET Core não diferencia maiúsculas de minúsculas.
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly ProdutoContext _context;

        public ProdutosController(ProdutoContext context)
        {
            _context = context;
        }
        // O tipo de retorno dos metodos GET é tipo <ActionResult>, podendo representar uma ampla variedade de códigos de status HTTP.
        // Sendo estes: 1° se nenhum item corresponder à ID solicitada, o método retornará um código de erro Status 404NotFound e
        // Caso contrário, o método retornará 200 com um corpo de resposta JSON. O retorno de item resulta em uma resposta HTTP 200.
        // O ASP.NET Core serializa automaticamente o objeto em JSON e grava o JSON no corpo da mensagem de resposta.
        // O código de resposta para esse tipo de retorno é 200 OK, supondo que não haja nenhuma exceção sem tratamento.
        // As exceções sem tratamento são convertidas em erros 5xx.

        // O atributo [HttpGet] indica um método que responde a uma solicitação HTTP GET.
        // O caminho da URL de cada método é construído da seguinte maneira.
        // Se o atributo [HttpGet] tiver um modelo de rota (por exemplo, [HttpGet("products")]), acrescente isso ao caminho.
        // Porém esta não usa.
        // GET: api/Produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            return await _context.Produtos.ToListAsync();
        }

        // No método GetProduto a seguir, "{id}" é uma variável de espaço reservado para o identificador exclusivo do item pendente.
        // Quando GetProduto é invocado, o valor de "{id}" na URL é fornecido para o método no parâmetro id.
        // GET: api/Produtos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProduto(long id)
        {
            var Produto = await _context.Produtos.FindAsync(id);

            if (Produto == null)
            {
                return NotFound();
            }

            return Produto;
        }

        // PutProduto é semelhante a PostProduto, exceto pelo uso de HTTP PUT.
        // A resposta é 204 (Sem conteúdo). De acordo com a especificação de HTTP, uma solicitação PUT
        // exige que o cliente envie a entidade inteira atualizada, não apenas as alterações.
        // Para dar suporte a atualizações parciais, use HTTP PATCH.
        // Devera haver algum item dentro do banco para realizar está ação.
        // PUT: api/Produtos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(long id, Produto Produto)
        {
            if (id != Produto.Id)
            {
                return BadRequest();
            }

            _context.Entry(Produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Produtos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Produto>> PostProduto(Produto Produto)
        {
            _context.Produtos.Add(Produto);
            await _context.SaveChangesAsync();

            // return CreatedAtAction("GetProduto", new { id = Produto.Id }, Produto);
            // O código é um método HTTP POST, conforme indicado pelo atributo [HttpPost].
            // O método obtém o valor do Produto no corpo da solicitação HTTP.
            return CreatedAtAction(nameof(GetProduto), new { id = Produto.Id }, Produto);
            // O método CreatedAtAction: 
            // Retorna um código de status HTTP 201 em caso de êxito. HTTP 201 é a resposta padrão
            // para um método HTTP POST que cria um recurso no servidor.
            // Adiciona um cabeçalho de Local à resposta. O cabeçalho Location especifica o URI
            // do item de tarefas pendentes recém-criado. Para obter mais informações, confira 10.2.2 201 Criado.
            // Faz referência à ação GetProduto para criar o URI de 
            // Location do cabeçalho. A palavra-chave nameof do C# é usada para
            // evitar o hard-coding do nome da ação, na chamada CreatedAtAction.
        }

        // Exclui o Produto que tem o ID especificado.
        // Se não existir, a resposta é HTTP 204 No Content
        // DELETE: api/Produtos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(long id)
        {
            var Produto = await _context.Produtos.FindAsync(id);
            if (Produto == null)
            {
                return NotFound();
            }

            _context.Produtos.Remove(Produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProdutoExists(long id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }
    }
}
