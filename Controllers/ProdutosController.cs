using Microsoft.AspNetCore.Mvc;
using Api_dotnet.Data;
using Api_dotnet.Models;
using System.Linq;
namespace Api_dotnet.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProdutosController:ControllerBase
    {
        private readonly AplicationDBContext _database;   
        public ProdutosController (AplicationDBContext database){
            _database = database;
        }
        [HttpGet]
        public IActionResult PegarProdutos(){
            var produtos = _database.produtos.ToList();
            int tamanhoLista = produtos.Count;
            if(tamanhoLista > 0){
                Response.StatusCode = 200;
                return new ObjectResult(new {produtos});
            }
            else{
                Response.StatusCode = 404;
                return new ObjectResult(new {msg="Não existe nenhum produto registrado"});
            }
        }
        [HttpGet("{id}")]
        public IActionResult PegarProduto(int id){
            try{
                var produto = _database.produtos.First(p=> p.Id == id);
                Response.StatusCode = 302;
                return new ObjectResult(produto);
            }catch{
                Response.StatusCode = 404;
                return new ObjectResult(new {msg="O produto selecionado não foi encontrado"});
            }
        }
        [HttpPost]
        public IActionResult PostProduto([FromBody] ProdutoTemp produtoTemp){
            if(produtoTemp.Nome.Length <= 1){
                Response.StatusCode = 400;
                return new ObjectResult(new {msg = "O nome deve conter mais de um caracter"});
            }
            if(produtoTemp.Preco <=0){
                Response.StatusCode = 400;
                return new ObjectResult(new {msg ="O preço deve ser maior que 0"});
            }
            Produto  p = new Produto();
            p.Nome = produtoTemp.Nome;
            p.Preco = produtoTemp.Preco;
            _database.Add(p);
            _database.SaveChanges();
            Response.StatusCode = 201;
            return new ObjectResult("");// nesee verbo http ira retornar o status 201 que vai avisar ao navegador que o produto foi criado com suceso 
        }
        [HttpDelete("{id}")]
        public IActionResult DeletarProduto(int id){
            try{
                var produto = _database.produtos.First(p=> p.Id == id);
                _database.produtos.Remove(produto);
                _database.SaveChanges();
                Response.StatusCode = 204;
                return new ObjectResult("");
            }catch{
                Response.StatusCode = 404;
                return new ObjectResult(new {msg ="O produto selecionado não pode ser deletado"});
            }
        }
        [HttpPatch]
        public IActionResult Patch([FromBody] Produto prodMuda){
            if(prodMuda.Id > 0){
                if(prodMuda.Nome.Length <= 1){
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "O nome deve conter mais de um caracter"});
                }
                else if(prodMuda.Preco <=0){
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg ="O preço deve ser maior que 0"});
                }
                try{
                    var prodId = _database.produtos.First(produtoTemp=> produtoTemp.Id == prodMuda.Id);
                    if(prodId != null){
                        //editar produto
                        /*?=true
                        : false*/
                        prodId.Nome = prodMuda.Nome != null ? prodMuda.Nome : prodId.Nome;//condicao ? faz algo : faz outra coisa
                        prodId.Preco = prodMuda.Preco != 0 ? prodMuda.Preco : prodId.Preco;
                        _database.SaveChanges();
                        Response.StatusCode = 200;
                        return new ObjectResult("");
                    }
                    else{//caso tenha o id mas o produto seja null
                        Response.StatusCode = 404;
                        return new ObjectResult(new {msg="O prduto não pode ser encontrado"});
                    }
                }catch{//caso o id seja positivo porem o id não existe
                    Response.StatusCode = 404;
                    return new ObjectResult(new {msg="O prduto não pode ser encontrado"});
                }
            }else{//caso o id seja menor ou igual a zero
                Response.StatusCode = 404;
                return new ObjectResult(new {msg="O prduto não pode ser encontrado"});
            }
        }
    }
}