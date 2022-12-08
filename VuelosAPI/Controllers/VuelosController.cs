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
    }
}
