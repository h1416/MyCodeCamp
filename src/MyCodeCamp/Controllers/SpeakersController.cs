﻿using AutoMapper;
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
        public ActionResult Get(string moniker)
        {
            try
            {
                var speakers = _repo.GetSpeakersByMoniker(moniker);

                return Ok(speakers);
            }
            catch
            {
            }

            return BadRequest();
        }

        [HttpGet("{id}")]
        public ActionResult Get(string moniker, int id)
        {
            try
            {
                var speaker = _repo.GetSpeaker(id);
                if (speaker == null) return NotFound();
                if (speaker.Camp.Moniker != moniker) return BadRequest("Speaker not specified camp");

                return Ok(speaker);
            }
            catch
            {
            }

            return BadRequest();
        }
    }
}
