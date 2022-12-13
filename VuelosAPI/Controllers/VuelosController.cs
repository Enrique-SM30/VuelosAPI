using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VuelosAPI.Models;
using VuelosAPI.Repositories;

namespace VuelosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VuelosController : ControllerBase
    {
        private sistem21_salidasvuelosContext context;
        Repository<Vuelo> repository;

        public VuelosController(sistem21_salidasvuelosContext context)
        {
            this.context = context;
            repository = new(context);
        }
        [HttpGet]
        public IActionResult Get()
        {
            var data = repository.Get().OrderBy(x => x.Hora);
            return Ok(data);
        }
        [HttpPost]
        public IActionResult Post(Vuelo vuelo)
        {
            if (vuelo == null)
            {
                return BadRequest("Especifique el vuelo");
            }

            if (Validate(vuelo, out List<string> errores, true))
            {
                repository.Insert(vuelo);
                return Ok();
            }
            else
            {
                return BadRequest(errores);
            }
        }

        [HttpPut]
        public IActionResult Put(Vuelo vuelo)
        {
            if (vuelo == null)
            {
                return BadRequest("Especifique el vuelo");
            }

            if (Validate(vuelo, out List<string> errores, false))
            {
                var entidad = repository.Get(vuelo.Codigo);
                if (entidad == null)
                {
                    return NotFound();
                }
                entidad.Puerta = vuelo.Puerta;
                entidad.Estado = vuelo.Estado;
                entidad.Hora = vuelo.Hora;
                entidad.Destino = vuelo.Destino;
               

                repository.Update(entidad);
                return Ok();

            }
            else
            {
                return BadRequest(errores);
            }
        }

        [HttpDelete("{Codigo}")]
        public IActionResult Delete(string codigo)
        {
            var entidad = repository.Get(codigo);
            if (entidad == null)
            {
                return NotFound();
            }

            repository.Delete(entidad);
            return Ok();
        }


        private bool Validate(Vuelo vuelo, out List<string> errors, bool esPost)
        {
            errors = new List<string>();
            if (string.IsNullOrWhiteSpace(vuelo.Codigo))
                errors.Add("Escriba el codigo del vuelo");
            if(esPost==true)
            {
                if (repository.Get().Any(x => x.Codigo == vuelo.Codigo))
                    errors.Add("Este codigo de vuelo ya existe");
            }
            if (string.IsNullOrWhiteSpace(vuelo.Puerta))
                errors.Add("Escriba la puerta de abordaje del vuelo");
            if (string.IsNullOrWhiteSpace(vuelo.Estado))
                errors.Add("Escriba el status del vuelo");
            if (string.IsNullOrWhiteSpace(vuelo.Destino))
                errors.Add("Escriba el destino del vuelo");
            if (string.IsNullOrWhiteSpace(vuelo.Hora))
                errors.Add("Escriba el hora de abordaje del vuelo");

            return errors.Count == 0;
        }

    }
}
 