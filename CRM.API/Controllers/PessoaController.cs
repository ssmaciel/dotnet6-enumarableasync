using Bogus;
using CRM.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaController : ControllerBase
    {

        [HttpGet]
        public IEnumerable<Pessoa> GetPessoas()
        {
            foreach (var pessoa in GerarPessoas())
            {
                yield return pessoa;
                Task.Delay(100).Wait();
            }
        }

        [HttpGet("stream")]
        public async IAsyncEnumerable<Pessoa> GetPessoasStream()
        {
            foreach (var pessoa in GerarPessoas())
            {
                yield return pessoa;
                await Task.Delay(100);
            }
        }

        static List<Pessoa> GerarPessoas()
        {
            var pessoas = new List<Pessoa>();

            for (int i = 0; i < 10; i++)
            {
                var fake = new Faker<Bogus.Person>()
                    .CustomInstantiator(p => new Bogus.Person())
                    .RuleFor(u => u.FullName, (f, u) => f.Name.FullName())
                    .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
                    .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName).ToLower())
                    .Generate();

                pessoas.Add(new()
                {
                    Nome = fake.FullName,
                    Email = fake.Email,
                    Foto = fake.Avatar
                });
            }

            return pessoas;
        }
    }
}
