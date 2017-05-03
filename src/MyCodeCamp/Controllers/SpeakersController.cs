using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyCodeCamp.Data;

namespace MyCodeCamp.Controllers
{
    [Route("api/camps/{moniker}/speakers")]
    public class SpeakersController : BaseController
    {
        private ILogger<CampsController> _logger;
        private IMapper _mapper;
        private ICampRepository _repo;

        public SpeakersController(ICampRepository repo,
            ILogger<CampsController> logger,
            IMapper mapper)
        {
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public void Get(string moniker)
        {

        }
    }
}
