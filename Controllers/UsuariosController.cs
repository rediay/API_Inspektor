using Inspektor_API_REST.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;


namespace Inspektor_API_REST.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ILogger<UsuariosController> _logger;
        private readonly listasrestrictivas_riskconsultingcolombia_comContext _db;        

        public UsuariosController(ILogger<UsuariosController> logger, Models.listasrestrictivas_riskconsultingcolombia_comContext db)
        {
            _logger = logger;
            _db = db;
        }

        // GET: api/<UsuariosController>
        [HttpGet]
        public IEnumerable<Usuarios> Get()
        {
            var listaUsuarios = _db.Usuarios.ToList();
            return listaUsuarios;
        }

        // GET api/<UsuariosController>/5
        [HttpGet("{id}")]
        public Usuarios Get(int id)
        {
            var listaUsuarios = _db.Usuarios.Find(id);
            return listaUsuarios;
        }

        // POST api/<UsuariosController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UsuariosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsuariosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
