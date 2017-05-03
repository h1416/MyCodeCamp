using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyCodeCamp.Data;
using MyCodeCamp.Models;
using System.Collections.Generic;

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
        public ActionResult Get(string moniker)
        {
            try
            {
                var speakers = _repo.GetSpeakersByMoniker(moniker);

                return Ok(_mapper.Map<IEnumerable<SpeakerModel>>(speakers));
            }
            catch
            {
            }

            return BadRequest();
        }

        [HttpGet("{id}", Name = "SpeakerGet")]
        public ActionResult Get(string moniker, int id)
        {
            try
            {
                var speaker = _repo.GetSpeaker(id);
                if (speaker == null) return NotFound();
                if (speaker.Camp.Moniker != moniker) return BadRequest("Speaker not specified camp");

                return Ok(_mapper.Map<SpeakerModel>(speaker));
            }
            catch
            {
            }

            return BadRequest();
        }
    }
}
