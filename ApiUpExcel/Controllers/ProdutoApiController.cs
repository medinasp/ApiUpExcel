using Data.Config;
using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiUpExcel.Controllers
{
    [Authorize]
    public class ProdutoApiController : Controller
    {
        private readonly IProduto IProduto;

        public ProdutoApiController(IProduto IProduto, DbContextOptions<ContextBase> optionsBuilder)
        {
            this.IProduto = IProduto;
            this.IProduto.SetContextOptions(optionsBuilder);
        }

        [HttpGet("/api/ListaProdutos")]
        public async Task<JsonResult> ListaProdutos()
        {
            return Json(await this.IProduto.List());
        }

        [HttpPost("/api/AdicionarProduto")]
        public async Task AdicionarProduto([FromBody] Produto produto)
        {
            await Task.FromResult(this.IProduto.Add(produto));
        }

        [Produces("application/json")]
        [HttpPut("/api/produtos/{id}")]
        public async Task<IActionResult> AtualizarProduto(int id, [FromBody] Produto produto)
        {
            if (produto == null || produto.Id != id)
            {
                return BadRequest();
            }

            var produtoExistente = await IProduto.GetEntityById(id);
            if (produtoExistente == null)
            {
                return NotFound();
            }

            produtoExistente.Nome = produto.Nome;
            produtoExistente.Imagem = produto.Imagem;

            await IProduto.Update(produtoExistente);

            return NoContent();
        }

        //[Authorize]
        [HttpPost("/api/DelProd")]
        public IActionResult Remover([FromBody] Produto produto)
        {
            IProduto.Delete(produto);
            return Ok();
        }

        public class DeleteProdutoRequest
        {
            public List<int> Ids { get; set; }
        }

        [HttpPost("api/excluirRangeProdutosPorIds")]
        public async Task<IActionResult> DeleteRange([FromBody] DeleteProdutoRequest request)
        {
            await IProduto.DeleteRange(request.Ids);
            return NoContent();
        }

        [HttpPost("api/adicionarRangeProdutos")]
        public IActionResult AddRange([FromBody] Produto[] produtos)
        {
            if (produtos == null || produtos.Length == 0)
            {
                return BadRequest("Nenhum produto fornecido");
            }

            IProduto.AddRange(produtos);

            return Ok("Produtos adicionados com sucesso");
        }
    }
}
